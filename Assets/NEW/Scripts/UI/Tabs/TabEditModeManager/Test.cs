using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pautik;

public class Test : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _maskConveyor;
    [SerializeField] private ConveyorReplacementManager _prefab;
    private RaycastHit _hit;

    private Vector3 _initialMousePosition;
    private Vector3 _previousPosition;
    private Vector3 _conveyorPosition;
    private Vector3 _nextConveyorPosition;
    private bool _isMousePositionInitialized;
    private float _distance;

    private Ray Ray => CameraPoint.ScreenPointToRay(_camera, Input.mousePosition);



    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(IsRaycastHit(Ray, Mathf.Infinity, _maskConveyor))
            {
                _conveyorPosition = _hit.collider.transform.position;
                _nextConveyorPosition = _conveyorPosition;
                _initialMousePosition = MousePosition();
                _isMousePositionInitialized = true;

                _distance = Vector3.Distance(CameraPoint.WorldPoint(_camera, _hit.collider.transform.position), CameraPoint.WorldPoint(_camera, _hit.collider.transform.position + Vector3.right));
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isMousePositionInitialized = false;
        }

        if (_isMousePositionInitialized)
        {
            Vector3 currentMousePosition = _initialMousePosition - MousePosition();
            currentMousePosition.y = 0;

            Vector3Int a = -Vector3Int.FloorToInt(currentMousePosition);

            float x = currentMousePosition.x;

            float v = 0;

            if (_previousPosition != a && _previousPosition.z >= a.z)
            {
                print(a);

                Vector3 direction = Vector3.back;
                Vector3 spawnPosition = _nextConveyorPosition + direction;

                if (References.Manager.TileCollection.Dict.ContainsKey(spawnPosition))
                {
                    if (!References.Manager.TileCollection.Dict[spawnPosition].TileOccupancyManager.IsCurrentTileOccupied)
                    {
                        ConveyorReplacementManager conveyorReplacementManager = InstantiateConveyor(spawnPosition);
                        _conveyorPosition = conveyorReplacementManager.transform.position;
                        _nextConveyorPosition = _conveyorPosition;
                    }
                }

                //_nextConveyorPosition += direction;
                _previousPosition = a;
            }
        }
    }

    private float P(float value)
    {
        float abs = Mathf.Abs(value);
        float rounded = Mathf.RoundToInt(value);
        return rounded;
    }

    private Vector3 MousePosition()
    {
        return CameraPoint.WorldPoint(_camera, Input.mousePosition);
    }

    private bool IsRaycastHit(Ray ray, float distance, LayerMask layerMask)
    {
        return Physics.Raycast(ray, out _hit, distance, layerMask);
    } 

    private ConveyorReplacementManager InstantiateConveyor(Vector3 position)
    {
        return Instantiate(_prefab, position, Quaternion.identity);
    }
}

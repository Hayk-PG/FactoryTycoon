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
            float y = (_initialMousePosition - MousePosition()).y;
            float z = currentMousePosition.z;
            float c = _hit.collider.transform.position.x;

            Vector3 direction = new Vector3(x, y, z).normalized;

            float v = 0;

            if (IsForwards(direction))
            {
                print($"Forward: {direction}");
            }

            if (IsBackwards(direction))
            {
                print($"Backwards: {direction}");
            }

            if (IsLeft(direction))
            {
                print($"Left: {direction}");
            }

            if (IsRight(direction))
            {
                print($"Right: {direction}");
            }

            //print($"{direction}");

            //if (direction.x < 0 && direction.y > 0 && direction.z > 0)
            //{
            //    print($"Backwards: {direction}");
            //}

            //if (direction.x > 0 && direction.y < 0 && direction.z < 0)
            //{
            //    print($"Forward: {direction}");
            //}

            //if (direction.x > 0 && direction.y > 0 && direction.z < 0)
            //{
            //    print($"Left: {direction}");
            //}

            //if (direction.x < 0 && direction.y < 0 && direction.z > 0)
            //{
            //    print($"Right: {direction}");
            //}

            //print(Vector3.Angle(currentMousePosition - _hit.collider.transform.position, _hit.collider.transform.forward) - 45);
            //Quaternion angle = Quaternion.LookRotation(currentMousePosition - _conveyorPosition, _hit.collider.transform.forward);
            //print(angle);

            if (_previousPosition != a/* && _previousPosition.z >= a.z*/)
            {
                Vector3 dir = IsForwards(direction) ? Vector3.forward : IsBackwards(direction) ? Vector3.back : IsLeft(direction) ? Vector3.left : IsRight(direction) ? Vector3.right : Vector3.zero;
                Vector3 spawnPosition = _nextConveyorPosition + dir;

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

    private bool AAA(Vector3 direction)
    {
        return direction.x >= -0.1f && direction.x <= 0.1f || direction.y >= -0.1f && direction.y <= 0.1f || direction.z >= -0.1f && direction.z <= 0.1f;
    }

    private bool IsBackwards(Vector3 direction)
    {
        if (AAA(direction))
        {
            return false;
        }

        return direction.x < 0 && direction.y > 0 && direction.z > 0;
    }

    private bool IsForwards(Vector3 direction)
    {
        if (AAA(direction))
        {
            return false;
        }

        return direction.x > 0 && direction.y < 0 && direction.z < 0;
    }

    private bool IsLeft(Vector3 direction)
    {
        if (AAA(direction))
        {
            return false;
        }

        return direction.x > 0 && direction.y > 0 && direction.z < 0;
    }

    private bool IsRight(Vector3 direction)
    {
        if (AAA(direction))
        {
            return false;
        }

        return direction.x < 0 && direction.y < 0 && direction.z > 0;
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

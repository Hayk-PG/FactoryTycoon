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
    private Vector3 _conveyorHitPosition;
    private Vector3 _conveyorSpawnPosition;
    private bool _isMousePositionInitialized;

    private Ray Ray => CameraPoint.ScreenPointToRay(_camera, Input.mousePosition);





    private void Update()
    {
        bool isMouseButtonDown = Input.GetMouseButtonDown(0);
        bool isMouseButtonReleased = Input.GetMouseButtonUp(0);
        bool isConveyorHit = IsRaycastHit(Ray, Mathf.Infinity, _maskConveyor);

        if (isMouseButtonDown && isConveyorHit)
        {
            bool canExtend = Get<ConveyorReplacementManager>.From(_hit.collider.gameObject).IsInputSection;

            GetConveyorHitPosition();
            AssignConveyorSpawnPosition(_conveyorHitPosition);
            SetInitialMousePosition(MousePosition());
            FindNearestInputSectionConveyors(canExtend);
        }

        if (isMouseButtonReleased)
        {
            DetectMousePositionInitialization(false);
        }

        if (_isMousePositionInitialized)
        {
            Vector3 currentMousePosition = _initialMousePosition - MousePosition();
            Vector3Int roundedMousePosition = -Vector3Int.FloorToInt(currentMousePosition);
            Vector3 direction = currentMousePosition.normalized;
            bool isMousePositionUpdated = _previousPosition != roundedMousePosition;

            if (isMousePositionUpdated)
            {
                Vector3 dir = IsForwards(direction) ? Vector3.forward : IsBackwards(direction) ? Vector3.back : IsLeft(direction) ? Vector3.left : IsRight(direction) ? Vector3.right : Vector3.zero;
                Vector3 spawnPosition = _conveyorSpawnPosition + dir;
                bool hasTile = References.Manager.TileCollection.Dict.ContainsKey(spawnPosition);

                if (hasTile)
                {
                    bool isTileOccupied = References.Manager.TileCollection.Dict[spawnPosition].TileOccupancyManager.IsCurrentTileOccupied;

                    if (!isTileOccupied)
                    {
                        ConveyorReplacementManager conveyorReplacementManager = InstantiateConveyor(spawnPosition);
                        _conveyorHitPosition = conveyorReplacementManager.transform.position;
                        _conveyorSpawnPosition = _conveyorHitPosition;
                    }
                }

                _previousPosition = roundedMousePosition;
            }
        }
    }

    private void GetConveyorHitPosition()
    {
        _conveyorHitPosition = _hit.collider.transform.position;
    }

    private void AssignConveyorSpawnPosition(Vector3 position)
    {
        _conveyorSpawnPosition = position;
    }

    private void SetInitialMousePosition(Vector3 position)
    {
        _initialMousePosition = position;
    }

    private void FindNearestInputSectionConveyors(bool canExtend)
    {
        if (canExtend)
        {
            DetectMousePositionInitialization(true);
        }
        else
        {
            if (References.Manager.ConveyorCollection.Dict.ContainsKey(_hit.collider.transform.position + Vector3.right))
            {
                if (References.Manager.ConveyorCollection.Dict[_hit.collider.transform.position + Vector3.right].IsInputSection)
                {
                    DetectMousePositionInitialization(true);
                    return;
                }
            }

            if (References.Manager.ConveyorCollection.Dict.ContainsKey(_hit.collider.transform.position + Vector3.left))
            {
                if (References.Manager.ConveyorCollection.Dict[_hit.collider.transform.position + Vector3.left].IsInputSection)
                {
                    DetectMousePositionInitialization(true);
                    return;
                }
            }

            if (References.Manager.ConveyorCollection.Dict.ContainsKey(_hit.collider.transform.position + Vector3.forward))
            {
                if (References.Manager.ConveyorCollection.Dict[_hit.collider.transform.position + Vector3.forward].IsInputSection)
                {
                    DetectMousePositionInitialization(true);
                    return;
                }
            }

            if (References.Manager.ConveyorCollection.Dict.ContainsKey(_hit.collider.transform.position + Vector3.back))
            {
                if (References.Manager.ConveyorCollection.Dict[_hit.collider.transform.position + Vector3.back].IsInputSection)
                {
                    DetectMousePositionInitialization(true);
                    return;
                }
            }
        }
    }

    private void DetectMousePositionInitialization(bool isInitialized)
    {
        _isMousePositionInitialized = isInitialized;
    }

    private bool CheckAcceptableDirection(Vector3 direction)
    {
        return direction.x >= -0.1f && direction.x <= 0.1f || direction.y >= -0.1f && direction.y <= 0.1f || direction.z >= -0.1f && direction.z <= 0.1f;
    }

    private bool IsBackwards(Vector3 direction)
    {
        if (CheckAcceptableDirection(direction))
        {
            return false;
        }

        return direction.x < 0 && direction.y > 0 && direction.z > 0;
    }

    private bool IsForwards(Vector3 direction)
    {
        if (CheckAcceptableDirection(direction))
        {
            return false;
        }

        return direction.x > 0 && direction.y < 0 && direction.z < 0;
    }

    private bool IsLeft(Vector3 direction)
    {
        if (CheckAcceptableDirection(direction))
        {
            return false;
        }

        return direction.x > 0 && direction.y > 0 && direction.z < 0;
    }

    private bool IsRight(Vector3 direction)
    {
        if (CheckAcceptableDirection(direction))
        {
            return false;
        }

        return direction.x < 0 && direction.y < 0 && direction.z > 0;
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

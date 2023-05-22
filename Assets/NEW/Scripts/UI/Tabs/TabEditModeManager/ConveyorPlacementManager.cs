using UnityEngine;
using Pautik;

public class ConveyorPlacementManager : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _conveyorMask;
    [SerializeField] private ConveyorSegment _conveyorPrefab;

    private RaycastHit _hitInfo;
    private Vector3 _initialMousePosition;
    private Vector3 _previousPosition;
    private Vector3 _conveyorHitPosition;
    private Vector3 _conveyorSpawnPosition;
    private bool _isMousePositionInitialized;

    private Ray Ray => CameraPoint.ScreenPointToRay(_camera, Input.mousePosition);


    

    private void Update()
    {
        ExecuteOnMouseDown(Input.GetMouseButtonDown(0));
        ExecuteOnMouseRelease(Input.GetMouseButtonUp(0));
        ProcessOnMousePositionInitialize(_isMousePositionInitialized);
    }

    private void ExecuteOnMouseDown(bool isMouseDown)
    {
        if (isMouseDown)
        {
            bool isHit = IsRaycastHit(Ray, Mathf.Infinity, _conveyorMask);

            if (isHit)
            {
                bool canExtend = References.Manager.ConveyorCollection.Dict[_hitInfo.collider.transform.position].IsInputSection;

                GetConveyorHitPosition();
                AssignConveyorSpawnPosition(_conveyorHitPosition);
                SetInitialMousePosition(MousePosition());
                FindNearestInputSectionConveyors(canExtend);
            }
        }
    }

    private void ExecuteOnMouseRelease(bool isMouseReleased)
    {
        if (isMouseReleased)
        {
            SetMousePositionInitialization(false);
        }
    }

    private void ProcessOnMousePositionInitialize(bool isMousePositionInitialized)
    {
        if (isMousePositionInitialized)
        {
            Vector3 currentMousePosition = _initialMousePosition - MousePosition();
            Vector3Int roundedMousePosition = -Vector3Int.FloorToInt(currentMousePosition);
            Vector3 direction = currentMousePosition.normalized;

            bool isMousePositionUpdated = _previousPosition != roundedMousePosition;

            if (isMousePositionUpdated)
            {
                Vector3 dir = GetDirection(direction);
                Vector3 spawnPosition = _conveyorSpawnPosition + dir;

                bool hasTile = References.Manager.TileCollection.Dict.ContainsKey(spawnPosition);

                if (dir == Vector3.zero)
                {
                    return;
                }

                SpawnConveyorAndUpdatePositions(hasTile, spawnPosition, roundedMousePosition);
            }
        }
    }

    private Vector3 GetDirection(Vector3 direction)
    {
        return IsForwards(direction) ? Vector3.forward : IsBackwards(direction) ? Vector3.back : IsLeft(direction) ? Vector3.left : IsRight(direction) ? Vector3.right : Vector3.zero;
    }

    private void SpawnConveyorAndUpdatePositions(bool hasTile, Vector3 spawnPosition, Vector3Int roundedMousePosition)
    {
        if (hasTile)
        {
            bool isTileOccupied = References.Manager.TileCollection.Dict[spawnPosition].TileOccupancyManager.IsCurrentTileOccupied;

            if (!isTileOccupied)
            {
                ConveyorSegment conveyor = InstantiateConveyor(spawnPosition);
                _conveyorHitPosition = conveyor.transform.position;
                _conveyorSpawnPosition = _conveyorHitPosition;
            }
        }

        _previousPosition = roundedMousePosition;
    }

    private void GetConveyorHitPosition()
    {
        _conveyorHitPosition = _hitInfo.collider.transform.position;
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
            SetMousePositionInitialization(true);

            return;
        }

        if (References.Manager.ConveyorCollection.Dict.ContainsKey(_hitInfo.collider.transform.position + Vector3.right))
        {
            if (References.Manager.ConveyorCollection.Dict[_hitInfo.collider.transform.position + Vector3.right].IsInputSection)
            {
                SetMousePositionInitialization(true);
                return;
            }
        }

        if (References.Manager.ConveyorCollection.Dict.ContainsKey(_hitInfo.collider.transform.position + Vector3.left))
        {
            if (References.Manager.ConveyorCollection.Dict[_hitInfo.collider.transform.position + Vector3.left].IsInputSection)
            {
                SetMousePositionInitialization(true);
                return;
            }
        }

        if (References.Manager.ConveyorCollection.Dict.ContainsKey(_hitInfo.collider.transform.position + Vector3.forward))
        {
            if (References.Manager.ConveyorCollection.Dict[_hitInfo.collider.transform.position + Vector3.forward].IsInputSection)
            {
                SetMousePositionInitialization(true);
                return;
            }
        }

        if (References.Manager.ConveyorCollection.Dict.ContainsKey(_hitInfo.collider.transform.position + Vector3.back))
        {
            if (References.Manager.ConveyorCollection.Dict[_hitInfo.collider.transform.position + Vector3.back].IsInputSection)
            {
                SetMousePositionInitialization(true);
                return;
            }
        }
    }

    private void SetMousePositionInitialization(bool isInitialized)
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
        return Physics.Raycast(ray, out _hitInfo, distance, layerMask);
    }

    private ConveyorSegment InstantiateConveyor(Vector3 position)
    {
        return Instantiate(_conveyorPrefab, position, Quaternion.identity);
    }
}

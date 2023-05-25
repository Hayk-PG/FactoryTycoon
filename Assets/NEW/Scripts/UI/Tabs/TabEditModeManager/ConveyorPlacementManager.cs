using UnityEngine;
using Pautik;

public class ConveyorPlacementManager : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _conveyorMask;
    [SerializeField] private ConveyorSegment _conveyorPrefab;

    private ConveyorSegment _hitConveyorSegment;
    private ConveyorSegment _newConveyorSegment;
    private RaycastHit _hitInfo;
    private Vector3 _initialMousePosition;
    private Vector3 _previousPosition;
    private Vector3 _conveyorHitPosition;
    private Vector3 _conveyorSpawnPosition;
    private bool _isMousePositionInitialized;

    private Ray Ray => CameraPoint.ScreenPointToRay(_camera, Input.mousePosition);


    

    private void Update()
    {
        // Executes logic when the mouse button is pressed down. 
        // Checks if a raycast hits a conveyor segment and performs related actions.
        ExecuteOnMouseDown(Input.GetMouseButtonDown(0));
        // Executes logic when the mouse button is released. Resets the mouse position initialization state.
        ExecuteOnMouseRelease(Input.GetMouseButtonUp(0));
        // Processes logic related to mouse position initialization. 
        // Calculates the current mouse position and direction and spawns a new conveyor segment if necessary.
        ProcessOnMousePositionInitialize(_isMousePositionInitialized);
    }

    // Executes logic when the mouse button is pressed down.
    private void ExecuteOnMouseDown(bool isMouseDown)
    {      
        if (isMouseDown)
        {
            // Check if a raycast hits a conveyor segment
            bool isHit = IsRaycastHit(Ray, Mathf.Infinity, _conveyorMask);

            if (isHit)
            {
                // Get the position of the conveyor segment that was hit
                GetConveyorHitPosition();
                // Assign the hit position as the spawn position for the new conveyor segment
                AssignConveyorSpawnPosition(_conveyorHitPosition);
                // Set the initial mouse position for calculating movement
                SetInitialMousePosition(MousePosition());
                // Find the nearest input section conveyors to handle extending logic
                FindNearestInputSectionConveyors();
            }
        }
    }

    // Executes logic when the mouse button is released.
    private void ExecuteOnMouseRelease(bool isMouseReleased)
    {
        if (isMouseReleased)
        {
            SetMousePositionInitialization(false);
        }
    }

    // Processes logic related to mouse position initialization.
    private void ProcessOnMousePositionInitialize(bool isMousePositionInitialized)
    {
        if (isMousePositionInitialized)
        {
            // Calculate the current mouse position relative to the initial mouse position
            Vector3 currentMousePosition = _initialMousePosition - MousePosition();
            // Round the current mouse position to the nearest integer values
            Vector3Int roundedMousePosition = -Vector3Int.FloorToInt(currentMousePosition);
            // Calculate the normalized direction of the mouse movement
            Vector3 direction = currentMousePosition.normalized;

            // Check if the mouse position has been updated since the last frame
            bool isMousePositionUpdated = _previousPosition != roundedMousePosition;

            if (isMousePositionUpdated)
            {
                // Determine the direction of conveyor placement based on the mouse movement direction
                Vector3 dir = GetDirection(direction);
                // Calculate the spawn position of the new conveyor segment
                Vector3 spawnPosition = _conveyorSpawnPosition + dir;

                // Check if there is a tile at the spawn position
                bool hasTile = References.Manager.TileCollection.Dict.ContainsKey(spawnPosition);

                // Skip spawning the conveyor segment if the direction is zero (no valid direction)
                if (dir == Vector3.zero)
                {
                    return;
                }

                // Spawn the conveyor segment and update relevant positions and directions
                SpawnConveyorAndUpdatePositions(hasTile, spawnPosition, roundedMousePosition, dir);
            }
        }
    }

    // Gets the direction based on the provided direction vector.
    private Vector3 GetDirection(Vector3 direction)
    {
        return IsForwards(direction) ? Vector3.forward : IsBackwards(direction) ? Vector3.back : IsLeft(direction) ? Vector3.left : IsRight(direction) ? Vector3.right : Vector3.zero;
    }

    // Spawns a new conveyor segment and updates related positions.
    private void SpawnConveyorAndUpdatePositions(bool hasTile, Vector3 spawnPosition, Vector3Int roundedMousePosition, Vector3 direction)
    {
        if (hasTile)
        {
            // Check if the tile at the spawn position is occupied
            bool isTileOccupied = References.Manager.TileCollection.Dict[spawnPosition].TileOccupancyManager.IsCurrentTileOccupied;

            if (!isTileOccupied)
            {
                // Instantiate a new conveyor segment at the spawn position
                _newConveyorSegment = InstantiateConveyor(spawnPosition);
                _newConveyorSegment.SetConveyorDirection(direction);
                _newConveyorSegment.UpdatePreviousConveyorDirection(_hitConveyorSegment, direction);
                _conveyorHitPosition = _newConveyorSegment.transform.position;
                _conveyorSpawnPosition = _conveyorHitPosition;
            }
        }

        _previousPosition = roundedMousePosition;
    }

    // Retrieves the position where the raycast hits the conveyor.
    private void GetConveyorHitPosition()
    {
        _conveyorHitPosition = _hitInfo.collider.transform.position;
    }

    // Assigns the conveyor spawn position.
    private void AssignConveyorSpawnPosition(Vector3 position)
    {
        _conveyorSpawnPosition = position;
    }

    // Sets the initial mouse position.
    private void SetInitialMousePosition(Vector3 position)
    {
        _initialMousePosition = position;
    }

    // Finds the nearest input section conveyors.
    private void FindNearestInputSectionConveyors()
    {
        _hitConveyorSegment = References.Manager.ConveyorCollection.Dict[_hitInfo.collider.transform.position];

        bool isOutputSection = _hitConveyorSegment.IsOutputSection;
        bool canExtend = isOutputSection && _hitConveyorSegment.CanExtendWithFewNeighbors();

        if (canExtend)
        {
            SetMousePositionInitialization(true);

            return;
        }

        //for (int i = 0; i < AdjacentPositionCalculator.GetAdjacentPositions(transform.position).Length; i++)
        //{
        //    Vector3 adjacentPosition = AdjacentPositionCalculator.GetAdjacentPositions(transform.position)[i];
        //    Checker.IsValueInDictionary(References.Manager.ConveyorCollection.Dict, adjacentPosition, out ConveyorSegment conveyorSegment);

        //    if(conveyorSegment != null && conveyorSegment.IsOutputSection)
        //    {
        //        SetMousePositionInitialization(true);
        //        return;
        //    }
        //}    
    }

    // Sets the mouse position initialization state.
    private void SetMousePositionInitialization(bool isInitialized)
    {
        _isMousePositionInitialized = isInitialized;
    }

    // Checks if the provided direction vector falls within the acceptable range for each direction.
    private bool CheckAcceptableDirection(Vector3 direction)
    {
        return direction.x >= -0.1f && direction.x <= 0.1f || direction.y >= -0.1f && direction.y <= 0.1f || direction.z >= -0.1f && direction.z <= 0.1f;
    }

    // Checks if the direction vector represents a backward direction.
    private bool IsBackwards(Vector3 direction)
    {
        if (CheckAcceptableDirection(direction))
        {
            return false;
        }

        return direction.x < 0 && direction.y > 0 && direction.z > 0;
    }

    // Checks if the direction vector represents a forward direction.
    private bool IsForwards(Vector3 direction)
    {
        if (CheckAcceptableDirection(direction))
        {
            return false;
        }

        return direction.x > 0 && direction.y < 0 && direction.z < 0;
    }

    // Checks if the direction vector represents a left direction.
    private bool IsLeft(Vector3 direction)
    {
        if (CheckAcceptableDirection(direction))
        {
            return false;
        }

        return direction.x > 0 && direction.y > 0 && direction.z < 0;
    }

    // Checks if the direction vector represents a right direction.
    private bool IsRight(Vector3 direction)
    {
        if (CheckAcceptableDirection(direction))
        {
            return false;
        }

        return direction.x < 0 && direction.y < 0 && direction.z > 0;
    }

    // Retrieves the mouse position in world coordinates.
    private Vector3 MousePosition()
    {
        return CameraPoint.WorldPoint(_camera, Input.mousePosition);
    }

    // Performs a raycast and checks if it hits an object with the specified layer mask.
    private bool IsRaycastHit(Ray ray, float distance, LayerMask layerMask)
    {
        return Physics.Raycast(ray, out _hitInfo, distance, layerMask);
    }

    // Instantiates a new conveyor segment at the specified position.
    private ConveyorSegment InstantiateConveyor(Vector3 position)
    {
        return Instantiate(_conveyorPrefab, position, Quaternion.identity);
    }
}

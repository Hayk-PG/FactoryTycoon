using UnityEngine;
using Pautik;
using System;
using System.Collections.Generic;

public class ObjectPlacementValidator : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _tileLayerMask;
    private RaycastHit _hit;

    private TileHighlightManager _respondedTile;
    private List<TileHighlightManager> _allRequiredTiles = new List<TileHighlightManager>();
    private bool _isResponded;

    private Ray Ray => CameraPoint.ScreenPointToRay(_camera, Input.mousePosition);

    public event Action<Vector3, SelectableObjectInfo, Transform, ObjectPlacementValidator> OnObjectPlacementValidationRequest;




    /// <summary>
    /// // Requests object placement validation for the selected object
    /// </summary>
    /// <param name="selectedObject"></param>
    public void RequestObjectPlacementValidation(SelectableObjectInfo selectedObject, Transform dummy)
    {
        if (IsRaycastHit(Ray, Mathf.Infinity, _tileLayerMask))
        {
            RaiseObjectPlacementValidationRequestEvent(_hit.transform.position, selectedObject, dummy);
        }
    }

    // Checks if a raycast hit occurs
    private bool IsRaycastHit(Ray ray, float distance, LayerMask layerMask)
    {
        return Physics.Raycast(ray, out _hit, distance, layerMask);
    }

    // Raises the object placement validation request event
    public void RaiseObjectPlacementValidationRequestEvent(Vector3 position, SelectableObjectInfo selectedObject, Transform dummy = null)
    {
        OnObjectPlacementValidationRequest?.Invoke(position, selectedObject, dummy, this);
    }

    /// <summary>
    /// Responds to object placement validation request
    /// </summary>
    /// <param name="tileHighlightManager">The tile highlight manager of the selected tile</param>
    /// <param name="selectedObject">The selected object</param>
    public void RespondObjectPlacementValidationRequest(TileHighlightManager tileHighlightManager, SelectableObjectInfo selectedObject, Transform dummy)
    {
        bool isSameTile = _respondedTile == tileHighlightManager;

        if (isSameTile)
        {
            return;
        }

        // Set the new responder tile
        GetResponderTile(tileHighlightManager);

        // Reset the highlighting of previously required tiles
        ResetPreviouslyRequiredTiles();

        // Initialize the required tiles list
        InitializeRequiredTilesList();

        // Store all the required tiles for object placement
        StoreAllRequiredTiles(selectedObject);

        // Try to highlight or block the required tiles based on the placement validity
        TryHighlightTiles(dummy);
    }

    /// <summary>
    /// Places the selected object in the game world.
    /// </summary>
    /// <param name="selectedObject">The selected object to place.</param>
    /// <param name="dummy">The dummy object representing the placement location.</param>
    public void PlaceSelectedObject(SelectableObjectInfo selectedObject = null, Transform dummy = null)
    {
        SetDummyActive(dummy, false);

        if (!_isResponded)
        {
            return;
        }

        InstantiateSelectedObject(selectedObject);
        SetTilesOccupied(true);
    }

    // Sets the current responder tile
    private void GetResponderTile(TileHighlightManager tileHighlightManager)
    {
        _respondedTile = tileHighlightManager;
        _isResponded = true;
    }

    // Resets the highlighting of previously responded tiles
    private void ResetPreviouslyRequiredTiles()
    {
        foreach (var requiredTile in _allRequiredTiles)
        {
            requiredTile.ResetHighlight();
        }
    }

    // Initializes the list of required tiles
    private void InitializeRequiredTilesList()
    {
        _allRequiredTiles = new List<TileHighlightManager>();
    }

    // Stores all required tiles for object placement
    private void StoreAllRequiredTiles(SelectableObjectInfo selectedObject)
    {
        // Iterate over each dimension of the selected object
        foreach (var dimension in selectedObject.Dimension)
        {
            // Calculate the position of the neighbor tile
            Vector3 neighborTilePosition = _respondedTile.transform.position + dimension;

            // Check if the neighbor tile is a valid tile (exists and not occupied)
            bool isValidTile = References.Manager.TileCollection.Dict.ContainsKey(neighborTilePosition) && !References.Manager.TileCollection.Dict[neighborTilePosition].TileOccupancyManager.IsCurrentTileOccupied;

            // Compare the validity of the tile using a conditional statement
            Conditions<bool>.Compare(isValidTile, () => AddTile(References.Manager.TileCollection.Dict[neighborTilePosition].TileHighlightManager), () => _isResponded = false);

            // Add the current tile to the required tiles list
            AddTile(_respondedTile);         
        }
    }

    private void AddTile(TileHighlightManager tileHighlightManager)
    {
        _allRequiredTiles.Add(tileHighlightManager);
    }

    // Tries to highlight the required tiles if all requirements are met, otherwise blocks the tiles
    private void TryHighlightTiles(Transform dummy)
    {
        Conditions<bool>.Compare(_isResponded, ()=> HighlightTiles(dummy), ()=> BlockTiles(dummy));
    }

    // Highlights all required tiles
    private void HighlightTiles(Transform dummy)
    {
        foreach (var highlightedTile in _allRequiredTiles)
        {
            highlightedTile.Highlight();
        }

        SetDummyActive(dummy, true);
    }

    // Block all required tiles
    private void BlockTiles(Transform dummy)
    {
        foreach (var highlightedTile in _allRequiredTiles)
        {
            highlightedTile.Block();
        }

        SetDummyActive(dummy, false);
    }

    /// <summary>
    /// Instantiates the selected object at the placement location.
    /// </summary>
    /// <param name="selectedObject">The selected object to instantiate.</param>
    private void InstantiateSelectedObject(SelectableObjectInfo selectedObject)
    {
        bool hasSelectedObject = selectedObject != null;

        if (!hasSelectedObject)
        {
            return;
        }

        SelectableObjectInfo newObject = Instantiate(selectedObject, _respondedTile.transform.position, Quaternion.identity);
        newObject.ObjectReplacementAnimation.StartReplacementAnimation();
    }

    /// <summary>
    /// Sets the occupied state of the required tiles.
    /// </summary>
    /// <param name="isOccupied">The desired occupied state.</param>
    private void SetTilesOccupied(bool isOccupied)
    {
        foreach (var tile in _allRequiredTiles)
        {
            tile.SetTileOccupied(isOccupied);
        }
    }

    /// <summary>
    /// Sets the position and active state of a dummy object.
    /// </summary>
    /// <param name="dummy">The dummy object to set.</param>
    /// <param name="isActive">The desired active state of the dummy object.</param>
    private void SetDummyActive(Transform dummy, bool isActive)
    {
        bool hasDummy = dummy != null;

        if (!hasDummy)
        {
            return;
        }

        dummy.position = _respondedTile.transform.position + Vector3.up / 2;
        dummy.gameObject.SetActive(isActive);
    }
}

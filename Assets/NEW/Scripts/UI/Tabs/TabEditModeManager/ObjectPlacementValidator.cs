using UnityEngine;
using Pautik;
using System;
using System.Collections.Generic;

public class ObjectPlacementValidator : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _tileLayerMask;
    private RaycastHit _hit;

    private TileHighlightManager _responderTile;
    private List<TileHighlightManager> _allRequiredTiles = new List<TileHighlightManager>();
    private bool _isResponded;

    private Ray Ray => CameraPoint.ScreenPointToRay(_camera, Input.mousePosition);

    public event Action<Vector3, SelectableObjectInfo, ObjectPlacementValidator> OnObjectPlacementValidationRequest;




    /// <summary>
    /// // Requests object placement validation for the selected object
    /// </summary>
    /// <param name="selectedObject"></param>
    public void RequetObjectPlacementValidation(SelectableObjectInfo selectedObject)
    {
        if (IsRaycastHit(Ray, Mathf.Infinity, _tileLayerMask))
        {
            RaiseObjectPlacementValidationRequestEvent(_hit.transform.position, selectedObject);
        }
    }

    // Checks if a raycast hit occurs
    private bool IsRaycastHit(Ray ray, float distance, LayerMask layerMask)
    {
        return Physics.Raycast(ray, out _hit, distance, layerMask);
    }

    // Raises the object placement validation request event
    private void RaiseObjectPlacementValidationRequestEvent(Vector3 position, SelectableObjectInfo selectedObject)
    {
        OnObjectPlacementValidationRequest?.Invoke(position, selectedObject, this);
    }

    /// <summary>
    /// Responds to object placement validation request
    /// </summary>
    /// <param name="tileHighlightManager">The tile highlight manager of the selected tile</param>
    /// <param name="selectedObject">The selected object</param>
    public void RespondObjectPlacementValidationRequest(TileHighlightManager tileHighlightManager, SelectableObjectInfo selectedObject)
    {
        bool isSameTile = _responderTile == tileHighlightManager;

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
        TryHighlightTiles();
    }

    // Sets the current responder tile
    private void GetResponderTile(TileHighlightManager tileHighlightManager)
    {
        _responderTile = tileHighlightManager;
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
            Vector3 neighborTilePosition = _responderTile.transform.position + dimension;

            // Check if the neighbor tile is a valid tile (exists and not occupied)
            bool isValidTile = References.Manager.TileCollection.Dict.ContainsKey(neighborTilePosition) && !References.Manager.TileCollection.Dict[neighborTilePosition].TileOccupancyManager.IsCurrentTileOccupied;

            // Compare the validity of the tile using a conditional statement
            Conditions<bool>.Compare(isValidTile, () => AddTile(References.Manager.TileCollection.Dict[neighborTilePosition].TileHighlightManager), () => _isResponded = false);

            // Add the current tile to the required tiles list
            AddTile(_responderTile);         
        }
    }

    private void AddTile(TileHighlightManager tileHighlightManager)
    {
        _allRequiredTiles.Add(tileHighlightManager);
    }

    // Tries to highlight the required tiles if all requirements are met, otherwise blocks the tiles
    private void TryHighlightTiles()
    {
        Conditions<bool>.Compare(_isResponded, HighlightTiles, BlockTiles);
    }

    // Highlights all required tiles
    private void HighlightTiles()
    {
        foreach (var highlightedTile in _allRequiredTiles)
        {
            highlightedTile.Highlight();
        }
    }

    // Block all required tiles
    private void BlockTiles()
    {
        foreach (var highlightedTile in _allRequiredTiles)
        {
            highlightedTile.Block();
        }
    }
}

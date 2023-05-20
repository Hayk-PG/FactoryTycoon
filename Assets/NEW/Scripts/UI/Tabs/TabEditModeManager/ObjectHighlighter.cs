using UnityEngine;
using Pautik;
using System;
using System.Collections.Generic;

public class ObjectHighlighter : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _tileLayerMask;
    private RaycastHit _hit;
    private TileHighlightManager _highlightedTile;
    private List<TileHighlightManager> _highlightedTiles = new List<TileHighlightManager>();
    private bool _isHighlighted;

    private Ray Ray => CameraPoint.ScreenPointToRay(_camera, Input.mousePosition);

    public event Action<Vector3, SelectableObjectInfo, ObjectHighlighter> OnTileHighlight;
    
    
    
    
    public void DetectRayCastHit(SelectableObjectInfo selectedObject)
    {
        if (IsRaycastHit(Ray, Mathf.Infinity, _tileLayerMask))
        {
            RaiseOnTileHighlightEvent(_hit.transform.position, selectedObject);
        }
    }

    private bool IsRaycastHit(Ray ray, float distance, LayerMask layerMask)
    {
        return Physics.Raycast(ray, out _hit, distance, layerMask);
    }

    private void RaiseOnTileHighlightEvent(Vector3 position, SelectableObjectInfo selectedObject)
    {
        OnTileHighlight?.Invoke(position, selectedObject, this);
    }

    public void ToggleHighlightedTiles(TileHighlightManager tileHighlightManager, SelectableObjectInfo selectedObject)
    {
        if(_highlightedTile == tileHighlightManager)
        {
            return;
        }

        _highlightedTile = tileHighlightManager;      
        _isHighlighted = true;

        foreach (var highlightedTile in _highlightedTiles)
        {
            highlightedTile.ResetHighlight();
        }

        _highlightedTiles = new List<TileHighlightManager>();

        foreach (var dimension in selectedObject.Dimension)
        {
            Vector3 neighbourTilePosition = _highlightedTile.transform.position + dimension;

            if (References.Manager.TileCollection.Dict.ContainsKey(neighbourTilePosition) && !References.Manager.TileCollection.Dict[neighbourTilePosition].TileOccupancyManager.IsCurrentTileOccupied)
            {
                _highlightedTiles.Add(References.Manager.TileCollection.Dict[neighbourTilePosition].TileHighlightManager);
            }
            else
            {
                _isHighlighted = false;
            }
        }

        _highlightedTiles.Add(_highlightedTile);

        if (_isHighlighted)
        {
            foreach (var highlightedTile in _highlightedTiles)
            {
                highlightedTile.Highlight();
            }
        }
        else
        {
            foreach (var highlightedTile in _highlightedTiles)
            {
                highlightedTile.Block();
            }
        }
    }
}

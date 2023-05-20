using UnityEngine;

public class TileHighlightManager  : MonoBehaviour
{
    [Header("Tile Highlighting")]
    [SerializeField] private MeshRenderer _meshRenderer;

    [Header("Colors")]
    [SerializeField] private Color _colorNormal;
    [SerializeField] private Color _colorInEditMode;
    [SerializeField] private Color _colorHighlighted;
    [SerializeField] private Color _blockedHighlightColor;

    [Header("Tile Occupation Detector")]
    [SerializeField] private TileOccupancyManager _tileOccupancyManager;
    
    
    
    
    private void OnEnable()
    {
        References.Manager.EditModeManager.OnEditMode += OnEditMode;
        References.Manager.ObjectHighlighter.OnTileHighlight += ToggleTileHighlightAtPosition;
    }

    private void OnEditMode(bool isEditModeActive)
    {
        ToggleTileHighlight(isEditModeActive);
    }

    private void ToggleTileHighlight(bool isHighlighted)
    {
        if (!isHighlighted)
        {
            ChangeMaterialColor(_colorNormal);
            return;
        }

        if (IsCurrentTileOccupied())
        {
            ChangeMaterialColor(_blockedHighlightColor);
        }
        else
        {
            ChangeMaterialColor(_colorInEditMode);
        }
    }

    private void ToggleTileHighlightAtPosition(Vector3 position)
    {
        bool isCurrentTileHighlighted = transform.position == position;

        if (isCurrentTileHighlighted)
        {
            _meshRenderer.material.color = _colorHighlighted;
        }
        else
        {
            _meshRenderer.material.color = _colorNormal;
        }
    }

    private void ChangeMaterialColor(Color color)
    {
        _meshRenderer.material.color = color;
    }

    private bool IsCurrentTileOccupied()
    {
        return _tileOccupancyManager.IsCurrentTileOccupied;
    }
}

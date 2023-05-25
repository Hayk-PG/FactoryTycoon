using UnityEngine;
using Pautik;

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

    private bool _isEditModeActive;
    
    
    
    
    private void OnEnable()
    {
        // Subscribe to the edit mode event and object placement validation request event
        References.Manager.EditModeManager.OnEditMode += OnEditMode;
        References.Manager.ObjectPlacementValidator.OnObjectPlacementValidationRequest += OnObjectPlacementValidationRequest;
    }

    // If not in edit mode, set the normal color
    private void OnEditMode(bool isEditModeActive)
    {
        _isEditModeActive = isEditModeActive;

        // Compare the current tile's occupancy status and set the appropriate color
        Conditions<bool>.Compare(IsCurrentTileOccupied(), () => ChangeMaterialColor(_blockedHighlightColor), () => ChangeMaterialColor(_colorInEditMode));
    }

    // Respond to the object placement validation request
    private void OnObjectPlacementValidationRequest(Vector3 position, SelectableObjectInfo selectedObject, Transform dummy, ObjectPlacementValidator objectPlacementValidator)
    {
        if(transform.position != position)
        {
            return;
        }

        objectPlacementValidator.RespondObjectPlacementValidationRequest(this, selectedObject, dummy);
    }

    /// <summary>
    /// Reset the highlight color based on the current tile's occupancy status
    /// </summary>
    public void ResetHighlight()
    {
        Conditions<bool>.Compare(IsCurrentTileOccupied(), () => ChangeMaterialColor(_blockedHighlightColor), () => ChangeMaterialColor(_colorInEditMode));
    }

    /// <summary>
    /// Set the highlight color based on the current tile's occupancy status
    /// </summary>
    public void Highlight()
    {
        Conditions<bool>.Compare(IsCurrentTileOccupied(), () => ChangeMaterialColor(_blockedHighlightColor), () => ChangeMaterialColor(_colorHighlighted));
    }

    /// <summary>
    /// Set the block color
    /// </summary>
    public void Block()
    {
        ChangeMaterialColor(_blockedHighlightColor);
    }

    /// <summary>
    /// Sets the occupied state of the tile.
    /// </summary>
    /// <param name="isOccupied">The desired occupied state.</param>
    public void SetTileOccupied(bool isOccupied)
    {
        _tileOccupancyManager.IsCurrentTileOccupied = isOccupied;
        ResetHighlight();
    }

    // Change the material color to the specified color
    private void ChangeMaterialColor(Color color)
    {
        if (!_isEditModeActive)
        {
            _meshRenderer.material.color = _colorNormal;

            return;
        }

        _meshRenderer.material.color = color;
    }

    // Check if the current tile is occupied
    private bool IsCurrentTileOccupied()
    {
        return _tileOccupancyManager.IsCurrentTileOccupied;
    }
}

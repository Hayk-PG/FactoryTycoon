using UnityEngine;

public class TileHighlightManager  : MonoBehaviour
{
    [Header("Tile Highlighting")]
    [SerializeField] private MeshRenderer _meshRenderer;
    [Header("Colors")]
    [SerializeField] private Color _colorNormal;
    [SerializeField] private Color _colorHighlighted;
    [SerializeField] private Color _blockedHighlightColor;




    private void OnEnable()
    {
        GameSceneObjectsReferences.Manager.ObjectHighlighter.OnTileHighlight += OnHighlight;
    }

    private void OnHighlight(Vector3 position)
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
}

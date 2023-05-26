using UnityEngine;

public class TileManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TileOccupancyManager _tileOccupancyManager;
    [SerializeField] private TileHighlightManager _tileHighlightManager;

    [Header("Parent: Isle Manager")]
    [SerializeField] private IsleManager _isleManager; // Reference to the parent IsleManager component responsible for managing the tilemap and isle-related functionality.

    public TileOccupancyManager TileOccupancyManager => _tileOccupancyManager;
    public TileHighlightManager TileHighlightManager => _tileHighlightManager;
    public IsleManager IsleManager => _isleManager;

    public Vector3 Id => transform.position;
}
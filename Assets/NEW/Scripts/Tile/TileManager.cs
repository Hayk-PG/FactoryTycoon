using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] private TileOccupancyManager _tileOccupancyManager;
    [SerializeField] private TileHighlightManager _tileHighlightManager;

    public TileOccupancyManager TileOccupancyManager => _tileOccupancyManager;
    public TileHighlightManager TileHighlightManager => _tileHighlightManager;

    public Vector3 Id => transform.position;
}

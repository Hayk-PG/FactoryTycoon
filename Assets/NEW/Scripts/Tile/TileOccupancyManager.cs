using UnityEngine;

public class TileOccupancyManager : MonoBehaviour
{
    [SerializeField] private bool _isCurrentTileOccupied;

    public bool IsCurrentTileOccupied
    {
        get => _isCurrentTileOccupied;
        set => _isCurrentTileOccupied = value;
    }




    public void DetectTileOccupancy()
    {

    }
}

using UnityEngine;

[ExecuteInEditMode]
public class EditorTileMapBuilder : MonoBehaviour
{
    [SerializeField] private Transform _tile;
    [SerializeField] private Transform[] _placableTiles;
    [SerializeField] private Transform _ground;
    [SerializeField] private Vector3 _relativePosition;
    [SerializeField] private Vector3 _dimension;
    [SerializeField] private int _tileSize;
    private int _activePlacableTileIndex;
    [SerializeField] private bool _build;   
    
    
    
    
    private void Update()
    {
        if (!_build)
        {
            return;
        }

        ResetActivePlacableTileIndex();
        BuildTileMap();

        _build = false;
    }

    // Iterate over the X and Z dimensions of the tile map
    private void BuildTileMap()
    {       
        for (float x = 0; x < _dimension.x; x += _tileSize)
        {
            for (float z = 0; z > _dimension.z; z -= _tileSize)
            {
                bool hasPlacableTiles = _placableTiles != null && _placableTiles.Length > 0;

                if (hasPlacableTiles)
                {
                    PlaceTiles(x, z);
                }
                else
                {
                    InstantiateTile(x, z);
                }                
            }
        }
    }

    // Instantiate a tile at the specified position relative to the ground
    private void InstantiateTile(float x, float z)
    {
        Transform tile = Instantiate(_tile, _relativePosition + new Vector3(x, 0, z), Quaternion.identity, _ground);
        tile.name = _tile.name;
    }

    // Place a tile at the specified position relative to the ground
    private void PlaceTiles(float x, float z)
    {
        bool isIndexOutOfRange = _activePlacableTileIndex >= _placableTiles.Length;

        if (isIndexOutOfRange)
        {
            return;
        }

        _placableTiles[_activePlacableTileIndex].transform.position = _relativePosition + new Vector3(x, 0, z);
        _placableTiles[_activePlacableTileIndex].transform.SetParent(_ground);
        _activePlacableTileIndex++;
    }

    private void ResetActivePlacableTileIndex()
    {
        _activePlacableTileIndex = 0;
    }
}

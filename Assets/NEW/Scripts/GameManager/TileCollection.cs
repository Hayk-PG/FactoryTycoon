using System.Collections.Generic;
using UnityEngine;
using Pautik;

public class TileCollection : MonoBehaviour
{
    public Dictionary<Vector3, TileManager> Dict = new Dictionary<Vector3, TileManager>();



    private void Awake()
    {
        GetAllTiles();
    }

    public void GetAllTiles()
    {
        GlobalFunctions.Loop<TileManager>.Foreach(FindObjectsOfType<TileManager>(), tileManager => Dict.Add(tileManager.transform.position, tileManager));
    }

    public void AddTile(KeyValuePair<Vector3, TileManager> tile)
    {
        if (Dict.ContainsKey(tile.Key))
        {
            return;
        }

        Dict.Add(tile.Key, tile.Value);
    }

    public void RemoveTile(Vector3 key)
    {
        if (!Dict.ContainsKey(key))
        {
            return;
        }

        Dict.Remove(key);
    }
}

using UnityEngine;
using Pautik;

public class TileCollection : BaseCollection<Vector3, TileManager>
{
    protected override void InitializeCollection()
    {
        GlobalFunctions.Loop<TileManager>.Foreach(FindObjectsOfType<TileManager>(), tileManager => Dict.Add(tileManager.transform.position, tileManager));
    }
}

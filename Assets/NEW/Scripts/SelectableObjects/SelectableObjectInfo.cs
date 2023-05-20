using UnityEngine;

public class SelectableObjectInfo : MonoBehaviour
{
    [SerializeField] private Vector3 _objectSizeInTiles;

    public Vector3 ObjectSizeInTiles => _objectSizeInTiles;
}

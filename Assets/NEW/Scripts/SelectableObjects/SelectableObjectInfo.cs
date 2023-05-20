using UnityEngine;

public class SelectableObjectInfo : MonoBehaviour
{
    [SerializeField] private Vector3[] _dimension;

    public Vector3[] Dimension => _dimension;
}

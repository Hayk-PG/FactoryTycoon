using UnityEngine;

public class SelectableObjectInfo : MonoBehaviour
{
    [Header("The Object Scale Related To Tiles")]
    [SerializeField] private Vector3[] _dimension;

    [Header("Components")]
    [SerializeField] private ObjectReplacementAnimation _objectReplacementAnimation;

    public ObjectReplacementAnimation ObjectReplacementAnimation => _objectReplacementAnimation;
    public Vector3[] Dimension => _dimension;
}

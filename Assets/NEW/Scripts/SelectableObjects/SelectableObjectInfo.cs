using UnityEngine;

public class SelectableObjectInfo : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ObjectReplacementAnimation _objectReplacementAnimation;

    [Header("Isle")]
    [SerializeField] private IsleManager _isleManager;

    [Header("The Object Scale Related To Tiles")]
    [SerializeField] private Vector3[] _dimension;

    public ObjectReplacementAnimation ObjectReplacementAnimation => _objectReplacementAnimation;
    public IsleManager IsleManager
    {
        get => _isleManager;
        set => _isleManager = value;
    }
    public Vector3[] Dimension => _dimension;



    public void Initialize()
    {
        // TODO
    }
}
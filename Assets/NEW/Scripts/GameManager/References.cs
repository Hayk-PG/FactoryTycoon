using UnityEngine;

public class References : MonoBehaviour
{
    [Header("Game Manager")]
    [SerializeField] private TileCollection _tileCollection;

    [Header("Game Saving & Loading System")]
    [SerializeField] private GameObjectsPersistenceManager _gameObjectsPersistenceManager;

    [Header("Edit Mode Manager")]
    [SerializeField] private EditModeManager _editModeManager;
    [SerializeField] private ObjectHighlighter _objectHighlighter;

    // Instance
    public static References Manager { get; private set; }

    // Game Manager
    public TileCollection TileCollection => _tileCollection;

    // Game Saving & Loading System
    public GameObjectsPersistenceManager GameObjectsPersistenceManager => _gameObjectsPersistenceManager;

    // Edit Mode Manager
    public EditModeManager EditModeManager => _editModeManager;
    public ObjectHighlighter ObjectHighlighter => _objectHighlighter;




    private void Awake()
    {
        CreateInstance();
    }

    private void CreateInstance()
    {
        Manager = this;
    }
}

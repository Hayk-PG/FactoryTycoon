using UnityEngine;

public class References : MonoBehaviour
{
    [Header("Game Manager")]
    [SerializeField] private TileCollection _tileCollection;
    [SerializeField] private ConveyorCollection _conveyorCollection;

    [Header("Game Saving & Loading System")]
    [SerializeField] private GameObjectsPersistenceManager _gameObjectsPersistenceManager;

    [Header("Edit Mode Manager")]
    [SerializeField] private EditModeManager _editModeManager;
    [SerializeField] private ObjectPlacementValidator _objectPlacementValidator;

    [Header("Conveyor System Manager")]
    [SerializeField] private ConveyorSystemManager _conveyorSystemManager;

    [Header("Robot Task Manager")]
    [SerializeField] private RobotTaskManager _robotTaskManager;

    // Instance
    public static References Manager { get; private set; }

    // Game Manager
    public TileCollection TileCollection => _tileCollection;
    public ConveyorCollection ConveyorCollection => _conveyorCollection;

    // Game Saving & Loading System
    public GameObjectsPersistenceManager GameObjectsPersistenceManager => _gameObjectsPersistenceManager;

    // Edit Mode Manager
    public EditModeManager EditModeManager => _editModeManager;
    public ObjectPlacementValidator ObjectPlacementValidator => _objectPlacementValidator;

    // Conveyor System Manager
    public ConveyorSystemManager ConveyorSystemManager => _conveyorSystemManager;

    // Robot Task Manager
    public RobotTaskManager RobotTaskManager => _robotTaskManager;




    private void Awake()
    {
        CreateInstance();
    }

    private void CreateInstance()
    {
        Manager = this;
    }
}

using UnityEngine;

public class GameSceneObjectsReferences : MonoBehaviour
{
    [Header("Game Saving & Loading System")]
    [SerializeField] private GameObjectsPersistenceManager _gameObjectsPersistenceManager;
    [Header("Camera")]
    [SerializeField] private ObjectHighlighter _objectHighlighter;

    public static GameSceneObjectsReferences Manager { get; private set; }

    // Game Saving & Loading System
    public GameObjectsPersistenceManager GameObjectsPersistenceManager => _gameObjectsPersistenceManager;
    // Camera
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

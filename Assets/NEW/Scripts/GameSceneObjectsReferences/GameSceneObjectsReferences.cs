using UnityEngine;

public class GameSceneObjectsReferences : MonoBehaviour
{
    [SerializeField] private GameObjectsPersistenceManager _gameObjectsPersistenceManager;

    public static GameSceneObjectsReferences Manager { get; private set; }
    public GameObjectsPersistenceManager GameObjectsPersistenceManager => _gameObjectsPersistenceManager;




    private void Awake()
    {
        CreateInstance();
    }

    private void CreateInstance()
    {
        Manager = this;
    }
}

using System;
using UnityEngine;


public class GameObjectsPersistenceManager : MonoBehaviour
{
    [SerializeField] private PersistentObjectHandler _objectsSaver;
    [SerializeField] private PersistentObjectHandler _objectsLoader;

    public event Action<MonoBehaviour, string, Transform, Vector3, Vector3> OnLoadObj;
    public event Action<TilesContainer> OnCreateTilesInLoadedTilesContainer;
    public event Action<CanvasGroup> OnLoadObjOpenSpawnUI;
    
    
    
    
    public void SaveObject() 
    {
        _objectsSaver.Execute();
    }

    internal void LoadObject() 
    {
        _objectsLoader.Execute();
    }
}

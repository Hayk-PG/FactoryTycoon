using UnityEngine;

public abstract class PersistentObjectHandler : MonoBehaviour
{
    protected SavableGameObject[] _savableGameObjects;
    protected PersistentDataObjects[] _persistentDataObjects;
    protected const string _fileName = "GameObjects";
    protected string _savableObjectName;
    protected string _savableObjectParentName;
    protected int _savableObjectIndex;
    protected Vector3 _savableObjectLocalPosition;
    protected Vector3 _savableObjectLocalEulerAngles;
    
    
    
    public virtual void Execute()
    {
        FindSavableGameObjects();

        bool isSavableGameObjectsEmpty = _savableGameObjects == null || _savableGameObjects.Length == 0;

        if (isSavableGameObjectsEmpty)
        {
            return;
        }

        InitializePersistentDataObjects();

        RunIteration(_savableGameObjects.Length);
    }

    protected virtual void FindSavableGameObjects()
    {
        _savableGameObjects = FindObjectsOfType<SavableGameObject>();
    }

    protected virtual void InitializePersistentDataObjects()
    {
        _persistentDataObjects = new PersistentDataObjects[_savableGameObjects.Length];
    }

    protected virtual void StorePersistentDataObject(int index, PersistentDataObjects persistentDataObject)
    {
        _persistentDataObjects[index] = persistentDataObject;
    }

    protected virtual void RunIteration(int loopLength)
    {
        for (int i = 0; i < loopLength; i++)
        {
            OnIterationProcess(i);
        }

        OnIterationComplete();
    }

    protected abstract void OnIterationProcess(int index);

    protected abstract void OnIterationComplete();

    protected virtual void GetSavableObjectIndex(int index)
    {
        _savableObjectIndex = index;
    }

    protected virtual void GetSavableObjectName(string objectName)
    {
        _savableObjectName = objectName;
    }

    protected virtual void GetSavableObjectParentName(string parentName)
    {
        _savableObjectParentName = parentName;
    }

    protected virtual void GetSavableObjectLocalPosition(Vector3 localPosition)
    {
        _savableObjectLocalPosition = localPosition;
    }

    protected virtual void GetSavableObjectLocalEulerAngles(Vector3 localEulerAngles)
    {
        _savableObjectLocalEulerAngles = localEulerAngles;
    }
}

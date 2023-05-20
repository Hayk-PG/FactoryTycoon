using UnityEngine;
using Pautik;
using System;

public class GameObjectLoader : PersistentObjectHandler
{
    [SerializeField] MainUIScriptsHolder _mainScriptsHolder;

    private Transform _savableObjectParent;
    private Transform _savableObject;

    public event Action<MainBehaviour, string, Transform, Vector3, Vector3> OnLoadObj;
    public event Action<TilesContainer> OnCreateTilesInLoadedTilesContainer;
    public event Action<CanvasGroup> OnLoadObjOpenSpawnUI;
    
    
    
    
    public override void Execute()
    {
        InitializePersistentDataObjects();
        LoadFromJson();
    }

    protected override void InitializePersistentDataObjects()
    {
        _persistentDataObjects = null;
    }

    private void LoadFromJson()
    {
        JSON_BaseController.LoadFromJSON(_fileName, null, out _persistentDataObjects, OnJsonDataLoad);
    }

    private void OnJsonDataLoad()
    {
        RunIteration(_persistentDataObjects.Length);
    }

    protected override void OnIterationProcess(int index)
    {
        GetSavableObjectIndex(_persistentDataObjects[index].Index);
        GetSavableObjectName(_persistentDataObjects[index].ObjectName);
        GetSavableObjectParent(_persistentDataObjects[index].ParentName);
        GetSavableObjectLocalPosition(_persistentDataObjects[index].LocalPosition);
        GetSavableObjectLocalEulerAngles(_persistentDataObjects[index].LocalEulerAngles);
        InstantiateSavableObject();
        ProcessGameObject();
        RaiseOnCreateTilesEvent();
    }

    private void GetSavableObjectParent(string savableObjectParentName)
    {
        _savableObjectParent = string.IsNullOrEmpty(savableObjectParentName) ? null : GameObject.Find(savableObjectParentName).transform;
    }

    private void InstantiateSavableObject()
    {
        _savableObject = Instantiate(ObjectsHolder.instance.SavableGameObjects[_savableObjectIndex].transform);
    }

    private void ProcessGameObject()
    {
        MainBehaviour mainBehaviour = Get<MainBehaviour>.From(_savableObject.gameObject);

        bool hasMainBehaviour = mainBehaviour != null;

        if (hasMainBehaviour)
        {
            OnLoadObj.Invoke(mainBehaviour, _savableObjectName, _savableObjectParent, _savableObjectLocalPosition, _savableObjectLocalEulerAngles);
        }
        else
        {
            _savableObject.name = _savableObjectName;
            _savableObject.parent = _savableObjectParent;
            _savableObject.localPosition = _savableObjectLocalPosition;
            _savableObject.localEulerAngles = _savableObjectLocalEulerAngles;
        }
    }

    private void RaiseOnCreateTilesEvent()
    {
        OnCreateTilesInLoadedTilesContainer?.Invoke(Get<TilesContainer>.From(_savableObject.gameObject));
    }

    protected override void OnIterationComplete()
    {
        RaiseOnLoadObjOpenSpawnUIEvent();
    }

    private void RaiseOnLoadObjOpenSpawnUIEvent()
    {
        OnLoadObjOpenSpawnUI?.Invoke(_mainScriptsHolder.MainMenuButtons.MainMenuButtonsContainer.GetComponent<CanvasGroup>());
    }
}

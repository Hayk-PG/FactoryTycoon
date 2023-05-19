
public class GameObjectSaver : PersistentObjectHandler
{
    protected override void OnIterationProcess(int index)
    {
        GetSavableObjectIndex(ObjectsHolder.instance.SavableGameObjects.FindIndex(item => item.name == _savableGameObjects[index].name));
        GetSavableObjectName(_savableGameObjects[index].transform.name);
        GetSavableObjectParentName(_savableGameObjects[index].transform.parent != null ? _savableGameObjects[index].transform.parent.name : null);
        GetSavableObjectLocalPosition(_savableGameObjects[index].transform.localPosition);
        GetSavableObjectLocalEulerAngles(_savableGameObjects[index].transform.localEulerAngles);
        StorePersistentDataObject(index, new PersistentDataObjects(_savableObjectIndex, _savableObjectName, _savableObjectParentName, _savableObjectLocalPosition, _savableObjectLocalEulerAngles));
    }

    protected override void OnIterationComplete()
    {
        SaveToJson();
    }

    private void SaveToJson()
    {
        JSON_BaseController.SaveToJSON(_persistentDataObjects, _fileName, null);
    }
}

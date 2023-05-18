using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;

/// <summary>
/// Save/Load gameobjects
/// </summary>
public class SaveLoadGameobjects : MonoBehaviour
{
    //EVENTS
    public event Action<MonoBehaviour, string, Transform, Vector3, Vector3> OnLoadObj;
    /// <summary>
    /// Populate tiles in the loaded tilesContainer
    /// </summary>
    public event Action<TilesContainer> OnCreateTilesInLoadedTilesContainer;
    /// <summary>
    /// Closes main menu buttons container,and invoking OnGroundTilesSpawnUI OnSpawnUiActivity event ,that spawnable objects can get spawned properly
    /// </summary>
    public event Action<CanvasGroup> OnLoadObjOpenSpawnUI;

    const string fileName = "GameObjects";

    [SerializeField] MainUIScriptsHolder mainScriptsHolder;

    // 1) Find all savable gameobjects and cache them inside the SavableObjs array
    // 2) Create new SavedObjectsDatasClass array and set it's length
    // 3) Transfer gameobject's data into the SavedObjectsDatasClass
    // 4) Save to json

    /// <summary>
    /// Save all savable gameobjects datas to json.file
    /// </summary>
    internal void SaveObjs() {
      
        SavableGameObjects[] SavableObjs = FindObjectsOfType<SavableGameObjects>();
        SavedObjectsDatasClass[] SavedObjectsDatasClass = new SavedObjectsDatasClass[SavableObjs.Length];

        for (int s = 0; s < SavableObjs.Length; s++) {

            int index = ObjectsHolder.instance.SavableGameObjects.FindIndex(item => item.name == SavableObjs[s].name);
            string objName = SavableObjs[s].transform.name;
            string objParentName = SavableObjs[s].transform.parent != null ? SavableObjs[s].transform.parent.name : null;
            Vector3 localPosition = SavableObjs[s].transform.localPosition;
            Vector3 localEulerAngles = SavableObjs[s].transform.localEulerAngles;

            SavedObjectsDatasClass[s] = new SavedObjectsDatasClass(index, objName, objParentName, localPosition, localEulerAngles);
        }

        JSON_BaseController.SaveToJSON(SavedObjectsDatasClass, fileName, null);
    }

    // 1) Create new SavedObjectsDatasClass array and set it's value null
    // 2) Get all informations from json and put inside theSavedObjectsDatasClass array
    // 3) Instantiate gameobjects
    // 4) Invoke OnCreateTilesInLoadedTilesContainer event to create TilesContainer child tiles
    // 5) If gameobject contains MainBehaviour component, then invoke OnLoadObj event to set gameobject's condition
    // 6) If not, call Assign_NonMainBehaviour_GameObjects_Datas method
    // 7) Call OnLoadObjOpenSpawnUI event and susbcirbe to it in OnGroundTilesSpawnUI class: It allows gameobjects to update functions in MainBehaviour class without opening SpawnUI

    /// <summary>
    /// Load all saved gameobjects from json into the scene
    /// </summary>
    internal void LoadObjs() {
       
        SavedObjectsDatasClass[] SavedObjectsDatasClass = null;

        JSON_BaseController.LoadFromJSON(fileName, null, out SavedObjectsDatasClass, delegate {

            for (int i = 0; i < SavedObjectsDatasClass.Length; i++) {

                int index = SavedObjectsDatasClass[i].index;
                string name = SavedObjectsDatasClass[i].objName;
                Transform parent = string.IsNullOrEmpty(SavedObjectsDatasClass[i].parentName) ? null : GameObject.Find(SavedObjectsDatasClass[i].parentName).transform;
                Vector3 localPosition = SavedObjectsDatasClass[i].localPosition;
                Vector3 localEulerAngles = SavedObjectsDatasClass[i].localEulerAngles;

                Transform obj = Instantiate(ObjectsHolder.instance.SavableGameObjects[index].transform);

                OnCreateTilesInLoadedTilesContainer?.Invoke(obj.GetComponent<TilesContainer>() != null ? obj.GetComponent<TilesContainer>() : null);

                if (obj.GetComponent<MainBehaviour>() != null) {
                    OnLoadObj.Invoke(obj.GetComponent<MainBehaviour>(), name, parent, localPosition, localEulerAngles);
                }
                else {
                    Assign_NonMainBehaviour_GameObjects_Datas(obj, name, parent, localPosition, localEulerAngles);
                }   
            }
        });

        OnLoadObjOpenSpawnUI?.Invoke(mainScriptsHolder.MainMenuButtons.MainMenuButtonsContainer.GetComponent<CanvasGroup>());
    }

    void Assign_NonMainBehaviour_GameObjects_Datas(Transform obj, string name, Transform parent, Vector3 localPosition, Vector3 localEulerAngles) {

        obj.name = name;
        obj.parent = parent;
        obj.localPosition = localPosition;
        obj.localEulerAngles = localEulerAngles;
    }
  
    [Serializable]
    public class SavedObjectsDatasClass {

        public int index;
        public string objName;
        public string parentName;
        public Vector3 localPosition;
        public Vector3 localEulerAngles;


        public SavedObjectsDatasClass(int index, string objName, string parentName, Vector3 localPosition, Vector3 localEulerAngles) {

            this.index = index;
            this.objName = objName;
            this.parentName = parentName;
            this.localPosition = localPosition;
            this.localEulerAngles = localEulerAngles;
        }
    }








}

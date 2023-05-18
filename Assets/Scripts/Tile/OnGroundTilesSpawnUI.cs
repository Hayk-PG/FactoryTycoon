using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundTilesSpawnUI : MonoBehaviour
{
    public delegate void OnGroundTileSpanwUIActivity(bool isOpened);
    public event OnGroundTileSpanwUIActivity OnSpawnUiActivity;

    [Header("CANVAS GROUP")]
    [SerializeField] CanvasGroup spawnUICanvasGroup;
    public bool isOpened;
    public bool useUpdateMethod = true;

    SaveLoadGameobjects saveLoadGameObjects;//Controller for saving or loading savable gameobjects

    void Awake() {

        isOpened = spawnUICanvasGroup.interactable;
        saveLoadGameObjects = ObjectsHolder.instance.SaveLoadGameobjects;
    }

    void OnEnable() {
        saveLoadGameObjects.OnLoadObjOpenSpawnUI += SaveLoadGameObjects_OnLoadObjOpenSpawnUI;    
    }
   
    void OnDisable() {
        saveLoadGameObjects.OnLoadObjOpenSpawnUI -= SaveLoadGameObjects_OnLoadObjOpenSpawnUI;
    }

    void Update() {

        if (useUpdateMethod) {

            if (spawnUICanvasGroup.alpha <= 0) {

                OnSpawnUiActivity?.Invoke(false);
                //isOpened = false;
            }
            if (spawnUICanvasGroup.alpha >= 1) {

                OnSpawnUiActivity?.Invoke(true);
                //isOpened = true;
            }
        }
        else {
            return;
        }
    }

    /// <summary>
    /// Closes main menu buttons container,and invoking OnGroundTilesSpawnUI OnSpawnUiActivity event ,that spawnable objects can get spawned properly:
    /// Obj parameter represents the main menu buttons container's CanvasGroup
    /// </summary>
    /// <param name="obj"></param>
    void SaveLoadGameObjects_OnLoadObjOpenSpawnUI(CanvasGroup obj) {

        obj.alpha = 0;
        obj.interactable = false;
        obj.blocksRaycasts = false;

        StartCoroutine(Invok_OnSpawnUiActivity_Event_Without_Opening_SpawnUI());
    }

    IEnumerator Invok_OnSpawnUiActivity_Event_Without_Opening_SpawnUI() {

        useUpdateMethod = false;
        OnSpawnUiActivity?.Invoke(true);

        yield return new WaitForSeconds(1);
        useUpdateMethod = true;
    }










}

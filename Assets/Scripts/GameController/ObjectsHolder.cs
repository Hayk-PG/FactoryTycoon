using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectsHolder : MonoBehaviour {
    public static ObjectsHolder instance;

    [Header("MAIN COMPONENTS")]
    [SerializeField] Camera mainCamera; public Camera MainCamera => mainCamera;

    [Header("GAME CONTROLLER COMPONENTS")]
    [SerializeField] Inputs inputs; 
    [SerializeField] LayerMasks layerMasks;

    /// <summary>
    /// Inputs controller
    /// </summary>
    public Inputs Inputs => inputs;

    /// <summary>
    /// LayerMasks controller
    /// </summary>
    public LayerMasks LayerMasks => layerMasks;

    [Header("Spawn")]
    [SerializeField] SpanwOnTileController spawnOnTileController;

    /// <summary>
    /// Spawning spawnable gameobjects
    /// </summary>
    public SpanwOnTileController SpawnOnTileController => spawnOnTileController;

    [Header("SPAWNABLE OBJECTS")]
    [SerializeField] MainBehaviour[] spawnableObjects;

    /// <summary>
    /// Spawnable gameobjects prefabs array
    /// </summary>
    public MainBehaviour[] SpawnableObjects => spawnableObjects;

    [Header("SPAWNABE OBJECTS UI")]

    [SerializeField] Button spawnObjectButton;    
    [SerializeField] OnGroundTilesSpawnUI onGroundTilesSpawnUI;
    [SerializeField] ArrowButtonsForSpawnableObjRotation spawnableObjectsRotation;

    /// <summary>
    /// Spawnable objects spawn button prefab
    /// </summary>
    public Button SpawnObjectButton => spawnObjectButton;

    /// <summary>
    /// GroundTiles----> OnGroundTilesSpawnUI script
    /// </summary>
    public OnGroundTilesSpawnUI OnGroundTilesSpawnUI => onGroundTilesSpawnUI;

    /// <summary>
    /// Controls spawnable objects rotation
    /// </summary>
    public ArrowButtonsForSpawnableObjRotation SpawnableObjectsRotation => spawnableObjectsRotation;

    [Header("WORLD UI")]
    [SerializeField] Canvas arrowWorldButtonPrefab; public Canvas ArrowWorldButtonPrefab => arrowWorldButtonPrefab;

    [Header("SAVE")]
    [SerializeField] SaveRoboGlobalTemplates saveRobotTemplates;

    /// <summary>
    /// Saves and loads robot templates
    /// </summary>
    public SaveRoboGlobalTemplates SaveRobotTemplates => saveRobotTemplates;

    [Header("SAVABLE GAME OBJECTS")]
    [SerializeField] List<GameObject> savableGameObjects;

    /// <summary>
    /// List of game objects that can be saved and loaded
    /// </summary>
    public List<GameObject> SavableGameObjects {

        get {

           if(!savableGameObjects.Contains(spawnableObjects[spawnableObjects.Length - 1].gameObject)) {
                foreach (var item in spawnableObjects) {
                    savableGameObjects.Add(item.gameObject);
                }
            }

            return savableGameObjects;
        }
    }

    [Header("UI")]
    [SerializeField] SaveLoadGameobjects saveLoadGameobjects;

    /// <summary>
    /// Main class for loadig saved gameobjects from json
    /// </summary>
    public SaveLoadGameobjects SaveLoadGameobjects {
        get {
            if(saveLoadGameobjects == null) {
                if(FindObjectOfType<SaveLoadGameobjects>() != null) {
                    saveLoadGameobjects = FindObjectOfType<SaveLoadGameobjects>();
                }
            }
            return saveLoadGameobjects;
        }
    }

    [Header("ITEMS")]
    [SerializeField] Animator[] itemsAnimator;

    /// <summary>
    /// Items animator for runtime usage
    /// </summary>
    public Animator[] ItemsAnimator => itemsAnimator;

    void Awake() {

        instance = this;
    }

    void Start() {

        AddSavableComponentToAllSpawnableOrSavableGameObjects();
    }

    void AddSavableComponentToAllSpawnableOrSavableGameObjects() {

        foreach (var obj in SavableGameObjects) {
            if (obj.GetComponent<SavableGameObjects>() == null) {
                obj.gameObject.AddComponent<SavableGameObjects>();
            }
        }
    }











}

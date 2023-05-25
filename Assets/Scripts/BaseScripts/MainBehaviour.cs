using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Main behaviour of all spawnable objects
/// </summary>
public abstract class MainBehaviour : PhysicsBoxCastAll, IRaySelect, ISpawnableObjects, IAutoSaveData {

    protected delegate void OnEnableDisableClasses(bool isActive);
    /// <summary>
    /// Enable or disable desired script after getting spawned
    /// </summary>
    protected OnEnableDisableClasses ClassesDependstGotSpawnedBoolean { get; set; }

    //Events Holders
    OnGroundTilesSpawnUI OnClickSpawnButton;
    SpanwOnTileController OnSpawnTileController;
    GameObjectsPersistenceManager OnLoadSavedGameObject;

    [Header("MESH RENDERER")]
    [SerializeField] protected MeshRenderer[] meshRend;
    [SerializeField] protected Material highlightedMaterial;
    [SerializeField] protected Material errorMaterial;

    protected Dictionary<Material, MeshRenderer> meshMaterials = new Dictionary<Material, MeshRenderer>();

    [Header("COLLIDER")]
    public Collider col;

    /// <summary>
    /// Main collider of this gameobject
    /// </summary>
    public Collider Col => col;
    public abstract float ColX { get; set; }
    public abstract float ColY { get; set; }
    public abstract float ColZ { get; set; }

    /// <summary>
    /// Has this object spawned
    /// </summary>
    public bool GotSpawned { get; set; }

    /// <summary>
    /// Can other gameObject spawn on this gameObject
    /// </summary>
    public bool CanSpawnTheObject { get; set; }

    /// <summary>
    /// This is the Vector3 point, where choosen gameobject will spawn 
    /// </summary>
    public Vector3 OnThisGameObjectSpawnPosition { get => new Vector3(transform.position.x, Col.bounds.max.y, transform.position.z); set { } }

    /// <summary>
    /// Can this object get spawned
    /// </summary>
    public virtual bool CanThisGameObjectGetSpawned { get => true; set { } }
   
    /// <summary>
    /// Is Spawn UI open:
    /// Hierarchy/GroundTiles/SpawnUI
    /// </summary>
    protected bool isSpawnUIOpen;

    /// <summary>
    /// What type of choosen gameobject is, before getting spawned
    /// </summary>
    public enum SPAWN_TYPE { ON_TILE , ON_CONVEYOR}
    public SPAWN_TYPE spawn_type;

    #region IAutoSaveData
    public virtual float SliderValue { get; set; }
    public virtual Vector3 LocalPosition { get; set; }
    public virtual Vector3 LocalEulerAngles { get; set; }
    #endregion



    protected virtual void Awake() {
     
        OnClickSpawnButton = ObjectsHolder.instance.OnGroundTilesSpawnUI;
        OnSpawnTileController = ObjectsHolder.instance.SpawnOnTileController;
        OnLoadSavedGameObject = ObjectsHolder.instance.SaveLoadGameobjects;

        InitializeDefaultMaterials();
    }

    protected virtual void OnEnable() {

        OnClickSpawnButton.OnSpawnUiActivity += OnClickSpawnButton_OnSpawnUiActivity;
        OnSpawnTileController.OnGameObjectGotSpawned += OnSpawnTileController_OnGameObjectGotSpawned;
        OnSpawnTileController.OnRotateChoosenObj += OnSpawnTileController_OnRotateChoosenObj;
        OnLoadSavedGameObject.OnLoadObj += OnLoadSavedGameObject_OnLoadObj;
    }
   
    protected virtual void OnDisable() {

        OnClickSpawnButton.OnSpawnUiActivity -= OnClickSpawnButton_OnSpawnUiActivity;
        OnSpawnTileController.OnGameObjectGotSpawned -= OnSpawnTileController_OnGameObjectGotSpawned;
        OnSpawnTileController.OnRotateChoosenObj -= OnSpawnTileController_OnRotateChoosenObj;
        OnLoadSavedGameObject.OnLoadObj -= OnLoadSavedGameObject_OnLoadObj;
    }
   
    protected virtual void Update() {
      
        if (isSpawnUIOpen) {
            MainBoxCast(Col);
            OnActivateThisGameObject(GotSpawned);
        }
        else {
            return;
        }
    }
  
    public void SetTransform(float X, float Y, float Z, Transform parent) {

        transform.position = new Vector3(X + ColX, Y + ColY, Z + ColZ);
        transform.parent = parent;
    }

    public void SetEulerAngles(Vector3 eulerAngles) {

        transform.eulerAngles = eulerAngles;
    }

    public virtual void OnHover() {

        foreach (var item in meshMaterials) {

            item.Value.material = highlightedMaterial;
        }
    }

    public virtual void OnHoverOccupied() {

        foreach (var item in meshMaterials) {

            item.Value.material = errorMaterial;
        }
    }

    public virtual void OnUnhover() {

        foreach (var item in meshMaterials) {

            item.Value.material = item.Key;
        }
    }
    
    void InitializeDefaultMaterials() {

     /* Get all meshes
      * Cache meshes materials in dictionary */

        foreach (var item in meshRend) {

            for (int i = 0; i < item.materials.Length; i++) {

                meshMaterials.Add(item.materials[i], item);
            }
        }
    }
 
    public Button CreateRelatedButtonInSpawnUI(Transform parent) {

        Button relatedButton = Instantiate(ObjectsHolder.instance.SpawnObjectButton, parent);
        relatedButton.name = gameObject.name;
        relatedButton.GetComponentInChildren<Text>().text = relatedButton.name;

        return relatedButton;
    }

    private void OnClickSpawnButton_OnSpawnUiActivity(bool isOpened) {

        isSpawnUIOpen = isOpened;

        OnUnhover();
    }

    void OnSpawnTileController_OnRotateChoosenObj(MainBehaviour choosenGameObject, float angle) {
       
        if (choosenGameObject == this) {

            float y = transform.eulerAngles.y;

            SetEulerAngles(new Vector3(0, y += angle, 0));
        }
    }

    void OnSpawnTileController_OnGameObjectGotSpawned(MainBehaviour choosenGameObject, string name, Transform parent, Vector3 localPosition, Vector3 localEulerAngles) {

        if (choosenGameObject == this) {
            gameObject.name = name;
            SetTransform(localPosition.x, localPosition.y, localPosition.z, parent);
            SetEulerAngles(localEulerAngles);
            GotSpawned = true;
        }
    }

    void OnLoadSavedGameObject_OnLoadObj(MonoBehaviour loadGameObject, string name, Transform parent, Vector3 localPosition, Vector3 localEulerAngles) {
        
        if(loadGameObject == this) {
            gameObject.name = name;
            transform.parent = parent;
            transform.localPosition = localPosition;
            transform.localEulerAngles = localEulerAngles;
            GotSpawned = true;
        }
    }






}

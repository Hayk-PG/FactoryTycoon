using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpanwOnTileController : MonoBehaviour
{
    public delegate void GameObjectGotSpawned(MainBehaviour choosenGameObject, string name,Transform parent, Vector3 localPosition, Vector3 localEulerAngles);
    public delegate void OnSpawnableObjectRotationBeforeSpawning(MainBehaviour choosenObj, float angle);

    /// <summary>
    /// Spawn the gameobject by SpawnOnTileController.OnClickCreateButton
    /// </summary>
    public event GameObjectGotSpawned OnGameObjectGotSpawned;

    /// <summary>
    /// Controls spawnable object's rotation
    /// </summary>
    public event OnSpawnableObjectRotationBeforeSpawning OnRotateChoosenObj;

    [SerializeField] InitializeAllSpawnableObjectsButton Spawn;
    [SerializeField] OnGroundTilesSpawnUI OnGroundTilesSpawnUI;
    [SerializeField] ArrowButtonsForSpawnableObjRotation OnSpawnableObjectsRotation;

    Ray mouseRay;
    RaycastHit mouseHit;

    /// <summary>
    /// Choosen obj position
    /// </summary>
    Vector3 SpawnPosition { get; set; }

    /// <summary>
    /// Choosen obj local eulerAngles
    /// </summary>
    Vector3 SpawnLocalEulerAngles { get; set; }


    /// <summary>
    /// By raycast selected tile
    /// </summary>
    ISpawnableObjects SelectedTile { get; set; }

    /// <summary>
    /// Choosen gameobject ready to get spawned
    /// </summary>
    MainBehaviour ChoosenObj { get; set; }



    void OnEnable() {

        Spawn.OnClickToChooseObject += Spawn_OnClickToChooseObject;
        Spawn.OnClickCreateButton += Spawn_OnClickCreateButton;
        OnGroundTilesSpawnUI.OnSpawnUiActivity += OnGroundTilesSpawnUI_OnSpawnUiActivity;
        OnSpawnableObjectsRotation.OnClickArrowButtonsToRotateSpawnableOjects += OnSpawnableObjectsRotation_OnClickArrowButtonsToRotateSpawnableOjects;       
    }
    
    void OnDisable() {

        Spawn.OnClickToChooseObject -= Spawn_OnClickToChooseObject;
        Spawn.OnClickCreateButton -= Spawn_OnClickCreateButton;
        OnGroundTilesSpawnUI.OnSpawnUiActivity -= OnGroundTilesSpawnUI_OnSpawnUiActivity;
        OnSpawnableObjectsRotation.OnClickArrowButtonsToRotateSpawnableOjects -= OnSpawnableObjectsRotation_OnClickArrowButtonsToRotateSpawnableOjects;
    }

    /// <summary>
    /// Returns true,if choosenObj exists: false if obj is null
    /// </summary>
    bool IsChoosenObjActive() {

        if (ChoosenObj != null)
            return true;
        else
            return false;
    }

    bool CastRayTile(MainBehaviour choosenObj) {

        mouseRay = ObjectsHolder.instance.MainCamera.ScreenPointToRay(Input.mousePosition);

        return choosenObj == null ? Physics.Raycast(mouseRay.origin, mouseRay.direction, out mouseHit, Mathf.Infinity, ObjectsHolder.instance.LayerMasks.TileMask) :
            choosenObj.spawn_type == MainBehaviour.SPAWN_TYPE.ON_TILE ? Physics.Raycast(mouseRay.origin, mouseRay.direction, out mouseHit, Mathf.Infinity, ObjectsHolder.instance.LayerMasks.TileMask) :
            choosenObj.spawn_type == MainBehaviour.SPAWN_TYPE.ON_CONVEYOR ? Physics.Raycast(mouseRay.origin, mouseRay.direction, out mouseHit, Mathf.Infinity, ObjectsHolder.instance.LayerMasks.ConveyorMask) : false;
    } 
  
    void Spawn_OnClickToChooseObject(ISpawnableObjects Obj) {

        if (IsChoosenObjActive()) {
            Destroy(ChoosenObj.gameObject);
        }

        MainBehaviour spawnableObj = (MainBehaviour)Obj;
        ChoosenObj = Instantiate(spawnableObj);
        ChoosenObj.name = spawnableObj.name;
    }

    void ChoosenObjTransform(MainBehaviour choosenObj) {

        choosenObj.SetTransform(SpawnPosition.x, SpawnPosition.y, SpawnPosition.z, null);
    }

    void OnSpawnableObjectsRotation_OnClickArrowButtonsToRotateSpawnableOjects(float angle) {
            
        if(IsChoosenObjActive()) {

            OnRotateChoosenObj?.Invoke(ChoosenObj, angle);
        }
    }

    //HACK Using OnGroundTilesSpawnUI OnSpawnUiActivity(bool isOpened) event instead of Update function
    void OnGroundTilesSpawnUI_OnSpawnUiActivity(bool isOpen) {     

        if (isOpen) {

            if (IsChoosenObjActive()) {
                SpawnLocalEulerAngles = ChoosenObj.transform.localEulerAngles;
            }

            if (CastRayTile(ChoosenObj)) {

                SelectedTile = mouseHit.collider.GetComponent<ISpawnableObjects>() != null ? mouseHit.collider.GetComponent<ISpawnableObjects>() : null;
                SpawnPosition = SelectedTile.OnThisGameObjectSpawnPosition;

                if (IsChoosenObjActive()) {
                    ChoosenObjTransform(ChoosenObj);                   
                }
            }
        }

        if (!isOpen && IsChoosenObjActive()) {

            Destroy(ChoosenObj.gameObject);
        }
    }

    void Spawn_OnClickCreateButton(bool isButtonPressed) {

        if (isButtonPressed && IsChoosenObjActive() && SelectedTile.CanSpawnTheObject && ChoosenObj.CanThisGameObjectGetSpawned) {

            MainBehaviour obj = Instantiate(ChoosenObj);
            OnGameObjectGotSpawned?.Invoke(obj, ChoosenObj.gameObject.name, null, SpawnPosition, SpawnLocalEulerAngles);
        }
    }



}

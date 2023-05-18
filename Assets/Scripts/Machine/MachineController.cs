using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineController : MonoBehaviour, IMachineController
{  
    OnGroundTilesSpawnUI OnGroundTilesSpawnUI { get; set; }

    [Header("TRANSFORMS")]
    [SerializeField] Transform[] points;

    [Header("COMPONENTS")]
    [SerializeField] LinkEachOther linkEachOther;
    [SerializeField] MachineTool machineTool;

    /// <summary>
    /// Checking if OnGroundTilesSpawnUI OnSpawnUiActivity event is called and it's parameter is true
    /// </summary>
    bool OnSpawnUiActivityInvoked { get; set; }

    /// <summary>
    /// Are items iside the machine
    /// </summary>
    bool ItemLoaded { get; set; }
  
    /// <summary>
    /// Gets related conveyor
    /// </summary>
    Conveyor relatedConveyor;

    /// <summary>
    /// Items that are inside the machine
    /// </summary>
    public List<GameObject> LoadedItems = new List<GameObject>();


    void Awake() {

        OnGroundTilesSpawnUI = ObjectsHolder.instance.OnGroundTilesSpawnUI;
    }

    void OnEnable() {

        OnGroundTilesSpawnUI.OnSpawnUiActivity += OnGroundTilesSpawnUI_OnSpawnUiActivity;
    }
   
    void OnDisable() {

        OnGroundTilesSpawnUI.OnSpawnUiActivity -= OnGroundTilesSpawnUI_OnSpawnUiActivity;
    }

     void Update() {

        if (!OnSpawnUiActivityInvoked) {

            if (AreItemsReadyToGetLoaded() && !ItemLoaded) {

                StartCoroutine(RunItems());
            }
        }
        else {
            return;
        }
    }

    /// <summary>
    /// Are item readys to get loaded into the machine
    /// </summary>
    /// <returns></returns>
    bool AreItemsReadyToGetLoaded() {

        if (DetectedItemsCollider().Length > 0)
            return true;
        else
            return false;
    }

    /// <summary>
    /// Detect all items passing in front of the machine
    /// </summary>
    /// <returns></returns>
    Collider[] DetectedItemsCollider() {

        return Physics.OverlapBox(points[0].position, new Vector3(0.25f, 0.25f, 0.25f), points[0].localRotation, ObjectsHolder.instance.LayerMasks.ItemMask);
    }

    /// <summary>
    /// Gets related conveyor: Controls all related conveyors via this conveyor
    /// </summary>
    /// <returns></returns>
    Conveyor GetRelatedConveyor() {

        bool conv = Physics.BoxCast(points[0].position, Vector3.zero, points[0].forward, out RaycastHit hit, points[0].localRotation, 0.5f, ObjectsHolder.instance.LayerMasks.ConveyorMask);

        return hit.collider != null ? hit.collider.GetComponent<Conveyor>() : null;
    }

    /// <summary>
    /// Run items coroutine
    /// </summary>
    /// <returns></returns>
    IEnumerator RunItems() {

        ItemLoaded = true;

        for (int i = 0; i < DetectedItemsCollider().Length; i++) {

            AddItems_To_LoadedItemsList(i);
        }

        yield return new WaitForSeconds(0.1f);

        if(LoadedItems.Count > 0) {

            LinkedMainBehaviour_IndirectControll(false);
            Conveyors_Activity(false);                
        }

        yield return new WaitForSeconds(1);

        LinkedMainBehaviour_IndirectControll(true);
        Conveyors_Activity(true);
        Finish();

        ItemLoaded = false;
    }  

    void AddItems_To_LoadedItemsList(int i) {

        if (DetectedItemsCollider()[i].GetComponent<BaseItemDeformation>() != null)
        {
            if ((MachineController)DetectedItemsCollider()[i].GetComponent<BaseItemDeformation>().Machine != this)
            {
                if (!LoadedItems.Contains(DetectedItemsCollider()[i].gameObject))
                {
                    LoadedItems.Add(DetectedItemsCollider()[i].gameObject);
                    DetectedItemsCollider()[i].GetComponent<BaseItemDeformation>().Skin.enabled = false;
                    DetectedItemsCollider()[i].GetComponent<BaseItemDeformation>().Machine = this;
                    for (int s = 0; s < machineTool.MachineSliders.Length; s++)
                    {
                        DetectedItemsCollider()[i].GetComponent<BaseItemDeformation>().SetBlendShapeValue(s, machineTool.MachineSliders[s].SliderValue);
                    }                  
                }
            }
        }
    }

    void Finish()
    {
        for (int i = 0; i < LoadedItems.Count; i++)
        {
            LoadedItems[i].GetComponent<BaseItemDeformation>().CleanUp();
            LoadedItems[i].GetComponent<BaseItemDeformation>().Skin.enabled = true;
            LoadedItems.RemoveAt(i);
        }
    }
  
    /// <summary>
    /// Call IIndirectControll interface functions ,by getting component IIndirectControll from MainBehaviour obj
    /// </summary>
    /// <param name="run"></param>
    void LinkedMainBehaviour_IndirectControll(bool run) {

        if (!run) {
            if (linkEachOther.ConnectedMainBehaviour != null) {
                linkEachOther.ConnectedMainBehaviour.GetComponent<IIndirectControll>()?.Stop();
                linkEachOther.ConnectedMainBehaviour.GetComponentInChildren<IIndirectControll>()?.Stop();
            }
        }
        else {
            if (linkEachOther.ConnectedMainBehaviour != null) {
                linkEachOther.ConnectedMainBehaviour.GetComponent<IIndirectControll>()?.Run();
                linkEachOther.ConnectedMainBehaviour.GetComponentInChildren<IIndirectControll>()?.Run();
            }
        }
    }

    /// <summary>
    /// Call IIndirectControll interface functions,by getting conveyor's IIndirectControll component
    /// </summary>
    /// <param name="run"></param>
    void Conveyors_Activity(bool run) {

        if (!run) {
            if (GetRelatedConveyor() != null) {
                foreach (var conveyor in GetRelatedConveyor().GetComponent<LinkConveyors>().linkedConveyors) {
                    conveyor.GetComponent<IIndirectControll>()?.Stop();
                }
            }
        }
        else {
            if (GetRelatedConveyor() != null) {
                foreach (var conveyor in GetRelatedConveyor().GetComponent<LinkConveyors>().linkedConveyors) {
                    conveyor.GetComponent<IIndirectControll>()?.Run();
                }
            }
        }
    }

    void OnGroundTilesSpawnUI_OnSpawnUiActivity(bool isOpened) {

        OnSpawnUiActivityInvoked = isOpened;
    }

















}

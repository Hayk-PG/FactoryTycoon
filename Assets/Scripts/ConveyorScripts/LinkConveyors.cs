using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkConveyors : MonoBehaviour
{
    OnGroundTilesSpawnUI OnGroundTilesSpawnUI { get; set; }

    /// <summary>
    /// Checking if OnGroundTilesSpawnUI OnSpawnUiActivity event is called and it's parameter is true
    /// </summary>
    bool OnSpawnUiActivityInvoked { get; set; }

    [Header("SENSORS")]
    [SerializeField] ConveyorCheckSides conveyorCheckSides;
    public ConveyorCheckSides ConveyorCheckSides => conveyorCheckSides;

    [Header("WHOLE CONVEYOR SYSTEM")]
    public List<Conveyor> linkedConveyors = new List<Conveyor>();

    [Header("ATTACHED CONVEYORS")]
    [SerializeField] List<Conveyor> attachedConveyors = new List<Conveyor>();

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

        if (OnSpawnUiActivityInvoked) {
            
            NullCheck();

            Add_HitCollidersToAttachedConveyorList();

            Add_AttachedConveyorList_To_HitColliders_LinkedConveyorsList();

            Add_LinkedConveyorsList_To_AttachedConveyorsLists_LinkedConveyorsList();

            Add_This_To_AttachedConveyorsLists_LinkedConveyorList();
        }
        else {
            return;
        }
    }

    /// <summary>
    /// Checking if conveyor not destroyed
    /// </summary>
    void NullCheck() {

        for (int i = 0; i < attachedConveyors.Count; i++) {
            if (attachedConveyors[i] == null) {
                attachedConveyors.Remove(attachedConveyors[i]);
            }
        }
        for (int i = 0; i < linkedConveyors.Count; i++) {
            if (linkedConveyors[i] == null) {
                linkedConveyors.Remove(linkedConveyors[i]);
            }
        }
    }

    /// <summary>
    /// Adding hit colliders (defined in MainSensorsBehaviour class) to attachedConveyors list
    /// </summary>
    void Add_HitCollidersToAttachedConveyorList() {

        if (ConveyorCheckSides.leftHit.collider != null && !attachedConveyors.Contains(ConveyorCheckSides.leftHit.collider.GetComponent<Conveyor>()) && ConveyorCheckSides.leftHit.collider.GetComponent<Conveyor>().GotSpawned) {
            attachedConveyors.Add(ConveyorCheckSides.leftHit.collider.GetComponent<Conveyor>());
        }
        if (ConveyorCheckSides.rightHit.collider != null && !attachedConveyors.Contains(ConveyorCheckSides.rightHit.collider.GetComponent<Conveyor>()) && ConveyorCheckSides.rightHit.collider.GetComponent<Conveyor>().GotSpawned) {
            attachedConveyors.Add(ConveyorCheckSides.rightHit.collider.GetComponent<Conveyor>());
        }
        if (ConveyorCheckSides.frontHit.collider != null && !attachedConveyors.Contains(ConveyorCheckSides.frontHit.collider.GetComponent<Conveyor>()) && ConveyorCheckSides.frontHit.collider.GetComponent<Conveyor>().GotSpawned) {
            attachedConveyors.Add(ConveyorCheckSides.frontHit.collider.GetComponent<Conveyor>());
        }
        if (ConveyorCheckSides.backHit.collider != null && !attachedConveyors.Contains(ConveyorCheckSides.backHit.collider.GetComponent<Conveyor>()) && ConveyorCheckSides.backHit.collider.GetComponent<Conveyor>().GotSpawned) {
            attachedConveyors.Add(ConveyorCheckSides.backHit.collider.GetComponent<Conveyor>());
        }
    }

    /// <summary>
    /// Adding conveyors from attachedConveyors list to attached conveyors linkedConveyors list
    /// </summary>
    void Add_AttachedConveyorList_To_HitColliders_LinkedConveyorsList() {

        if (attachedConveyors != null) {
            for (int a = 0; a < attachedConveyors.Count; a++) {
                for (int i = 0; i < attachedConveyors.Count; i++) {
                    if (!attachedConveyors[a].GetComponent<LinkConveyors>().linkedConveyors.Contains(attachedConveyors[i])) {
                        attachedConveyors[a].GetComponent<LinkConveyors>().linkedConveyors.Add(attachedConveyors[i]);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Adding conveyors from linkedConveyors list to attached conveyors linkedConveyors list
    /// </summary>
    void Add_LinkedConveyorsList_To_AttachedConveyorsLists_LinkedConveyorsList() {

        if (linkedConveyors != null) {
            for (int a = 0; a < attachedConveyors.Count; a++) {
                for (int l = 0; l < linkedConveyors.Count; l++) {
                    if (!attachedConveyors[a].GetComponent<LinkConveyors>().linkedConveyors.Contains(linkedConveyors[l])) {
                        attachedConveyors[a].GetComponent<LinkConveyors>().linkedConveyors.Add(linkedConveyors[l]);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Adding this gameobject to attached conveyors linkedConveyors list
    /// </summary>
    void Add_This_To_AttachedConveyorsLists_LinkedConveyorList() {

        for (int a = 0; a < attachedConveyors.Count; a++) {
            if (!attachedConveyors[a].GetComponent<LinkConveyors>().linkedConveyors.Contains(GetComponent<Conveyor>())) {
                attachedConveyors[a].GetComponent<LinkConveyors>().linkedConveyors.Add(GetComponent<Conveyor>());
            }
        }
    }
    
    void OnGroundTilesSpawnUI_OnSpawnUiActivity(bool isOpened) {

        OnSpawnUiActivityInvoked = isOpened;
    }








}

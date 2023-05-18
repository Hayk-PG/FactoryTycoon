using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ConveyorCheckSides))]
public class Conveyor : ConveyorMovement, IIndirectControll
{
    [Header("CONVEYOR")]
    [SerializeField] List<ItemMovement> items = new List<ItemMovement>();

    public Machine1 GameObjectOnTopOfTheConveyor;

    /// <summary>
    /// Indirect stop from IIndirect Stop()
    /// </summary>
    bool IsStoped { get; set; }



    protected override void Awake() {

        base.Awake();

        //HACK      CanSpawnTheObject  <== This is a temporary value
        CanSpawnTheObject = true;

        ClassesDependstGotSpawnedBoolean += delegate (bool isActive) {

            if (GetComponent<MainSensorsBehaviour>() != null) {
                if (GetComponent<MainSensorsBehaviour>().enabled != isActive)
                    GetComponent<MainSensorsBehaviour>().enabled = isActive;
            }
        };
    }

    protected override void Update() {

        base.Update();    
        ClassesDependstGotSpawnedBoolean(GotSpawned);
        ControllingItems();
    }

    void OnTriggerEnter(Collider other) {

        if (other.GetComponent<ItemMovement>() != null) {

            items.Add(other.GetComponent<ItemMovement>());           
        }
    }

    void OnTriggerExit(Collider other) {
        
        if (other.GetComponent<ItemMovement>() != null) {

            if (other.GetComponent<ItemMovement>().ActiveConveyor == this) {

                other.GetComponent<ItemMovement>().ActiveConveyor = null;

                OnUnhover();
            }

            items.Remove(other.GetComponent<ItemMovement>());           
        }
    }

    /// <summary>
    /// Conveyor's main fucntion
    /// </summary>
    void ControllingItems() {

        for (int i = 0; i < items.Count; i++) {

            if (!IsStoped) {

                if(items[i] != null) {

                    if (items[i].ActiveConveyor == null && items[i].transform.parent == null) {
                        items[i].ActiveConveyor = this;
                    }
                    else if(items[i].ActiveConveyor == this) {

                        if (items[i].ShouldStopped) {
                            items[i].ShouldStopped = false;
                        }
                    }
                }
                else {
                    items.Remove(items[i]);
                }
            }
            else {

                if (!items[i].ShouldStopped) {
                    items[i].ShouldStopped = true;
                }
                    
            }
        }
    }

    /// <summary>
    /// Stops the conveyor (IIndirect)
    /// </summary>
    public void Stop() {

        IsStoped = true;
    }

    /// <summary>
    /// Starts the conveyor (IIndirect)
    /// </summary>
    public void Run() {

        IsStoped = false;
    }









}

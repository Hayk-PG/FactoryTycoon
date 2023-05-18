using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMovement : MonoBehaviour,IMovement
{
    OnGroundTilesSpawnUI tilesSpawnUI;

    //IMOVEMENT INTERFACE
    public Vector3 Direction { get => ActiveConveyor != null ? ActiveConveyor.Direction : Vector3.zero; set { } }
    public float Speed { get => ActiveConveyor != null ? ActiveConveyor.Speed : 0; set { } }
    public Rigidbody Rb { get => GetComponent<Rigidbody>() != null ? GetComponent<Rigidbody>() : null; set { } }
    public Transform _Transform { get => transform; set { } }

    bool IsSpawnUiOpen { get; set; }
    
    public Conveyor ActiveConveyor { get; set; }
    public bool ShouldStopped { get; set; }




    void Awake() {

        tilesSpawnUI = ObjectsHolder.instance.OnGroundTilesSpawnUI;
    }

    void OnEnable() {

        tilesSpawnUI.OnSpawnUiActivity += TilesSpawnUI_OnSpawnUiActivity;
    }

    void OnDisable() {

        tilesSpawnUI.OnSpawnUiActivity -= TilesSpawnUI_OnSpawnUiActivity;
    }
    
    void Update() {

        if (!IsSpawnUiOpen && !ShouldStopped)
            transform.position += Direction * Speed * Time.deltaTime;
        else
            return;
    }

    void TilesSpawnUI_OnSpawnUiActivity(bool isOpened) {

        IsSpawnUiOpen = isOpened;
    }








}

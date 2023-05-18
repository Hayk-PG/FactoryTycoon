using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MainBehaviour
{
    AutoSaveGameObjectsData AutoSave;

    public override float ColX { get => 0; set { } }
    public override float ColY { get; set; }
    public override float ColZ { get => 0; set { } }

    #region AUTO SAVE DATA /IAutoSaveData
    [Header("SLIDER")]
    [SerializeField] TransformBySlider slider;
    /// <summary>
    /// ItemInstantiatePlate's transform Slider controller
    /// </summary>
    public override float SliderValue {
        get {
            if (slider != null)
                return slider.slider.value;
            else
                return 0;
        }
        set {
            if (slider != null)
                slider.slider.value = value;           
        }
    }
    /// <summary>
    /// ItemInstantiatePlate's local position
    /// </summary>
    public override Vector3 LocalPosition {
        get {
            return transform.localPosition;
        }
        set {
            transform.localPosition = value;
        }
    }
    /// <summary>
    /// ItemInstantiatePlate's local eulerangles
    /// </summary>
    public override Vector3 LocalEulerAngles {
        get {
            return transform.localEulerAngles;
        }
        set {
            transform.localEulerAngles = value;
        }
    }
    #endregion


    protected override void Awake() {

        base.Awake();

        AutoSave = MainUIController.instance.GetComponent<AutoSaveGameObjectsData>();

        ClassesDependstGotSpawnedBoolean += delegate (bool isActive) {

            if (GetComponent<InstantiateItems>() != null) {

                if (GetComponent<InstantiateItems>().enabled != isActive)
                    GetComponent<InstantiateItems>().enabled = isActive;
            }
        };
    }

    protected override void OnEnable() {

        base.OnEnable();


    }

    protected override void Update() {

        base.Update();

        ClassesDependstGotSpawnedBoolean(GotSpawned);
    }






}


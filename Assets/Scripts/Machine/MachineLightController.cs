using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineLightController : MonoBehaviour
{
    [Header("LIGHTS")]
    [SerializeField] Material[] lightMaterialsPrefab;
    [SerializeField] MeshRenderer[] lights;

    [Header("MACHINE")]
    [SerializeField] Transform machine;

    /// <summary>
    /// 0: WhiteLight 1: YellowLight 2: GreenLight 3:RedLight 4:Frame
    /// </summary>
    Material[] LightMaterialsPrefab
    {
        get
        {
            return lightMaterialsPrefab;
        }
    }
    Material[] RunLightMaterials
    {
        get
        {
            return lights[0].sharedMaterials;
        }
        set
        {
            lights[0].sharedMaterials = value;
        }
    }
    Material[] AlertLightMaterials
    {
        get
        {
            return lights[1].sharedMaterials;
        }
        set
        {
            lights[1].sharedMaterials = value;
        }
    }
    IMachineLightsController IMachineLightsController
    {
        get
        {
            return machine.GetComponent<IMachineLightsController>();
        }
    }


    void OnEnable()
    {
        IMachineLightsController.OnSendMachineStatus += IMachineLightsController_OnSendMachineStatus;
    }

    void OnDisable()
    {
        IMachineLightsController.OnSendMachineStatus -= IMachineLightsController_OnSendMachineStatus;
    }

    void IMachineLightsController_OnSendMachineStatus(bool obj)
    {
        if (obj)
        {
            RunLightMaterials = new Material[2] { LightMaterialsPrefab[2], LightMaterialsPrefab[4] };
            AlertLightMaterials = new Material[2] { LightMaterialsPrefab[0], LightMaterialsPrefab[4] };
        }
        else
        {
            RunLightMaterials = new Material[2] { LightMaterialsPrefab[0], LightMaterialsPrefab[4] };
            AlertLightMaterials = new Material[2] { LightMaterialsPrefab[3], LightMaterialsPrefab[4] };
        }
    }

















}

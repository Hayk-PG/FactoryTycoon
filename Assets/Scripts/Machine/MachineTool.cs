using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineTool : MonoBehaviour
{
    [Header("TRANSFORM")]
    [SerializeField] Transform slidersContainer;

    public MachineUISliderController[] MachineSliders
    {
        get
        {
            return slidersContainer.GetComponentsInChildren<MachineUISliderController>();
        }
    }


   
}

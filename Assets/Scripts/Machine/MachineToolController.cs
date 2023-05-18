using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineToolController : MachineToolBase 
{
    #region MachineToolBase
    public override float MinLimit
    {
        get
        {
            return minLimit;
        }
        set
        {
            minLimit = value;
        }
    }
    public override float MaxLimit
    {
        get
        {
            return maxLimit;
        }
        set
        {
            maxLimit = value;
        }
    }
    #endregion

    public enum MACHINE_TOOL
    {
        NONE,
        MICROGAUGE_BASE_CYLINDER,
        PRESS_VERTICAL,
        PRERSS_HORIZONTAL,
    }
    public MACHINE_TOOL machine_tool;

    #region COMPONENTS
    public virtual MachineUIController MachineUI
    {
        get
        {
            return GetComponentInChildren<MachineUIController>();
        }
    }
    public virtual MachineUIRotate MachineUIRotate
    {
        get
        {
            return GetComponentInChildren<MachineUIRotate>();
        }
    }
    #endregion

    [Header("ITEM")]
    #region ITEM
    [SerializeField] float itemDeformationValue;
    [SerializeField] int itemAngle;
    public float ItemDeformationValue
    {
        get
        {
            return itemDeformationValue;
        }
        set
        {
            itemDeformationValue = value;
        }
    }
    public int ItemAngle
    {
        get
        {
            return itemAngle;
        }
        set
        {
            itemAngle = value;
        }
    }
    #endregion

    void Awake()
    {
        MinLimit = 0;
        MaxLimit = 1;
    }

    //protected override void OnEnable()
    //{
    //    base.OnEnable();
    //    MachineUI.OnClickMachineCtrlButton += MachineUI_OnClickMachineCtrlButton;
    //    MachineUIRotate.OnClickToRotateAnItem += MachineUIRotate_OnClickToRotateAnItem;
    //}
    
    //protected override void OnDisable()
    //{
    //    base.OnDisable();
    //    MachineUI.OnClickMachineCtrlButton -= MachineUI_OnClickMachineCtrlButton;
    //    MachineUIRotate.OnClickToRotateAnItem -= MachineUIRotate_OnClickToRotateAnItem;
    //}

    //protected override void Slider_OnSliderValueChanged(float sliderValue)
    //{
    //    ItemDeformationValue = sliderValue;
    //}

    //protected virtual void MachineUI_OnClickMachineCtrlButton(int obj)
    //{
    //    machine_tool = (MACHINE_TOOL)obj;
    //}

    //protected void MachineUIRotate_OnClickToRotateAnItem(int obj)
    //{
    //    ItemAngle = obj;
    //}


}

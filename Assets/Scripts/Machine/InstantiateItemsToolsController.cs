using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateItemsToolsController : MachineToolBase
{   
    /// <summary>
    /// Objects moves or rotates
    /// </summary>
    public enum TRANSFORM_TYPE { ROTATION, POSITION}
    /// <summary>
    /// In what axis object's moves or rotates
    /// </summary>
    public enum AXIS { X, Y, Z}

    public TRANSFORM_TYPE transform_Type;
    public AXIS axis;

    [Header("TOOLS")]
    [SerializeField] Transform[] tool;
    [SerializeField] int index;

    /// <summary>
    /// What tool is active now
    /// </summary>
    public Transform ActiveTool {
        get {
            return tool[index];
        }
    }

    /// <summary>
    /// Active object's localPosition or localEulerAngles
    /// </summary>
    public Vector3 ActiveToolTransform {
        get {
            if (transform_Type == TRANSFORM_TYPE.POSITION)
                return ActiveTool.localPosition;
            else
                return ActiveTool.localEulerAngles;
        }
        set {
            if (transform_Type == TRANSFORM_TYPE.POSITION)
                ActiveTool.localPosition = value;
            else
                ActiveTool.localEulerAngles = value;
        }
    }

     
    protected override void Slider_OnSliderValueChanged(float obj) {

        if (axis == AXIS.X) {
            ActiveToolTransform = new Vector3(obj, ActiveToolTransform.y, ActiveToolTransform.z);
        }
        if (axis == AXIS.Y) {
            ActiveToolTransform = new Vector3(ActiveToolTransform.x, obj, ActiveToolTransform.z);
        }
        if (axis == AXIS.Z) {
            ActiveToolTransform = new Vector3(ActiveToolTransform.x, ActiveToolTransform.y, obj);
        }
    }

    








}

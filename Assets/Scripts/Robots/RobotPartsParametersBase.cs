using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RobotPartsParametersBase : ConvertEulerAnglesRotation,IRobotParts
{
    public UnityEvent OnPartsTransform;

    public delegate void OnDropItemOnConveyor();
    public event OnDropItemOnConveyor OnDropItem;

    public enum ANGLE_AXIS { X, Y, Z}
    public ANGLE_AXIS angle_axis;

    public enum TRANSFORM_MANIPLUATION { ROTATION, MOVEMENT}
    public TRANSFORM_MANIPLUATION transform_manipulation;

    public RobotController.SELECTED_PART selected_part;

    public float CurrentAngle {

        get {

            if(transform_manipulation == TRANSFORM_MANIPLUATION.ROTATION) {

                return angle_axis == ANGLE_AXIS.Y ? ConvertedAxis(transform.localEulerAngles.y) : ConvertedAxis(transform.localEulerAngles.z);
            }
            else {

                return angle_axis == ANGLE_AXIS.X ? transform.localPosition.x : angle_axis == ANGLE_AXIS.Y ? transform.localPosition.y : transform.localPosition.z;
            }

        }
        set { }
    }
    public float DefaultPosAngle { get; set; }
    public float SecondPosAngle { get; set; }
    public float LastPosAngle { get; set; }

    bool dropeItem;

    float f;
    bool f_wasEqualHighestValue;

    float a;
    bool a_wasEqualHighestValue;

    float b;
    bool b_wasEqualHighestValue;

    float c;
    bool c_wasEqualHighestValue;

    float d;
    bool d_wasEqualHighestValue;

    float e;
    bool e_wasEqualHighestValue;

    [Header("LIMIT TRANSFORM ")]
    public float axisMin;
    public float axisMax;

    [SerializeField] float defaultPosTime;
    [SerializeField] float secondPosTimeFirstValue;
    [SerializeField] float secondPosTimeSecondValue;
    [SerializeField] bool isHandPart;
    float partsTime;
    bool isPartsTimeEqualsHighestValue;


    public void PartTransform(Transform part) {

        if (partsTime < 3 && !isPartsTimeEqualsHighestValue) {

            partsTime += Time.deltaTime;

            if (isHandPart) {

                OnDropItemEvent(() => { if (!dropeItem) { OnDropItem?.Invoke(); dropeItem = true; } });
            }
        }
        if (partsTime >= 3) {

            isPartsTimeEqualsHighestValue = true;

            if (isHandPart) {

                OnDropItemEvent(() => dropeItem = false);
            }
        }
        if (isPartsTimeEqualsHighestValue) {

            partsTime -= Time.deltaTime;
        }
        if (partsTime <= 0) {

            isPartsTimeEqualsHighestValue = false;
        }

        float axis = transform_manipulation == TRANSFORM_MANIPLUATION.MOVEMENT && angle_axis == ANGLE_AXIS.Y ? part.localPosition.y :
            transform_manipulation == TRANSFORM_MANIPLUATION.MOVEMENT && angle_axis == ANGLE_AXIS.Z ? part.localPosition.z :
            transform_manipulation == TRANSFORM_MANIPLUATION.MOVEMENT && angle_axis == ANGLE_AXIS.X ? part.localPosition.x :
            transform_manipulation == TRANSFORM_MANIPLUATION.ROTATION && angle_axis == ANGLE_AXIS.Y ? part.localEulerAngles.y :
            transform_manipulation == TRANSFORM_MANIPLUATION.ROTATION && angle_axis == ANGLE_AXIS.Z ? part.localEulerAngles.z :
            0;

        float a = defaultPosTime <= 0 ? 1 : defaultPosTime;
        float b = secondPosTimeFirstValue <= 0 ? 1 : secondPosTimeFirstValue;
        float c = secondPosTimeSecondValue <= 0 ? 2 : secondPosTimeSecondValue;

        float desiredAxis = Mathf.Lerp(transform_manipulation == TRANSFORM_MANIPLUATION.ROTATION ? ConvertedAxis(axis): axis,
            partsTime <= a ? DefaultPosAngle : partsTime > b && partsTime <= c ? SecondPosAngle : LastPosAngle, 5 * Time.deltaTime); 

        if(transform_manipulation == TRANSFORM_MANIPLUATION.ROTATION) {

            part.localEulerAngles = angle_axis == ANGLE_AXIS.Y ? new Vector3(0, Mathf.Clamp(desiredAxis, axisMin, axisMax), 0) : new Vector3(0, 0, Mathf.Clamp(desiredAxis, axisMin, axisMax));
        }
        else {

            part.localPosition = angle_axis == ANGLE_AXIS.X ? new Vector3(Mathf.Clamp(desiredAxis, axisMin, axisMax), 0, 0) : 
                angle_axis == ANGLE_AXIS.Y ? new Vector3(0, Mathf.Clamp(desiredAxis, axisMin, axisMax), 0) : new Vector3(0, 0, Mathf.Clamp(desiredAxis, axisMin, axisMax));
        }
    }

    void OnDropItemEvent(System.Action Drop) {

        Drop?.Invoke();
    }

    


}

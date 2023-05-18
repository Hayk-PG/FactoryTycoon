using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(RobotPartsParametersBase))]
public class RobotPartsSeparateAdjustment : ConvertEulerAnglesRotation
{
    public UnityEvent OnRobotPartsRotationForwardsAdjustment;
    public UnityEvent OnRobotPartsRotationBackwardsAdjustment;

    [SerializeField] float degree;
    [SerializeField] float distance;

    public void BaseForwards(Transform SelectedPart) {

        float y = ConvertedAxis(SelectedPart.localEulerAngles.y);

        if (y < GetComponent<RobotPartsParametersBase>().axisMax) {

            SelectedPart.transform.localEulerAngles = new Vector3(0, y += degree * Time.deltaTime, 0);
        }
        else return;
    }

    public void BaseBackwards(Transform SelectedPart) {

        float y = ConvertedAxis(SelectedPart.localEulerAngles.y);

        if (y > GetComponent<RobotPartsParametersBase>().axisMin) {

            SelectedPart.transform.localEulerAngles = new Vector3(0, y -= degree * Time.deltaTime, 0);
        }
        else return;
    }

    public void VerticalMovementForwards(Transform SelectedPart) {

        float y = SelectedPart.localPosition.y;

        if (y > GetComponent<RobotPartsParametersBase>().axisMax) {

            SelectedPart.transform.localPosition = new Vector3(0, GetComponent<RobotPartsParametersBase>().axisMax, 0);
        }
        else {

            SelectedPart.transform.localPosition = new Vector3(0, y += distance * Time.deltaTime, 0);
        }
    }

    public void VerticalMovementBackwards(Transform SelectedPart) {

        float y = SelectedPart.localPosition.y;

        if (y < GetComponent<RobotPartsParametersBase>().axisMin) {

            SelectedPart.transform.localPosition = new Vector3(0, GetComponent<RobotPartsParametersBase>().axisMin, 0);
        }
        else {

            SelectedPart.transform.localPosition = new Vector3(0, y -= distance * Time.deltaTime, 0);
        }
    }

    public void HorizontalMovementForwards(Transform SelectedPart) {

        float x = SelectedPart.localPosition.x;

        if (x > GetComponent<RobotPartsParametersBase>().axisMax) {

            SelectedPart.transform.localPosition = new Vector3(GetComponent<RobotPartsParametersBase>().axisMax, 0, 0);
        }
        else {

            SelectedPart.transform.localPosition = new Vector3(x += distance * Time.deltaTime, 0, 0);
        }
    }

    public void HorizontalMovementBackwards(Transform SelectedPart) {

        float x = SelectedPart.localPosition.x;

        if (x < GetComponent<RobotPartsParametersBase>().axisMin) {

            SelectedPart.transform.localPosition = new Vector3(GetComponent<RobotPartsParametersBase>().axisMin, 0, 0);
        }
        else {

            SelectedPart.transform.localPosition = new Vector3(x -= distance * Time.deltaTime, 0, 0);
        }
    }
   
    public void CommonForwards(Transform SelectedPart) {

        float z = ConvertedAxis(SelectedPart.localEulerAngles.z);

        if (z < GetComponent<RobotPartsParametersBase>().axisMax) {

            SelectedPart.transform.localEulerAngles = new Vector3(0, 0, z += degree * Time.deltaTime);
        }
        else return;

        //float z = SelectedPart.localEulerAngles.z;
        //SelectedPart.transform.localEulerAngles = new Vector3(0, 0, z += degree * Time.deltaTime);
    }

    public void CommonBackwards(Transform SelectedPart) {

        float z = ConvertedAxis(SelectedPart.localEulerAngles.z);

        if (z > GetComponent<RobotPartsParametersBase>().axisMin) {

            SelectedPart.transform.localEulerAngles = new Vector3(0, 0, z -= degree * Time.deltaTime);
        }
        else return;

        //float z = SelectedPart.localEulerAngles.z;
        //SelectedPart.transform.localEulerAngles = new Vector3(0, 0, z -= degree * Time.deltaTime);
    }

    #region Outdated
    //public void FirstForwards(Transform SelectedPart) {

    //    float y = SelectedPart.localPosition.y;

    //    if (y > GetComponent<RobotPartsParametersBase>().axisMax) {

    //        SelectedPart.transform.localPosition = new Vector3(0, GetComponent<RobotPartsParametersBase>().axisMax, 0);
    //    }
    //    else {

    //        SelectedPart.transform.localPosition = new Vector3(0, y += distance * Time.deltaTime, 0);
    //    }
    //}

    //public void FirstBackwards(Transform SelectedPart) {

    //    float y = SelectedPart.localPosition.y;

    //    if (y < GetComponent<RobotPartsParametersBase>().axisMin) {

    //        SelectedPart.transform.localPosition = new Vector3(0, GetComponent<RobotPartsParametersBase>().axisMin, 0);
    //    }
    //    else {

    //        SelectedPart.transform.localPosition = new Vector3(0, y -= distance * Time.deltaTime, 0);
    //    }
    //}

    //public void LastForwards(Transform SelectedPart) {

    //    float y = SelectedPart.localPosition.y;

    //    if (y > GetComponent<RobotPartsParametersBase>().axisMax) {

    //        SelectedPart.transform.localPosition = new Vector3(0, GetComponent<RobotPartsParametersBase>().axisMax, 0);
    //    }
    //    else {

    //        SelectedPart.transform.localPosition = new Vector3(0, y += distance * Time.deltaTime, 0);
    //    }
    //}

    //public void LastBackwards(Transform SelectedPart) {

    //    float y = SelectedPart.localPosition.y;

    //    if (y < GetComponent<RobotPartsParametersBase>().axisMin) {

    //        SelectedPart.transform.localPosition = new Vector3(0, GetComponent<RobotPartsParametersBase>().axisMin, 0);
    //    }
    //    else {

    //        SelectedPart.transform.localPosition = new Vector3(0, y -= distance * Time.deltaTime, 0);
    //    }
    //}
    #endregion
















}

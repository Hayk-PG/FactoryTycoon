using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsReferenceForRobot : MonoBehaviour
{
    //INTERNAL
    [Header("OBJECTS REFERENCE FOR ROBOT")]
    [SerializeField] RobotRotation robotRotation; internal RobotRotation RobotRotation => robotRotation;
    [SerializeField] RobotPartsParametersBase robotPartsParamBase; internal RobotPartsParametersBase RobotPartsParamBase => robotPartsParamBase;
    [SerializeField] RobotPartsSeparateAdjustment robotSeparateAdjustment; internal RobotPartsSeparateAdjustment RobotSeparateAdjustment => robotSeparateAdjustment;
    [SerializeField] RobotController robotController; internal RobotController RobotController => robotController;

    [Header("UI")]
    [SerializeField] RobotsUIButtonsController robButtonCntrlPrefab; internal RobotsUIButtonsController RobotUiButtonsController => robButtonCntrlPrefab;
    [SerializeField] RobotsUITextController robotsUITextController;  internal RobotsUITextController RobotsUITextController => robotsUITextController;


}

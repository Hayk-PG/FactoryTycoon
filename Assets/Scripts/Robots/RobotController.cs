using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    [SerializeField] ObjectsReferenceForRobot objReference;
    public enum SELECTED_PART { Base, First, Second, Third, Fourth, Fivth, Sixth, Seventh}
    public SELECTED_PART selected_Part;

    public enum ROTATION_DIRECTION { Forwards, Backwards}
    public ROTATION_DIRECTION rotation_direction;
       
    [Header("PARTS")]
    public Transform[] robotMovingParts;
    [HideInInspector] public Transform selectedPart;

    /// <summary>
    /// Get selected part index
    /// </summary>
    public int SelectedPartIndex { get; set; }

    [Header("PARTS INITIAL TRANSFORM")]
    public Vector3[] PartsInitialLocalPosition;
    public Vector3[] PartInitialLocalEulerAngles;

    void Awake() {

        selectedPart = robotMovingParts[0];
    }

    void Start() {

        CachePartsInitialTransform();
    }

    void OnEnable() {

        objReference.RobotUiButtonsController._OnClickRobotParts += RobotButtons_OnClickRobotParts;
        objReference.RobotsUITextController.OnUpdateRobotPartsValuesFromUITextController += RobotsUITextController_OnUpdateRobotPartsValuesFromUITextController;
        objReference.RobotUiButtonsController._OnClickRobotPartsUpdatePosValues += RobotButtons__OnClickRobotPartsUpdatePosValues;
        objReference.RobotUiButtonsController.OnClickResetPartTransform += RobotUiButtonsController_OnClickResetPartTransform;

        objReference.RobotUiButtonsController.OnIncreaseRotAngle += RobotButtons_OnIncreaseRotAngle;
        objReference.RobotUiButtonsController.OnDecreaseRotAngle += RobotButtons_OnDecreaseRotAngle;
    }
  
    void OnDisable() {

        objReference.RobotUiButtonsController._OnClickRobotParts -= RobotButtons_OnClickRobotParts;
        objReference.RobotsUITextController.OnUpdateRobotPartsValuesFromUITextController -= RobotsUITextController_OnUpdateRobotPartsValuesFromUITextController;
        objReference.RobotUiButtonsController._OnClickRobotPartsUpdatePosValues -= RobotButtons__OnClickRobotPartsUpdatePosValues;
        objReference.RobotUiButtonsController.OnClickResetPartTransform -= RobotUiButtonsController_OnClickResetPartTransform;

        objReference.RobotUiButtonsController.OnIncreaseRotAngle -= RobotButtons_OnIncreaseRotAngle;
        objReference.RobotUiButtonsController.OnDecreaseRotAngle -= RobotButtons_OnDecreaseRotAngle;
    }

    void Update() {

        if (objReference.RobotRotation.isRotating) {

            if(rotation_direction == ROTATION_DIRECTION.Forwards) {

                objReference.RobotRotation.RotateForwards(ref selectedPart);
            }
            if (rotation_direction == ROTATION_DIRECTION.Backwards) {

                objReference.RobotRotation.RotateBackwards(ref selectedPart);
            }
        }
    }

    void CachePartsInitialTransform() {

        PartsInitialLocalPosition = new Vector3[robotMovingParts.Length];
        PartInitialLocalEulerAngles = new Vector3[PartsInitialLocalPosition.Length];

        for (int p = 0; p < PartsInitialLocalPosition.Length; p++) {
            PartsInitialLocalPosition[p] = robotMovingParts[p].localPosition;
            PartInitialLocalEulerAngles[p] = robotMovingParts[p].localEulerAngles;
        }
    }

    void RobotButtons_OnClickRobotParts(int index) {

        SelectedPartIndex = index;

        selected_Part = (SELECTED_PART)index;

        selectedPart = robotMovingParts[index];
    }

    void RobotsUITextController_OnUpdateRobotPartsValuesFromUITextController(UnityEngine.UI.Text text)
    {
        IRobotParts rp = selectedPart?.GetComponent<IRobotParts>();

        text.text = "DP:" + rp.DefaultPosAngle + " SP:" + rp.SecondPosAngle + " LP:" + rp.LastPosAngle;
    }

    void RobotButtons__OnClickRobotPartsUpdatePosValues(UnityEngine.UI.Text text) {

        IRobotParts rp = selectedPart?.GetComponent<IRobotParts>();

        text.text = "DP:" + rp.DefaultPosAngle + " SP:" + rp.SecondPosAngle + " LP:" + rp.LastPosAngle;
    }

    void RobotButtons_OnIncreaseRotAngle(bool isPointerOn) {

        objReference.RobotRotation.isRotating = isPointerOn;

        rotation_direction = ROTATION_DIRECTION.Forwards;
    }

    void RobotButtons_OnDecreaseRotAngle(bool isPointerOn) {

        objReference.RobotRotation.isRotating = isPointerOn;

        rotation_direction = ROTATION_DIRECTION.Backwards;
    }

    void RobotUiButtonsController_OnClickResetPartTransform() {

        robotMovingParts[SelectedPartIndex].localPosition = PartsInitialLocalPosition[SelectedPartIndex];
        robotMovingParts[SelectedPartIndex].localEulerAngles = PartInitialLocalEulerAngles[SelectedPartIndex];
    }













}

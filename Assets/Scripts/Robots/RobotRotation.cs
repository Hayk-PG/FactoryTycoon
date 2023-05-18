using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotRotation : MonoBehaviour {

    [SerializeField] ObjectsReferenceForRobot objReference;

    [Header("ROTATION PARAMETERS")]
    public bool isRotating;

    IEnumerator RotateCoroutine;
    [SerializeField] bool run;
    [SerializeField] bool isRunning;


    void OnEnable() {

        objReference.RobotUiButtonsController.OnControlRobot += RobotButtons_OnControlRobot;
    }
    
    void OnDisable() {

        objReference.RobotUiButtonsController.OnControlRobot -= RobotButtons_OnControlRobot;
    }

    void Start() {

        RotateCoroutine = Rotate();
    }
  
    public void RotateForwards(ref Transform SelectedPart) {

        SelectedPart.GetComponent<RobotPartsSeparateAdjustment>()?.OnRobotPartsRotationForwardsAdjustment?.Invoke();
    }

    public void RotateBackwards(ref Transform SelectedPart) {

        SelectedPart.GetComponent<RobotPartsSeparateAdjustment>()?.OnRobotPartsRotationBackwardsAdjustment?.Invoke();        
    }
   
    void RobotButtons_OnControlRobot(bool run) {

        if (run) {

            if (!isRunning) {

                StartCoroutine(RotateCoroutine);

                isRunning = true;
            }
        }
        else {

            StopCoroutine(RotateCoroutine);

            isRunning = false;
        }
    }

    IEnumerator Rotate() {

        yield return null;

        float i = 0;

        while (i < 5) {

            for (int r = 0; r < objReference.RobotController.robotMovingParts.Length; r++) {

                objReference.RobotController.robotMovingParts[r].GetComponent<RobotPartsParametersBase>().OnPartsTransform?.Invoke();               
            }

            i = (i + Time.deltaTime) % 5;

            yield return null;
        }
    }












}

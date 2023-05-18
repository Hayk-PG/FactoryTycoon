using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotHandController : MonoBehaviour
{
    [SerializeField] ObjectsReferenceForRobot objReference;

    public delegate void OnAutoCatch(bool isAutoMode, IMovement triggeredItem);
    public event OnAutoCatch _OnAutoCatch;

    [Header("COLLIDER")]
    [SerializeField] BoxCollider triggerCol;
    public Collider TriggerCol { get { return triggerCol; } }

    [Header("BOOL")]
    [SerializeField] bool isTriggered;
    public bool IsManualMode { get; set; } = true;

    IMovement triggeredItem;


    void OnEnable() {

        objReference.RobotUiButtonsController.OnManualCatch += RobotButtons_OnManualCatch;
        objReference.RobotUiButtonsController.OnRobotManualMode += RobotButtons_OnRobotManualMode;

        _OnAutoCatch += RobotHandController_OnAutoCatch;

        objReference.RobotPartsParamBase.OnDropItem += BasePart_OnDropItem;
    }
    
    void OnDisable() {

        objReference.RobotUiButtonsController.OnManualCatch -= RobotButtons_OnManualCatch;
        objReference.RobotUiButtonsController.OnRobotManualMode -= RobotButtons_OnRobotManualMode;

        _OnAutoCatch -= RobotHandController_OnAutoCatch;

        objReference.RobotPartsParamBase.OnDropItem -= BasePart_OnDropItem;
    }

    void OnTriggerEnter(Collider other) {

        if(other.GetComponent<IMovement>() != null) {

            isTriggered = true;

            triggeredItem = other.GetComponent<IMovement>();

            _OnAutoCatch?.Invoke(!IsManualMode, triggeredItem);
        }
    }

    void OnTriggerExit(Collider other) {

        isTriggered = false;
        triggeredItem = null;
    }

    private void RobotButtons_OnRobotManualMode(bool isManualMode) {

        this.IsManualMode = isManualMode;
    }

    void CatchAndReleaseItem(IMovement triggeredItem, Transform parent, bool useGravity, bool usParent) {

        triggeredItem._Transform.parent = usParent ? transform : null;
        triggeredItem.Rb.useGravity = useGravity;
    }

    void RobotButtons_OnManualCatch(bool _catch) {

        if (_catch) {

            if (isTriggered && triggeredItem != null) {

                if (IsManualMode) {

                    CatchAndReleaseItem(triggeredItem, transform, false, true);
                }
            }
        }
        else {

            if (triggeredItem != null) {

                if (IsManualMode) {

                    CatchAndReleaseItem(triggeredItem, null, true, false);
                }               
            }
        }
    }  

    private void RobotHandController_OnAutoCatch(bool isAutoMode, IMovement triggeredItem) {

        if (isAutoMode) {

            CatchAndReleaseItem(triggeredItem, transform, false, true);
        }
        else {

            return;
        }
    }

    private void BasePart_OnDropItem() {

        if (triggeredItem != null) {

            if (!IsManualMode) {

                CatchAndReleaseItem(triggeredItem, null, true, false);
            }
        }
    }

   

}

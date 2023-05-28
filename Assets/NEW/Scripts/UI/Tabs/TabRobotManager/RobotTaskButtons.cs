using UnityEngine;

public class RobotTaskButtons : MonoBehaviour
{
    [SerializeField] private Btn _firstTargetRotationAssignerButton;
    [SerializeField] private Btn _secondTargetRotationAssignerButton;
    [SerializeField] private Btn _thirdTargetRotationAssignerButton;
    [SerializeField] private Btn _runButton;




    private void OnEnable()
    {
        _firstTargetRotationAssignerButton.OnSelect += () => OnTaskSelect(RobotTaskType.SetTargetRotation1);
        _secondTargetRotationAssignerButton.OnSelect += () => OnTaskSelect(RobotTaskType.SetTargetRotation2);
        _thirdTargetRotationAssignerButton.OnSelect += () => OnTaskSelect(RobotTaskType.SetTargetRotation3);

        _runButton.OnSelect += () => OnTaskSelect(RobotTaskType.Run);
    }

    private void OnTaskSelect(RobotTaskType robotTaskType)
    {
        References.Manager.RobotTaskManager.RaiseRobotTaskEvent(robotTaskType);
    }
}

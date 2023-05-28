using UnityEngine;
using Pautik;

public class RobotJointsSelectionButtons : MonoBehaviour
{
    [SerializeField] private Btn[] _robotJointSelectionButtons;

    private object[] _data = new object[1];



    private void OnEnable()
    {
        // Subscribe to the OnSelect event of each robot joint selection button
        // Assign a lambda expression as the event listener to capture the button index
        GlobalFunctions.Loop<Btn>.Foreach(_robotJointSelectionButtons, btnRobotJoint => btnRobotJoint.OnSelect += () => OnRobotJointSelect(btnRobotJoint.transform.GetSiblingIndex()));
    }

    private void OnRobotJointSelect(int index)
    {
        // Joint associated button index
        _data[0] = index;

        // Raise an event to notify the RobotTaskManager about the joint selection
        References.Manager.RobotTaskManager.RaiseRobotTaskEvent(RobotTaskType.SelectJoint, _data);
    }
}
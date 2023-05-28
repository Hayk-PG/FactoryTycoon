using UnityEngine;

public class RobotManager : MonoBehaviour
{
    [SerializeField] private RobotJointManager[] _robotJoints;

    public RobotJointManager[] RobotJoints => _robotJoints;
}

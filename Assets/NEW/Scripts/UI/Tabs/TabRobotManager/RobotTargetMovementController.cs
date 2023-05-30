using UnityEngine;

public class RobotTargetMovementController    : MonoBehaviour
{
    [SerializeField] private FixedJoystick _movementJoystick;
    [SerializeField] private FixedJoystick _rotationJoystick;

    private object[] _data = new object[2];

    private bool IsMovementJoystickReleased
    {
        get => _movementJoystick.Direction.x < 0.1f && _movementJoystick.Direction.x > -0.1f && _movementJoystick.Direction.y < 0.1f && _movementJoystick.Direction.y > -0.1f;
    }
    private bool IsRotationJoystickReleased
    {
        get => _rotationJoystick.Direction.x > -0.1f && _rotationJoystick.Direction.x < 0.1f;
    }




    private void FixedUpdate()
    {
        SendMovementDataToRobot();
        SendRotationDataToRobot();
    }

    private void SendMovementDataToRobot()
    {
        if(IsMovementJoystickReleased)
        {
            return;
        }

        _data[0] = _movementJoystick.Direction;

        References.Manager.RobotTaskManager.RaiseRobotTaskEvent(RobotTaskType.Move, _data);
    }

    private void SendRotationDataToRobot()
    {
        if (IsRotationJoystickReleased)
        {
            return;
        }

        _data[1] = _rotationJoystick.Direction.x;

        References.Manager.RobotTaskManager.RaiseRobotTaskEvent(RobotTaskType.Rotate, _data);
    }
}
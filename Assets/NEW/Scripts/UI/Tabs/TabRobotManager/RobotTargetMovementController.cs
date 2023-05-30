using UnityEngine;
using TMPro;
using Pautik;

public class RobotTargetMovementController : MonoBehaviour
{
    [Header("Joysticks")]
    [SerializeField] private FixedJoystick _movementJoystick;
    [SerializeField] private FixedJoystick _rotationJoystick;

    [Header("Texts")]
    [SerializeField] private TMP_Text _robotPositionText;
    [SerializeField] private TMP_Text _robotRotationText;
    [SerializeField] private TMP_Text _endEffectorRotationText;

    [Header("Toggle")]
    [SerializeField] private CustomToggle _toggle;

    private bool _isEndEffectorRotationOn = false;

    private object[] _data = new object[2];

    private bool IsMovementJoystickReleased
    {
        get => _movementJoystick.Direction.x < 0.1f && _movementJoystick.Direction.x > -0.1f && _movementJoystick.Direction.y < 0.1f && _movementJoystick.Direction.y > -0.1f;
    }
    private bool IsRotationJoystickReleased
    {
        get => _rotationJoystick.Direction.x > -0.1f && _rotationJoystick.Direction.x < 0.1f;
    }




    private void OnEnable()
    {
        References.Manager.RobotTaskManager.OnRobotTask += OnRobotTask;

        _toggle.OnValueChange += isOn => _isEndEffectorRotationOn = isOn;
    }

    private void FixedUpdate()
    {
        SendMovementDataToRobot();
        SendRobotRotationDataToRobot();
    }

    private void OnRobotTask(RobotTaskType robotTaskType, object[] data)
    {
        DisplayRobotPosition(robotTaskType, data);
        DisplayRobotRotation(robotTaskType, data);
        DisplayEndEffectorRotation(robotTaskType, data);
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

    private void SendRobotRotationDataToRobot()
    {
        if (IsRotationJoystickReleased)
        {
            return;
        }

        _data[1] = _rotationJoystick.Direction.x;

        References.Manager.RobotTaskManager.RaiseRobotTaskEvent(_isEndEffectorRotationOn ? RobotTaskType.RotateEndEffector : RobotTaskType.RotateRobot, _data);
    }

    private void DisplayRobotPosition(RobotTaskType robotTaskType, object[] data)
    {
        if(robotTaskType != RobotTaskType.ObserveRobotPosition)
        {
            return;
        }

        Vector2 position = (Vector2)data[0];

        _robotPositionText.text = $"Position: {position}";
    }

    private void DisplayRobotRotation(RobotTaskType robotTaskType, object[] data)
    {
        if (robotTaskType != RobotTaskType.ObserveRobotRotation)
        {
            return;
        }

        float angle = (float)data[1];

        _robotRotationText.text = $"Rotation: {Converter.DecimalString(Mathf.RoundToInt(angle))}°";
    }

    private void DisplayEndEffectorRotation(RobotTaskType robotTaskType, object[] data)
    {
        if (robotTaskType != RobotTaskType.ObservreEndEffectorRotation)
        {
            return;
        }

        float angle = (float)data[1];

        _robotRotationText.text = $"End Effector Rotation: {Converter.DecimalString(Mathf.RoundToInt(angle))}°";
    }
}
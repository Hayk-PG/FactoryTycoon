using UnityEngine;
using TMPro;
using Pautik;

public class RobotTargetMovementController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private FixedJoystick _movementJoystick; // Joystick for movement control
    [SerializeField] private FixedJoystick _rotationJoystick; // Joystick for rotation control
    [SerializeField] private TMP_Text _robotPositionText; // Text displaying robot position
    [SerializeField] private TMP_Text _robotRotationText; // Text displaying robot rotation
    [SerializeField] private TMP_Text _endEffectorRotationText; // Text displaying end effector rotation
    [SerializeField] private CustomToggle _toggle; // Toggle for end effector rotation control

    private bool _isEndEffectorRotationOn = false;

    private object[] _data = new object[2];

    // Check if the movement joystick is released (not being used)
    private bool IsMovementJoystickReleased
    {
        get => _movementJoystick.Direction.x < 0.1f && _movementJoystick.Direction.x > -0.1f && _movementJoystick.Direction.y < 0.1f && _movementJoystick.Direction.y > -0.1f;
    }
    // Check if the rotation joystick is released (not being used)
    private bool IsRotationJoystickReleased
    {
        get => _rotationJoystick.Direction.x > -0.1f && _rotationJoystick.Direction.x < 0.1f;
    }




    private void OnEnable()
    {
        References.Manager.RobotTaskManager.OnRobotTask += OnRobotTask;

        // Subscribe to the OnValueChange event of the toggle
        _toggle.OnValueChange += isOn => _isEndEffectorRotationOn = isOn;
    }

    private void FixedUpdate()
    {
        SendMovementDataToRobot();
        SendRotationDataToRobot();
    }

    // Event handler for the OnRobotTask event
    private void OnRobotTask(RobotTaskType robotTaskType, object[] data)
    {
        DisplayRobotPosition(robotTaskType, data);
        DisplayRobotRotation(robotTaskType, data);
        DisplayEndEffectorRotation(robotTaskType, data);
    }

    // Send movement data to the robot based on joystick input
    private void SendMovementDataToRobot()
    {
        if(IsMovementJoystickReleased)
        {
            return; // No movement input, so return
        }

        _data[0] = _movementJoystick.Direction;

        // Raise the RobotTask event with the Move task type and movement data
        References.Manager.RobotTaskManager.RaiseRobotTaskEvent(RobotTaskType.Move, _data);
    }

    // Send robot rotation data to the robot based on joystick input
    private void SendRotationDataToRobot()
    {
        if (IsRotationJoystickReleased)
        {
            return; // No rotation input, so return
        }

        _data[1] = _rotationJoystick.Direction.x;

        // Raise the RobotTask event with the appropriate task type based on the toggle state
        References.Manager.RobotTaskManager.RaiseRobotTaskEvent(_isEndEffectorRotationOn ? RobotTaskType.RotateEndEffector : RobotTaskType.RotateRobot, _data);
    }

    // Display the robot position in the UI
    private void DisplayRobotPosition(RobotTaskType robotTaskType, object[] data)
    {
        if(robotTaskType != RobotTaskType.ObserveRobotPosition)
        {
            return;
        }

        Vector2 position = (Vector2)data[0];

        _robotPositionText.text = $"Position: {position}";
    }

    // Display the robot rotation in the UI
    private void DisplayRobotRotation(RobotTaskType robotTaskType, object[] data)
    {
        if (robotTaskType != RobotTaskType.ObserveRobotRotation)
        {
            return; // Not observing robot rotation, so return
        }

        float angle = (float)data[1];

        PrintToggleLabelText("Rotation: ", angle);
    }

    // Display the end effector rotation in the UI
    private void DisplayEndEffectorRotation(RobotTaskType robotTaskType, object[] data)
    {
        if (robotTaskType != RobotTaskType.ObservreEndEffectorRotation)
        {
            return; // Not observing end effector rotation, so return
        }

        float angle = (float)data[1];

        PrintToggleLabelText("End Effector Rotation: ", angle);
    }

    // Print the toggle label text with the given text and value
    private void PrintToggleLabelText(string text, float value)
    {
        _robotRotationText.text = text + Converter.DecimalString(Mathf.RoundToInt(value));
    }
}
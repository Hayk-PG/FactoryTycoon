using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Pautik;
using System;

public class RobotJointManager : MonoBehaviour
{
    private enum AngleState { X, Y, Z }

    [Header("Joint Transform")]
    [SerializeField] private Transform _joint; // Reference to the joint transform to rotate

    [Header("Rotation Angle State")]
    [SerializeField] private AngleState _angleState; // Current angle state for rotation

    // Function delegates for angle-related calculations
    private Func<Vector3, float> GetAngle;
    private Func<Vector3, Vector3, bool> IsJointAngleLessThan;
    private Func<Vector3, Vector3, Vector3, float> GetRange;
    private Func<float, Vector3> Rotation;

    private Vector3 _startRotation; // Starting rotation of the joint
    private Vector3 _targetRotation; // Target rotation of the joint

    [Header("Angle Limits")]
    [SerializeField] private float _minAngle = -50; // Minimum angle for the slider
    [SerializeField] private float _maxAngle = 50; // Maximum angle for the slider
    private float _speed = 50; // Rotation speed

    [Header("Joint Index")]
    [SerializeField] private int _jointIndex;

    private bool _isJointSelected ;
    private bool _run; // Flag to control joint rotation
    private bool _isStartRotationMatchPassed; // Flag indicating if the start rotation has been matched

    private object[] _data = new object[10];




    private void Start()
    {
        SetAngleState();
    }

    private void OnEnable()
    {
        References.Manager.RobotTaskManager.OnRobotTask += OnRobotTask;
    }

    // Handles the robot task based on the specified task type and data.
    private void OnRobotTask(RobotTaskType robotTaskType, object[] data)
    {
        // Check if the robot task is for selecting a joint and handle the joint selection accordingly
        CheckAndHandleJointSelection(robotTaskType, data);

        // Update the rotation based on the slider value, if the robot task is for getting joint slider values
        UpdateRotationWithSlider(robotTaskType, data);
    }

    // Checks if the robot task is for selecting a joint and handles the joint selection accordingly.
    private void CheckAndHandleJointSelection(RobotTaskType robotTaskType, object[] data)
    {
        if (robotTaskType != RobotTaskType.SelectJoint)
        {
            return;
        }

        // Check if the selected joint matches the current joint
        _isJointSelected = (int)data[0] == _jointIndex;

        if (_isJointSelected )
        {
            // Set the joint slider value based on the current joint's local rotation
            SetJointSliderValue();
        }
    }

    // Sets the joint slider value based on the current joint's local rotation.
    private void SetJointSliderValue()
    {
        Quaternion jointLocalRotation = _joint.transform.localRotation;
        Vector3 jointEulerAngles  = jointLocalRotation.eulerAngles;

        // Prepare the data to be sent for setting the joint slider values
        _data[0] = _jointIndex; // Joint index
        _data[1] = _minAngle; // Minimum angle for the slider
        _data[2] = _maxAngle; // Maximum angle for the slider
        _data[3] = GetAngle(Angle(jointEulerAngles)); // Current joint angle

        // Raise an event to set the joint slider values
        References.Manager.RobotTaskManager.RaiseRobotTaskEvent(RobotTaskType.SetJointSliderValues, _data);
    }

    // Updates the rotation based on the slider value, if the robot task is for getting joint slider values.
    private void UpdateRotationWithSlider(RobotTaskType robotTaskType, object[] data)
    {
        if(robotTaskType != RobotTaskType.GetJointSliderValues)
        {
            return;
        }

        // Check if the rotation can be updated based on the current conditions
        bool canUpdateRotation  = !_run && _jointIndex == (int)data[0] && _isJointSelected ;

        if (canUpdateRotation )
        {
            float sliderValue = (float)data[1]; // Get the slider value

            // Update the rotation based on the slider value
            UpdateRotation(Rotation(sliderValue));

            _isStartRotationMatchPassed = false;
        }
    }

    // Set angle-related function delegates based on the angle state
    private void SetAngleState()
    {
        switch (_angleState)
        {
            case AngleState.X:

                GetAngle = delegate (Vector3 rotation) { return rotation.x; };
                IsJointAngleLessThan = delegate (Vector3 a, Vector3 b) { return a.x < b.x; };
                GetRange = delegate (Vector3 a, Vector3 b, Vector3 value) { return Mathf.InverseLerp(a.x, b.x, value.x); };
                Rotation = delegate (float angle) { return new Vector3(angle, 0, 0); };

                break;

            case AngleState.Y:

                GetAngle = delegate (Vector3 rotation) { return rotation.y; };
                IsJointAngleLessThan = delegate (Vector3 a, Vector3 b) { return a.y < b.y; };
                GetRange = delegate (Vector3 a, Vector3 b, Vector3 value) { return Mathf.InverseLerp(a.y, b.y, value.y); };
                Rotation = delegate (float angle) { return new Vector3(0, angle, 0); };

                break;
            case AngleState.Z:

                GetAngle = delegate (Vector3 rotation) { return rotation.z; };
                IsJointAngleLessThan = delegate (Vector3 a, Vector3 b) { return a.z < b.z; };
                GetRange = delegate (Vector3 a, Vector3 b, Vector3 value) { return Mathf.InverseLerp(a.z, b.z, value.z); };
                Rotation = delegate (float angle) { return new Vector3(0, 0, angle); };

                break;
        }
    }

    // Toggle the run flag and start the joint rotation coroutine
    private void ToggleRun()
    {
        _run = !_run;

        StartCoroutine(JointRotationCoroutine());
    }

    private void SetDelay()
    {
        // TODO: Set the delay logic
    }

    // Set the target rotation to the current joint rotation
    private void OnSetTargetRotation()
    {
        _targetRotation = _joint.localEulerAngles;
    }

    // Set the start rotation to the current joint rotation
    private void SetStartRotation()
    {
        _startRotation = _joint.localEulerAngles;
    }

    // Check and perform joint rotation
    private IEnumerator JointRotationCoroutine()
    {
        while (_run)
        {
            CheckAndPerformJointRotation();

            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }

    private void CheckAndPerformJointRotation()
    {
        bool isRotationMatchStart = _joint.localEulerAngles == _startRotation;
        bool isRotationMatchTarget = _joint.localEulerAngles == _targetRotation;

        CalculateRange(out float range);

        if (!isRotationMatchStart && !_isStartRotationMatchPassed)
        {
            // Run joint rotation towards start rotation
            RunJointRotation(Angle(_startRotation), range <= 0.01f, true);
        }
        else if (!isRotationMatchTarget && _isStartRotationMatchPassed)
        {
            // Run joint rotation towards target rotation
            RunJointRotation(Angle(_targetRotation), range >= 0.99f, false);
        }
    }

    // Check if the current joint angle is less than the target angle
    private void RunJointRotation(Vector3 targetRotation, bool stopRotation, bool toggleStartRotationMatch)
    {
        bool isJointAngleLessThan = IsJointAngleLessThan(Angle(_joint.localEulerAngles), Angle(targetRotation));

        CalculateAngle(isJointAngleLessThan, out float angle);
        UpdateRotation(Rotation(angle));

        if (stopRotation)
        {
            // Stop rotation at the target angle and toggle the start rotation match flag
            UpdateRotation(targetRotation);
            ToggleStartRotationMatch(toggleStartRotationMatch);
        }
    }

    private void CalculateAngle(bool increaseAngle, out float angle)
    {
        angle = GetAngle(Angle(_joint.localEulerAngles));

        if (increaseAngle)
        {
            angle += _speed * Time.fixedDeltaTime;
        }
        else
        {
            angle -= _speed * Time.fixedDeltaTime;
        }
    }

    private void CalculateRange(out float range)
    {
        range = GetRange(Angle(_startRotation), Angle(_targetRotation), Angle(_joint.eulerAngles));
    }

    // Update the joint rotation
    private void UpdateRotation(Vector3 rotation)
    {
        _joint.localEulerAngles = rotation;
    }

    // Toggle the start rotation match flag
    private void ToggleStartRotationMatch(bool isStartRotationMatch)
    {
        _isStartRotationMatchPassed = isStartRotationMatch;
    }

    // Convert the angle values to the desired range
    private Vector3 Angle(Vector3 angle)
    {
        return new Vector3(Converter.ConvertAngle(angle.x), Converter.ConvertAngle(angle.y), Converter.ConvertAngle(angle.z));
    }
}
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
    private Func<float, Quaternion> EulerRotation;
    private Func<Vector3, float> EulerAngle;

    private Quaternion[] _targetRotations = new Quaternion[3];

    private int _targetRotationIndex;

    [Header("Angle Limits")]
    [SerializeField] private float _minAngle = -50; // Minimum angle for the slider
    [SerializeField] private float _maxAngle = 50; // Maximum angle for the slider
    private float _speed = 50; // Rotation speed

    [Header("Joint Index")]
    [SerializeField] private int _jointIndex;

    private bool _isReversing;

    private bool[] _hasTargetRotationsPassed = new bool[3];

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

        if(robotTaskType == RobotTaskType.Run)
        {
            ToggleRun();
        }

        if(robotTaskType == RobotTaskType.SetTargetRotation1 && _isJointSelected)
        {
            _targetRotations[0] = _joint.localRotation;
        }

        if (robotTaskType == RobotTaskType.SetTargetRotation2 && _isJointSelected)
        {
            _targetRotations[1] = _joint.localRotation;
        }

        if (robotTaskType == RobotTaskType.SetTargetRotation3 && _isJointSelected)
        {
            _targetRotations[2] = _joint.localRotation;
        }
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
            //UpdateRotation(Rotation(sliderValue));
            transform.localRotation = EulerRotation(sliderValue);
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
                EulerRotation = delegate (float angle) { return Quaternion.Euler(angle, 0, 0); };
                EulerAngle = delegate (Vector3 eulerAngles) { return eulerAngles.x; };

                break;

            case AngleState.Y:

                GetAngle = delegate (Vector3 rotation) { return rotation.y; };
                IsJointAngleLessThan = delegate (Vector3 a, Vector3 b) { return a.y < b.y; };
                GetRange = delegate (Vector3 a, Vector3 b, Vector3 value) { return Mathf.InverseLerp(a.y, b.y, value.y); };
                Rotation = delegate (float angle) { return new Vector3(0, angle, 0); };
                EulerRotation = delegate (float angle) { return Quaternion.Euler(0, angle, 0); };
                EulerAngle = delegate (Vector3 eulerAngles) { return eulerAngles.y; };

                break;
            case AngleState.Z:

                GetAngle = delegate (Vector3 rotation) { return rotation.z; };
                IsJointAngleLessThan = delegate (Vector3 a, Vector3 b) { return a.z < b.z; };
                GetRange = delegate (Vector3 a, Vector3 b, Vector3 value) { return Mathf.InverseLerp(a.z, b.z, value.z); };
                Rotation = delegate (float angle) { return new Vector3(0, 0, angle); };
                EulerRotation = delegate (float angle) { return Quaternion.Euler(0, 0, angle); };
                EulerAngle = delegate (Vector3 eulerAngles) { return eulerAngles.z; };

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
    private void SetTargetRotation()
    {
        //_targetRotation = _joint.localRotation.eulerAngles;
    }

    // Set the start rotation to the current joint rotation
    private void SetStartRotation()
    {
        //_startRotation = _joint.localRotation.eulerAngles;
    }

    // Check and perform joint rotation
    private IEnumerator JointRotationCoroutine()
    {
        while (_run)
        {
            //CheckAndPerformJointRotation();

            yield return StartCoroutine(Test(_targetRotations[_targetRotationIndex]));
            yield return StartCoroutine(Test(_targetRotations[_targetRotationIndex]));
            yield return StartCoroutine(Test(_targetRotations[_targetRotationIndex], true));

            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }

    private IEnumerator Test(Quaternion targetRotation, bool rotateBack = false)
    {
        while (_joint.localRotation != targetRotation)
        {
            _joint.localRotation = Quaternion.RotateTowards(_joint.localRotation, targetRotation, _speed * Time.fixedDeltaTime);

            print(_targetRotationIndex);

            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }

        if(_targetRotationIndex == _targetRotations.Length - 1)
        {
            _isReversing = true;
        }
        else if(_targetRotationIndex == 0)
        {
            _isReversing = false;
        }

        if (_isReversing)
        {
            _targetRotationIndex--;
        }
        else
        {
            _targetRotationIndex++;
        }

        yield return new WaitForSeconds(1);
    }

    private void CheckAndPerformJointRotation()
    {
        //float angle = EulerAngle(_joint.localRotation.eulerAngles);

        //_joint.localRotation = EulerRotation(angle += _speed * Time.fixedDeltaTime);

        Quaternion nextRotation;

        if (_joint.localRotation != _targetRotations[0])
        {
            if (!_hasTargetRotationsPassed[0])
            {
                nextRotation = _targetRotations[0];
            }
        }
        else
        {
            _hasTargetRotationsPassed[0] = true;
        }

        if (_joint.localRotation != _targetRotations[1])
        {
            if (_hasTargetRotationsPassed[0] && !_hasTargetRotationsPassed[1])
            {
                nextRotation = _targetRotations[1];
            }
        }
        else
        {
            _hasTargetRotationsPassed[1] = true;
        }

        if (_joint.localRotation == _targetRotations[2])
        {
            if (_hasTargetRotationsPassed[1] && !_hasTargetRotationsPassed[2])
            {
                nextRotation = _targetRotations[2];
            }
        }
        else
        {
            _hasTargetRotationsPassed[2] = true;
        }

        //_joint.localRotation = Quaternion.RotateTowards(_joint.localRotation, _targetRotation, 10 * Time.fixedDeltaTime);

        





        //bool isRotationMatchStart = _joint.localRotation.eulerAngles == _startRotation;
        //bool isRotationMatchTarget = _joint.localRotation.eulerAngles == _targetRotation;

        //CalculateRange(out float range);

        //if (!isRotationMatchStart && !_isStartRotationMatchPassed)
        //{
        //    // Run joint rotation towards start rotation
        //    RunJointRotation(Angle(_startRotation), range <= 0.01f, true);
        //}
        //else if (!isRotationMatchTarget && _isStartRotationMatchPassed)
        //{
        //    // Run joint rotation towards target rotation
        //    RunJointRotation(Angle(_targetRotation), range >= 0.99f, false);
        //}
    }

    // Check if the current joint angle is less than the target angle
    private void RunJointRotation(Vector3 targetRotation, bool stopRotation, bool toggleStartRotationMatch)
    {
        //bool isJointAngleLessThan = IsJointAngleLessThan(Angle(_joint.localEulerAngles), Angle(targetRotation));

        //CalculateAngle(isJointAngleLessThan, out float angle);
        //UpdateRotation(Rotation(angle));

        //if (stopRotation)
        //{
        //    // Stop rotation at the target angle and toggle the start rotation match flag
        //    UpdateRotation(targetRotation);
        //    ToggleStartRotationMatch(toggleStartRotationMatch);
        //}
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

    //private void CalculateRange(out float range)
    //{
    //    //range = GetRange(Angle(_startRotation), Angle(_targetRotation), Angle(_joint.eulerAngles));
    //}

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
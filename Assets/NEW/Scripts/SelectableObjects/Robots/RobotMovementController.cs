using System;
using System.Collections.Generic;
using UnityEngine;
using Pautik;

public class RobotMovementController : MonoBehaviour
{
    [Serializable]
    public struct CheckPointData
    {
        public Vector3 Position { get; set; }
        public float WaitTime { get; set; }
    }

    [Header("Transforms")]
    [SerializeField] private Transform _rigTarget; // Reference to the rig target transform
    [SerializeField] private Transform _robot; // Reference to the robot transform

    [Header("Position Limits")]
    [SerializeField] private Vector3 _minRange; // Minimum position range
    [SerializeField] private Vector3 _maxRange; // Maximum position range

    private Vector2 _movementDirection;   
    private Vector3 _movementVelocity;
    private Vector3 _desiredPosition;

    [Header("Checkpoints")]
    [SerializeField] private List<CheckPointData> _checkPoints = new List<CheckPointData>(); // List of checkpoint data

    [Header("Speed")]
    [SerializeField] private float _speed; // Movement speed

    private object[] _data = new object[3];




    private void OnEnable()
    {
        References.Manager.RobotTaskManager.OnRobotTask += OnRobotTask;
    }

    // Event handler for robot tasks
    private void OnRobotTask(RobotTaskType robotTaskType, object[] data)
    {
        UpdateRigTargetPosition(robotTaskType, data);
        UpdateRobotRotation(robotTaskType, data);
        UpdateRigTargetRotation(robotTaskType, data);
    }

    // Update the position of the rig target based on the robot task
    private void UpdateRigTargetPosition(RobotTaskType robotTaskType, object[] data)
    {
        if(robotTaskType != RobotTaskType.Move)
        {
            return;
        }

        // Calculate the desired position within the specified range
        _movementDirection = (Vector2)data[0];
        _movementVelocity = _movementDirection * _speed * Time.fixedDeltaTime;
        _desiredPosition.x = _desiredPosition.x > _maxRange.x ? _maxRange.x : _desiredPosition.x < _minRange.x ? _minRange.x : (_rigTarget.localPosition + _movementVelocity).x;
        _desiredPosition.y = _desiredPosition.y > _maxRange.y ? _maxRange.y : _desiredPosition.y < _minRange.y ? _minRange.y : (_rigTarget.localPosition + _movementVelocity).y;
        _desiredPosition.z = _maxRange.z;

        _rigTarget.localPosition = _desiredPosition;

        ShareRobotPosition(_rigTarget.localPosition);
    }

    // Update the rotation of the robot based on the robot task
    private void UpdateRobotRotation(RobotTaskType robotTaskType, object[] data)
    {
        if(robotTaskType != RobotTaskType.RotateRobot)
        {
            return;
        }

        float direction = (float)data[1];
        float angle = 30f * direction * _speed * Time.fixedDeltaTime;

        RotateTransformAroundAxis(_robot, Vector3.up, angle);
        ShareRotationsAngle(RobotTaskType.ObserveRobotRotation, _robot.rotation.eulerAngles.y);
    }

    // Update the rotation of the rig target (end effector) based on the robot task
    private void UpdateRigTargetRotation(RobotTaskType robotTaskType, object[] data)
    {
        if (robotTaskType != RobotTaskType.RotateEndEffector)
        {
            return;
        }

        float direction = (float)data[1];
        float angle = 15f * direction * _speed * Time.fixedDeltaTime;
        float localEulerAngles = Converter.ConvertAngle(_rigTarget.localRotation.eulerAngles.z);

        // Block rotation if it exceeds the specified range
        bool blockRotation = angle > 0 && localEulerAngles >= 90 || angle < 0 && localEulerAngles < -90;

        if (blockRotation)
        {
            return;
        }

        RotateTransformAroundAxis(_rigTarget, Vector3.forward, angle);
        ShareRotationsAngle(RobotTaskType.ObservreEndEffectorRotation, _rigTarget.rotation.eulerAngles.y);
    }

    // Rotate the specified transform around the specified axis by the given angle
    private void RotateTransformAroundAxis(Transform transform, Vector3 direction, float angle)
    {
        transform.Rotate(direction, angle);
    }

    // Share the current robot position with other components
    private void ShareRobotPosition(Vector2 position)
    {
        _data[0] = position;

        References.Manager.RobotTaskManager.RaiseRobotTaskEvent(RobotTaskType.ObserveRobotPosition, _data);
    }

    // Share the rotation angle with other components based on the robot task
    private void ShareRotationsAngle(RobotTaskType robotTaskType, float angle)
    {
        _data[1] = angle;

        References.Manager.RobotTaskManager.RaiseRobotTaskEvent(robotTaskType, _data);
    }
}
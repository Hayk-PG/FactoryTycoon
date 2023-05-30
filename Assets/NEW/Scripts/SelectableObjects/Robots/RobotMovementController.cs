using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for managing the robot's movement and checkpoints.
/// </summary>
public class RobotMovementController : MonoBehaviour
{
    [Serializable]
    public struct CheckPointData
    {
        public Vector3 Position { get; set; }
        public float WaitTime { get; set; }
    }

    [Header("Transforms")]
    [SerializeField] private Transform _rigTarget;
    [SerializeField] private Transform _robot;

    [Header("Position Limits")]
    [SerializeField] private Vector3 _minRange;
    [SerializeField] private Vector3 _maxRange;
    private Vector2 _movementDirection;   
    private Vector3 _movementVelocity;
    private Vector3 _desiredPosition;

    [Header("Checkpoints")]
    [SerializeField] private List<CheckPointData> _checkPoints = new List<CheckPointData>();

    [Header("Speed")]
    [SerializeField] private float _speed;
    private float _rotationDirection;

    private object[] _data = new object[3];



    private void OnEnable()
    {
        References.Manager.RobotTaskManager.OnRobotTask += OnRobotTask;
    }

    private void OnRobotTask(RobotTaskType robotTaskType, object[] data)
    {
        UpdateRigTargetPosition(robotTaskType, data);
        UpdateRobotRotation(robotTaskType, data);
        UpdateRigTargetRotation(robotTaskType, data);
    }

    private void UpdateRigTargetPosition(RobotTaskType robotTaskType, object[] data)
    {
        if(robotTaskType != RobotTaskType.Move)
        {
            return;
        }

        _movementDirection = (Vector2)data[0];
        _movementVelocity = _movementDirection * _speed * Time.fixedDeltaTime;

        _desiredPosition.x = _desiredPosition.x > _maxRange.x ? _maxRange.x : _desiredPosition.x < _minRange.x ? _minRange.x : (_rigTarget.localPosition + _movementVelocity).x;
        _desiredPosition.y = _desiredPosition.y > _maxRange.y ? _maxRange.y : _desiredPosition.y < _minRange.y ? _minRange.y : (_rigTarget.localPosition + _movementVelocity).y;
        _desiredPosition.z = _maxRange.z;

        _rigTarget.localPosition = _desiredPosition;

        ShareRobotPosition(_rigTarget.localPosition);
    }

    private void UpdateRobotRotation(RobotTaskType robotTaskType, object[] data)
    {
        if(robotTaskType != RobotTaskType.RotateRobot)
        {
            return;
        }

        _rotationDirection = (float)data[1];

        float angle = 30f * _rotationDirection * _speed * Time.fixedDeltaTime;

        _robot.Rotate(Vector3.up, angle);

        ShareRobotRotationAngle(_robot.rotation.eulerAngles.y);
    }

    private void UpdateRigTargetRotation(RobotTaskType robotTaskType, object[] data)
    {
        if (robotTaskType != RobotTaskType.RotateEndEffector)
        {
            return;
        }

        _rotationDirection = (float)data[1];

        float angle = 15f * _rotationDirection * _speed * Time.fixedDeltaTime;

        _rigTarget.Rotate(Vector3.forward, angle);

        ShareEndEffectorRotationAngle(_rigTarget.rotation.eulerAngles.y);
    }

    private void ShareRobotPosition(Vector2 position)
    {
        _data[0] = position;

        References.Manager.RobotTaskManager.RaiseRobotTaskEvent(RobotTaskType.ObserveRobotPosition, _data);
    }

    private void ShareRobotRotationAngle(float angle)
    {
        _data[1] = angle;

        References.Manager.RobotTaskManager.RaiseRobotTaskEvent(RobotTaskType.ObserveRobotRotation, _data);
    }

    private void ShareEndEffectorRotationAngle(float angle)
    {
        _data[1] = angle;

        References.Manager.RobotTaskManager.RaiseRobotTaskEvent(RobotTaskType.ObservreEndEffectorRotation, _data);
    }
}

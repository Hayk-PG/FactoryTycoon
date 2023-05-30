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




    private void OnEnable()
    {
        References.Manager.RobotTaskManager.OnRobotTask += OnRobotTask;
    }

    private void OnRobotTask(RobotTaskType robotTaskType, object[] data)
    {
        UpdateRigTargetPosition(robotTaskType, data);
        UpdateRobotRotation(robotTaskType, data);
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
    }

    private void UpdateRobotRotation(RobotTaskType robotTaskType, object[] data)
    {
        if(robotTaskType != RobotTaskType.Rotate)
        {
            return;
        }

        _rotationDirection = (float)data[1];

        float angle = 30f * _rotationDirection * _speed * Time.fixedDeltaTime;

        _robot.Rotate(Vector3.up, angle);
    }
}

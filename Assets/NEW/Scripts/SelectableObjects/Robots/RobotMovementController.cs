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
    [SerializeField] private Transform _target; // Transform of the target object
    [SerializeField] private Transform _robot; // Transform of the robot object

    [Header("Position Limits")]
    [SerializeField] private float _minHorizontalPosition = -4f; // Minimum horizontal position for the target
    [SerializeField] private float _maxHorizontalPosition = -1f; // Maximum horizontal position for the target
    [SerializeField] private float _minVerticalPosition = -0.8f; // Minimum vertical position for the target
    [SerializeField] private float _maxVerticalPosition = 3f; // Maximum vertical position for the target
    [SerializeField] private float _fixedDepthPosition = -0.29f; // Fixed depth position for the target

    [Header("Checkpoints")]
    [SerializeField] private List<CheckPointData> _checkPoints = new List<CheckPointData>(); // List of checkpoints for the robot

    private object[] _data = new object[2]; // Data array for sending to the RobotTaskManager




    private void Start()
    {
        InitializeMoveScreenTarget();
    }

    private void OnEnable()
    {
        References.Manager.RobotTaskManager.OnRobotTask += OnRobotTask;
    }

    // Initializes the movement of the target on the screen.
    private void InitializeMoveScreenTarget()
    {
        _data[0] = Mathf.InverseLerp(_minHorizontalPosition, _maxHorizontalPosition, _target.localPosition.x);
        _data[1] = Mathf.InverseLerp(_minVerticalPosition, _maxVerticalPosition, _target.localPosition.y);

        References.Manager.RobotTaskManager.RaiseRobotTaskEvent(RobotTaskType.InitializeMoveScreenTarget, _data);
    }

    // Handles the robot task received from the RobotTaskManager.
    private void OnRobotTask(RobotTaskType robotTaskType, object[] data)
    {
        UpdateRobotPositionWithMoveScreen(robotTaskType, data);
    }

    // Updates the robot's position based on the movement of the target on the screen.
    private void UpdateRobotPositionWithMoveScreen(RobotTaskType robotTaskType, object[] data)
    {
        if (robotTaskType == RobotTaskType.Move)
        {
            Vector2 normalizedPosition = (Vector2)data[0];

            float x = Mathf.Lerp(_minHorizontalPosition, _maxHorizontalPosition, normalizedPosition.x);
            float y = Mathf.Lerp(_minVerticalPosition, _maxVerticalPosition, normalizedPosition.y);

            _target.localPosition = new Vector3(x, y, _fixedDepthPosition);
        }
    }
}

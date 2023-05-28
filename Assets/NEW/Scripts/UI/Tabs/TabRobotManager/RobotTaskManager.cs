using System;
using UnityEngine;

/// <summary>
/// Manages robot task events and notifications.
/// </summary>
public class RobotTaskManager : MonoBehaviour
{
    /// <summary>
    /// Event for robot task notifications.
    /// </summary>
    public event Action<RobotTaskType, object[]> OnRobotTask;

    /// <summary>
    /// Raises a robot task event with the specified task type and data.
    /// </summary>
    /// <param name="robotTaskType">The type of robot task.</param>
    /// <param name="data">Optional data associated with the task.</param>
    public void RaiseRobotTaskEvent(RobotTaskType robotTaskType, object[] data = null)
    {
        // Invoke the event if there are subscribers
        OnRobotTask?.Invoke(robotTaskType, data);
    }
}

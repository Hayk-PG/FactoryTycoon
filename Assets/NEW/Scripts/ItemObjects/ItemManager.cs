using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody _rigidbody;

    private bool _isMoved;

    private List<Vector3> _queuedMoves = new List<Vector3>();




    /// <summary>
    /// Moves the item in the specified direction.
    /// </summary>
    /// <param name="direction">The direction to move the item.</param>
    public void MoveItem(Vector3 direction)
    {
        _queuedMoves.Add(direction);

        if (_isMoved)
        {
            return;
        }

        StartCoroutine(PerformMove(direction, 4));
    }

    // Performs the move towards the target position over a specified time.
    private IEnumerator PerformMove(Vector3 direction, float time)
    {
        // Set the move state to true as the move has been started.
        SetMoveState(true);

        // Calculate the initial and target positions
        Vector3 initialPosition = _rigidbody.position;
        Vector3 targetPosition = initialPosition + direction;
        Vector3 lerpedPosition = Vector3.zero;

        // Calculate the initial distance and distance threshold
        float distance = Vector3.Distance(initialPosition, targetPosition);
        float distanceThreshold = 0.05f;

        // Move towards the target position while the distance is above the threshold
        while (distance > distanceThreshold)
        {
            // Calculate the lerped position based on the current time and fixed delta time
            lerpedPosition = Vector3.Lerp(_rigidbody.position, targetPosition, time * Time.fixedDeltaTime);
            // Update the distance based on the new position
            distance = Vector3.Distance(_rigidbody.position, targetPosition);
            MoveToPosition(lerpedPosition);

            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }

        // Finalize the move towards the target position by directly setting the rigidbody's position to the target position.
        _rigidbody.position = targetPosition;
        // Remove the first queued move as it has been processed.
        _queuedMoves.RemoveAt(0);

        // Processes the queued moves and performs the next move if available.
        bool hasQueuedPushes = _queuedMoves.Count > 0;
        
        if (hasQueuedPushes)
        {
            // Process the remaining queued moves by starting the coroutine.
            StartCoroutine(PerformMove(_queuedMoves[0], 2));

            yield break;
        }

        // Set the move state to false as the move has been completed.
        SetMoveState(false);
    }

    // Sets the move state of the item.
    private void SetMoveState(bool isMoved)
    {
        _isMoved = isMoved;
    }

    // Moves the item to the specified position using the rigidbody's MovePosition method.
    private void MoveToPosition(Vector3 position)
    {
        _rigidbody.MovePosition(position);
    }
}
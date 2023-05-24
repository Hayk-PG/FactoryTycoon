using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pautik;

public class ItemManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody _rigidbody;

    // Represents the data for a movement action.
    public struct MoveData
    {
        public Vector3 Direction { get; set; } // The direction of the movement.
        public ConveyorSegment Conveyor { get; set; } // The target conveyor segment.
    }

    private bool _isMoved; // Flag indicating if the item is currently being moved.
    private List<MoveData> _queuedMoves = new List<MoveData>(); // Queue of queued movement data.




    /// <summary>
    /// Moves the item in the specified direction.
    /// </summary>
    /// <param name="direction">The direction to move the item.</param>
    public void MoveItem(Vector3 direction, ConveyorSegment conveyorSegment)
    {
        _queuedMoves.Add(new MoveData { Direction = direction, Conveyor = conveyorSegment });

        if (_isMoved)
        {
            return;
        }

        StartCoroutine(PerformMove(direction, conveyorSegment, 4));
    }

    // Performs the move towards the target position over a specified time.
    private IEnumerator PerformMove(Vector3 direction, ConveyorSegment conveyorSegment, float time)
    {
        // Set the move state to true as the move has been started.
        SetMoveState(true);

        // Calculate the initial and target positions
        Vector3 initialPosition = _rigidbody.position;
        Vector3 targetPosition = initialPosition + direction;
        Vector3 lerpedPosition = Vector3.zero;
        Vector3 targetConveyorPosition = conveyorSegment.transform.position + conveyorSegment.Direction;

        // Calculate the initial distance and distance threshold
        float distance = Vector3.Distance(initialPosition, targetPosition);
        float distanceThreshold = 0.05f;
        bool canMove = false;

        // Check if the target conveyor position exists in the dictionary
        Checker.IsValueInDictionary(References.Manager.ConveyorCollection.Dict, targetConveyorPosition, out ConveyorSegment targetConveyorSegment);

        // Move towards the target position while the distance is above the threshold
        while (distance > distanceThreshold)
        {
            if (targetConveyorSegment != null && !targetConveyorSegment.ConveyorTrigger.HasTriggeredItem || canMove)
            {
                canMove = true;
                // Calculate the lerped position based on the current time and fixed delta time
                lerpedPosition = Vector3.Lerp(_rigidbody.position, targetPosition, time * Time.fixedDeltaTime);
                // Update the distance based on the new position
                distance = Vector3.Distance(_rigidbody.position, targetPosition);
                MoveToPosition(lerpedPosition);
            }

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
            StartCoroutine(PerformMove(_queuedMoves[0].Direction, _queuedMoves[0].Conveyor, 2));

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
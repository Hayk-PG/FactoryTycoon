using UnityEngine;
using Pautik;

public class ConveyorTrigger : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ConveyorSegment _conveyorSegment;

    private ItemManager _triggeredItem;




    // Handles the logic when a collider enters the trigger.
    private void OnTriggerEnter(Collider other)
    {
        AssignTriggeredItem(Get<ItemManager>.From(other.gameObject));

        bool isTriggeredItemNull = _triggeredItem == null;

        if (isTriggeredItemNull)
        {
            return;
        }

        MoveAssignedItem();
    }

    // Handles the logic when a collider exits the trigger.
    private void OnTriggerExit(Collider other)
    {
        bool isTriggeredItemNotEqual = _triggeredItem != Get<ItemManager>.From(other.gameObject);

        if (isTriggeredItemNotEqual)
        {
            return;
        }

        AssignTriggeredItem(null);
    }

    // Assigns the triggered item.
    private void AssignTriggeredItem(ItemManager itemManager)
    {
        _triggeredItem = itemManager;
    }

    // Moves the assigned item using the conveyor segment direction.
    private void MoveAssignedItem()
    {
        _triggeredItem.MoveItem(_conveyorSegment.Direction);
    }
}

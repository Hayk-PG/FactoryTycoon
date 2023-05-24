using System.Collections;
using UnityEngine;
using Pautik;

public class InputConveyor : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ConveyorSegment _conveyorSegment;
    [SerializeField] private ItemManager _itemPrefab; // Item prefab




    private void Start()
    {
        StartCoroutine(SpawnItems());
    }

    // Spawns items on the conveyor over time
    private IEnumerator SpawnItems()
    {
        while (_conveyorSegment.IsInputSection)
        {
            yield return new WaitForSeconds(1f);

            // Calculate the target position for the conveyor segment
            Vector3 targetPosition = transform.position + _conveyorSegment.Direction;
            Vector3 itemInitialPosition = new Vector3(0f, 1.25f, 0f);

            // Check if the target conveyor position exists in the dictionary
            Checker.IsValueInDictionary(References.Manager.ConveyorCollection.Dict, targetPosition, out ConveyorSegment conveyorSegment);

            // Determine if the conveyor segment is available for moving an item
            bool canMoveItem = conveyorSegment == null || !conveyorSegment.ConveyorTrigger.HasTriggeredItem;

            // Instantiate the item if the conveyor can move the item
            if (canMoveItem)
            {
                Instantiate(_itemPrefab, transform.position + itemInitialPosition, Quaternion.identity);
            }
        }
    }
}

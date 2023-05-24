using System.Collections;
using UnityEngine;

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
            yield return new WaitForSeconds(2f);

            // Instantiate a new item prefab at the specified position and rotation
            Instantiate(_itemPrefab, transform.position + new Vector3(0f, 1.25f, 0f), Quaternion.identity);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputConveyor : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ConveyorSegment _conveyorSegment;
    [SerializeField] private ItemManager _item;




    private void Start()
    {
        StartCoroutine(SpawnItem());
    }

    private IEnumerator SpawnItem()
    {
        while (_conveyorSegment.IsInputSection)
        {
            yield return new WaitForSeconds(2);
            print("a");

            ItemManager itemManager = Instantiate(_item, transform.position + new Vector3(0f, 1.25f, 0f), Quaternion.identity);
        }
    }
}

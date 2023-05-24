using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pautik;

public class ConveyorTrigger : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ConveyorSegment _conveyorSegment;

    private ItemManager _triggeredItem;




    private void OnTriggerEnter(Collider other)
    {
        _triggeredItem = Get<ItemManager>.From(other.gameObject);

        if(_triggeredItem == null)
        {
            return;
        }

        //_triggeredItem.PushItem(_conveyorSegment.Direction);
    }

    private void OnTriggerExit(Collider other)
    {
        if(_triggeredItem != Get<ItemManager>.From(other.gameObject))
        {
            return;
        }

        //_triggeredItem = null;
    }
}

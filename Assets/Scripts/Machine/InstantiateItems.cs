using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateItems : MonoBehaviour
{
    [Header("ITEM")]
    [SerializeField] Transform[] items;
    [SerializeField] int spawnItemIndex;

    Transform Item { get { return items[spawnItemIndex]; } }

    [Header("INSTANTIATE")]
    [SerializeField] BoxCollider triggerCol;
    [SerializeField] Transform parent;
    Vector3 InstPoint { get { return new Vector3(triggerCol.bounds.center.x, triggerCol.bounds.min.y + Item.GetComponent<Collider>().bounds.extents.y, triggerCol.bounds.center.z); } }
    bool CanInstantiate { get; set; }


   
    void Start() {

        Invoke("InstantiateOnce", 1);

        StartCoroutine(SpawnItems());
    }   

    void OnTriggerStay(Collider other) {

        if(other.GetComponent<IMovement>() != null) CanInstantiate = false;
    }

    void OnTriggerExit(Collider other) {

        if (other.GetComponent<IMovement>() != null) CanInstantiate = true;
    }

    IEnumerator SpawnItems() {

        yield return new WaitForSeconds(2);

        yield return new WaitUntil(() => CanInstantiate);

        GameObject item = Item != null ? Instantiate(Item.gameObject, InstPoint, Quaternion.identity, parent) : null;

        yield return StartCoroutine(SpawnItems());
    }

    void InstantiateOnce() {

        GameObject item = Item != null ? Instantiate(Item.gameObject, InstPoint, Quaternion.identity, parent) : null;
    }





}

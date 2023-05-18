using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxColliderPosition : MonoBehaviour
{
    [SerializeField] BoxCollider[] colliders;
    [SerializeField] float[] posDifferences;
    [SerializeField] Transform movingParts;
    
   
    void Awake() {

        posDifferences = new float[colliders.Length];

        for (int i = 0; i < posDifferences.Length; i++) {
            posDifferences[i] = colliders[i].center.y - movingParts.localPosition.y;
        }
    }


    void Update() {

        SetTriggersPos();
    }

    void SetTriggersPos() {

        for (int c = 0; c < colliders.Length; c++) {
            float y = movingParts.transform.localPosition.y + posDifferences[c];
            colliders[c].center = new Vector3(colliders[c].center.x, y, colliders[c].center.z);
        }
    }








}

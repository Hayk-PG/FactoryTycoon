using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceTransform : MonoBehaviour
{
    [SerializeField] Vector3 worldSpaceRotation;

    void Update() {

        transform.eulerAngles = worldSpaceRotation;
    }








}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonChangableValues : MonoBehaviour
{
    [SerializeField] GameObject groundTiles;
    //GroundTile gameobject's name should always be "GroundTIles"
    public string GroundTiles {

        get {
            return groundTiles.name;
        }
        set {
            groundTiles.name = value;
        }
    }


    void Awake() {

        GroundTiles = "GroundTiles";
    }






}

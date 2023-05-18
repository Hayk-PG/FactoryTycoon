using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotCheckSides : MainSensorsBehaviour
{
    protected override void Awake() {

        SpawnUI = ObjectsHolder.instance.OnGroundTilesSpawnUI;

        sides = new Vector3[1];
    }

    protected override void Update() {

        SidesInitialization();
        BoolsInitialization();

        if (Down) {

            base.OnDown();
        }
    }

    protected override void SidesInitialization() {

        sides[0] = -transform.up;
    }

    protected override void BoolsInitialization() {

        down = Physics.Raycast(transform.position, sides[0], out downHit, 0.3f, ObjectsHolder.instance.LayerMasks.TileMask);
    }














}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine1 : Machine
{
    [Header("COLLIDER")]
    [SerializeField] Collider triggerCollider;
    [SerializeField] Collider bottomTriggerCollider;
    
    List<Collider> conveyors = new List<Collider>();
    List<Collider> _tiles = new List<Collider>();

    [SerializeField] int _tilesRequiredCount;
    [SerializeField] bool showInfo;

    /// <summary>
    /// Can this object get spawned (Machine)
    /// </summary>
    public override bool CanThisGameObjectGetSpawned { get; set; }
    

    protected override void Awake() {

        base.Awake();
    }

    protected override void Update() {

        if (isSpawnUIOpen)
            MainBoxCast();
        else
            return;
    }

    protected virtual void MainBoxCast() {

        Collider[] overlapConveyerColliders = Physics.OverlapBox(triggerCollider.bounds.center, triggerCollider.bounds.extents, transform.rotation, ObjectsHolder.instance.LayerMasks.ConveyorMask);
        Collider[] overlapTileColliders = Physics.OverlapBox(bottomTriggerCollider.bounds.center, bottomTriggerCollider.bounds.extents / 2, transform.rotation, ObjectsHolder.instance.LayerMasks.TileMask);
        Collider[] overlapOtherConveyorColliders = Physics.OverlapBox(Col.bounds.center, Col.bounds.extents / 2, transform.rotation, ObjectsHolder.instance.LayerMasks.ConveyorMask);

        CanThisGameObjectGetSpawned = conveyors.Count == overlapConveyerColliders.Length / 2 && _tiles.Count == overlapTileColliders.Length && _tiles.Count == _tilesRequiredCount && overlapOtherConveyorColliders.Length < 1 ? true : false;

        OverlapConveyerColliders(overlapConveyerColliders);
        OverlapTileColliders(overlapTileColliders);    

        if (CanThisGameObjectGetSpawned && !GotSpawned) {

            //OnHover();
        }
        if (!CanThisGameObjectGetSpawned) {

            OnHoverOccupied();
        }
        if (GotSpawned) {

            base.OnUnhover();           
        }

        if (showInfo)
        {
            print("conveyors.Count == " + conveyors.Count + " overlapConveyerColliders.Length: " + overlapConveyerColliders.Length + " _tiles.Count: " + _tiles.Count + " overlapTileColliders.Length: " + overlapTileColliders.Length);
        }
    }

    void OverlapConveyerColliders(Collider[] overlapConveyerColliders) {

        // Convert overlapConveyerColliders array to list
        // Check if detected conveyors don't have any other gameobjects on them
        // Check if no longer detected conveyors still in the list ,if so, remove them
       
        List<Collider> overlapConveyorsList = new List<Collider>(overlapConveyerColliders);

        foreach (var c in overlapConveyorsList) {

            if (!conveyors.Contains(c)) {
                
                if (c.GetComponent<Conveyor>().GameObjectOnTopOfTheConveyor == null) {
                    c.GetComponent<Conveyor>().GameObjectOnTopOfTheConveyor = this;
                    conveyors.Add(c);
                }              
            }            
        }

        for (int c = 0; c < conveyors.Count; c++) {

            if (!overlapConveyorsList.Contains(conveyors[c])) {

                if (conveyors[c].GetComponent<Conveyor>().GameObjectOnTopOfTheConveyor == this) {
                    conveyors[c].GetComponent<Conveyor>().GameObjectOnTopOfTheConveyor = null;
                }

                conveyors.Remove(conveyors[c]);
            }
        }     
    }
    
    void OverlapTileColliders(Collider[] overlapTileColliders) {

        // Convert overlapTileColliders array to list
        // Check if detected tiles don't have any other gameobjects on them
        // Check if no longer detected tiles still in the list ,if so, remove them

        List<Collider> overlapTilesList = new List<Collider>(overlapTileColliders);

        foreach (var t in overlapTilesList) {

            if (!_tiles.Contains(t)) {

                if (t.GetComponent<GroundTile>().GameObjectOnTopOfTheTile == null) {
                    t.GetComponent<GroundTile>().GameObjectOnTopOfTheTile = gameObject;
                    _tiles.Add(t);
                }
            }            
        }

        for (int t = 0; t < _tiles.Count; t++) {

            if (!overlapTilesList.Contains(_tiles[t])){

                if(_tiles[t].GetComponent<GroundTile>().GameObjectOnTopOfTheTile == gameObject) {
                    _tiles[t].GetComponent<GroundTile>().GameObjectOnTopOfTheTile = null;
                }

                _tiles.Remove(_tiles[t]);
            }
        }
    }




























}

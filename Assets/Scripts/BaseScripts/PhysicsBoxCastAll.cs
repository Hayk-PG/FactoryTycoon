using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsBoxCastAll : MonoBehaviour
{
    [Header("BASE CONTROLLER BOX CAST")]
    [SerializeField] int fixedTilesCount;
    int hitTilesCount;
    RaycastHit[] hits = new RaycastHit[] { };
    List<GroundTile> tiles = new List<GroundTile>();


    protected void MainBoxCast(Collider Col) {

        hits = Physics.BoxCastAll(Col.bounds.center, Col.bounds.extents / 2, Vector3.down, Quaternion.identity, 0.5f, ObjectsHolder.instance.LayerMasks.TileMask);
        hitTilesCount = hits.Length;

        if (hitTilesCount == fixedTilesCount) {

            foreach (RaycastHit hit in hits) {

                if (hit.collider.GetComponent<GroundTile>().GameObjectOnTopOfTheTile == null) {

                    hit.collider.GetComponent<GroundTile>().GameObjectOnTopOfTheTile = gameObject;
                    TilesClear(tiles);
                }
                if (hit.collider.GetComponent<GroundTile>().GameObjectOnTopOfTheTile == gameObject) {

                    TilesAdd(tiles, hit.collider.GetComponent<GroundTile>());
                }
                else if (hit.collider.GetComponent<GroundTile>().GameObjectOnTopOfTheTile != null && hit.collider.GetComponent<GroundTile>().GameObjectOnTopOfTheTile != gameObject) {

                    foreach (RaycastHit occupiedTiles in hits) {
                        occupiedTiles.collider.GetComponent<GroundTile>().OnHoverOccupied();
                    }
                    TilesClear(tiles);
                }
            }
        }
        else {

            foreach (RaycastHit hit in hits) {

                hit.collider.GetComponent<GroundTile>().OnHoverOccupied();
            }
            TilesClear(tiles);
        }

        if (tiles.Count == fixedTilesCount) {

            foreach (GroundTile t in tiles) {
                t.OnHover();
            }
        }
        else {

            foreach (GroundTile t in tiles) {
                t.OnHoverOccupied();
            }
        }
    }
   
    protected void OnActivateThisGameObject(bool isActive) {

        if (isActive) {

            GroundTilesOwnershipByThisGameObject();
        }
    }

    void GroundTilesOwnershipByThisGameObject() {

        foreach (RaycastHit hit in hits) {

            if (hit.collider.GetComponent<GroundTile>().GameObjectOnTopOfTheTile != gameObject && !hit.collider.GetComponent<GroundTile>().GameObjectOnTopOfTheTile.GetComponent<MainBehaviour>().GotSpawned)
                hit.collider.GetComponent<GroundTile>().GameObjectOnTopOfTheTile = hit.collider.GetComponent<GroundTile>().GameObjectOnTopOfTheTile != gameObject ? gameObject :
                    hit.collider.GetComponent<GroundTile>().GameObjectOnTopOfTheTile;
        }
    }

    void TilesClear(List<GroundTile> tiles) {

        if(tiles != null) {

            tiles.Clear();
        }
    }

    void TilesAdd(List<GroundTile> tiles, GroundTile hitCollider) {

        if (!tiles.Contains(hitCollider))
            tiles.Add(hitCollider);
    }











}

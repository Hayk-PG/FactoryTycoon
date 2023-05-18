using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateGroundTiles : MonoBehaviour
{
    public static InstantiateGroundTiles instance;

    [Header("COLLIDER")]
    [SerializeField] Collider groundCol;

    [Header("PREFAB")]
    [SerializeField] GroundTile[] groundTilePrefabs;

    [Header("TRANSFORM")]
    [SerializeField] Transform tilesParent;
    [SerializeField] Transform tileStartPoint;

    [Header("NUMBERS")]
    [SerializeField] float tilesCount;
    [SerializeField] int tilesIndex;

    float TilesCount => tilesCount > 0 ? tilesCount / 2 : 0;
    Vector3 TileStartPoint => tileStartPoint != null ? tileStartPoint.position : Vector3.zero;


    void Awake() {

        instance = this;
    }

    void Start() {

        //CreateTiles(null, null);
    }

    /// <summary>
    /// Create TilesContainer and GroundTiles as childs of TilesContainer
    /// </summary>
    public void CreateTiles(Transform parent, GameObject _tilesContainer) {

        GameObject tilesContainer = _tilesContainer != null ? _tilesContainer : Instantiate(ObjectsHolder.instance.SavableGameObjects.Find(item => item.GetComponent<TilesContainer>()), FindObjectOfType<InstantiateGroundTiles>().transform);
        tilesContainer.name = ObjectsHolder.instance.SavableGameObjects.Find(item => tilesContainer).name;

        GroundTile tileReference = Instantiate(groundTilePrefabs[0], new Vector3(1000, 1000, 1000), Quaternion.identity, parent != null ? parent: tilesContainer.transform);

        for (float x = 0; x < TilesCount; x += tileReference.ColX) {

            for (float z = 0; z < TilesCount; z += tileReference.ColZ) {

                GroundTile tile = Instantiate(groundTilePrefabs[tilesIndex]);

                float posX = x == 0 ? TileStartPoint.x - tileReference.ColX : x + TileStartPoint.x - tileReference.ColX;
                float posZ = z == 0 ? TileStartPoint.z - tileReference.ColZ : z + TileStartPoint.z - tileReference.ColZ;
                float posY = groundCol.bounds.max.y;

                tile.SetTransform(posX, posY, posZ, parent != null ? parent : tilesContainer.transform);
            }
        }

        Destroy(tileReference.gameObject);
    }
    





}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TilesContainer : MonoBehaviour
{
    OnGroundTilesSpawnUI tilesSpawnUI;
    SaveLoadGameobjects saveLoadGameobjects;

    [Header("Tiles")]
    [SerializeField] List<GroundTile> childTiles = new List<GroundTile>();

    List<float> horizontal = new List<float>();
    List<float> vertical = new List<float>();
    GroundTile[] sideTiles = new GroundTile[4];

    bool isSpawnUiOpen;


    void Awake() {

        tilesSpawnUI = ObjectsHolder.instance.OnGroundTilesSpawnUI;
        saveLoadGameobjects = ObjectsHolder.instance.SaveLoadGameobjects;
    }

    void OnEnable() {

        tilesSpawnUI.OnSpawnUiActivity += TilesSpawnUI_OnSpawnUiActivity;
        saveLoadGameobjects.OnCreateTilesInLoadedTilesContainer += SaveLoadGameobjects_OnCreateTilesInLoadedTilesContainer;
    }
   
    void OnDisable() {

        tilesSpawnUI.OnSpawnUiActivity -= TilesSpawnUI_OnSpawnUiActivity;
        saveLoadGameobjects.OnCreateTilesInLoadedTilesContainer -= SaveLoadGameobjects_OnCreateTilesInLoadedTilesContainer;
    }

    void Update() {

        if (isSpawnUiOpen) {

            GetChild();

            GetSideTiles(childTiles != null && horizontal != null && vertical != null);

            LoopSideTiles(sideTiles != null);
        }
    }

    void GetChild() {

        for (int c = 0; c < transform.childCount; c++) {

            AddToList(childTiles.Contains(transform.GetChild(c).GetComponent<GroundTile>()), () => {

                childTiles.Add(transform.GetChild(c).GetComponent<GroundTile>());
            });

            AddToList(horizontal.Contains(transform.GetChild(c).position.x), () => {

                horizontal.Add(transform.GetChild(c).position.x);
            });

            AddToList(vertical.Contains(transform.GetChild(c).position.z), () => {

                vertical.Add(transform.GetChild(c).position.z);
            });
        }
    }

    void AddToList(bool contains, System.Action Add) {

        if (!contains) {

            Add();
        }
    }

    void GetSideTiles(bool loop) {

        if (loop) {

            horizontal.Sort();
            vertical.Sort();

            //Left tile
            //Right tile
            //Back tile
            //Front tile

            foreach (var tile in childTiles) {

                if(tile.transform.position.x == horizontal[0] && tile.transform.position.z == vertical[vertical.Count / 2]) {

                    if (sideTiles[0] != tile)
                        sideTiles[0] = tile;
                }

                if (tile.transform.position.x == horizontal[horizontal.Count - 1] && tile.transform.position.z == vertical[vertical.Count / 2]) {

                    if (sideTiles[1] != tile)
                        sideTiles[1] = tile;
                }

                if (tile.transform.position.x == horizontal[horizontal.Count / 2] && tile.transform.position.z == vertical[0]) {

                    if (sideTiles[2] != tile)
                        sideTiles[2] = tile;
                }

                if (tile.transform.position.x == horizontal[horizontal.Count / 2] && tile.transform.position.z == vertical[vertical.Count -1]) {

                    if (sideTiles[3] != tile)
                        sideTiles[3] = tile;
                }
            }
        }
    }

    void LoopSideTiles(bool sideTilesComplete) {

        if (sideTilesComplete) {

            for (int s = 0; s < sideTiles.Length; s++) {

                int index = s;

                Vector3 pos = s == 0 ? new Vector3(-2, 0, 0) : s == 1 ? new Vector3(2, 0, 0) : s == 2 ? new Vector3(0, 0, -2) : new Vector3(0, 0, 2);
                Quaternion rot = s == 0 ? Quaternion.Euler(0, -90, 0) : s == 1 ? Quaternion.Euler(0, 90, 0) : s == 2 ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
                string arrowName = s == 0 ? "Left" : s == 1 ? "Right" : s == 2 ? "Back" : "Front";

                CreateArrowCanvasInTheSideTiles(sideTiles[index].transform.childCount < 1, !(bool)sideTiles[index].GetComponent<ISideTile>()?.ArrowButtonIsDestroyed, index, pos, rot, arrowName);
                
                sideTiles[index].GetComponent<ISideTile>().OnClickArrowCanvasButton(sideTiles[index].ArrowCanvas != null, ()=> {

                    OnClickArrowButton(index);
                });
            }
        }
    }

    void CreateArrowCanvasInTheSideTiles(bool dontHaveChild, bool canCreateArrowButton, int index, Vector3 pos, Quaternion rot, string arrowName) {

        if (dontHaveChild) {

            if (canCreateArrowButton) {

                sideTiles[index].GetComponent<ISideTile>()?.CreateArrowCanvas(pos, rot, arrowName, sideTiles[index].transform);
            }
        }
    }

    void OnClickArrowButton(int index) {

        GameObject tilesContainer = Instantiate(ObjectsHolder.instance.SavableGameObjects.Find(item => item.GetComponent<TilesContainer>()), FindObjectOfType<InstantiateGroundTiles>().transform);

        InstantiateGroundTiles.instance.CreateTiles(tilesContainer.transform, tilesContainer);

        switch (sideTiles[index].ArrowCanvas.name) {

            case "Left": tilesContainer.transform.localPosition = transform.localPosition - new Vector3(5, 0, 0); break;
            case "Right": tilesContainer.transform.localPosition = transform.localPosition + new Vector3(5, 0, 0); break;
            case "Back": tilesContainer.transform.localPosition = transform.localPosition - new Vector3(0, 0, 5); break;
            case "Front": tilesContainer.transform.localPosition = transform.localPosition + new Vector3(0, 0, 5); break;
        }
    }
   
    void TilesSpawnUI_OnSpawnUiActivity(bool isOpened) {

        isSpawnUiOpen = isOpened;
    }

    /// <summary>
    /// Populate tiles in the loaded tilesContainer
    /// </summary>
    /// <param name="obj"></param>
    void SaveLoadGameobjects_OnCreateTilesInLoadedTilesContainer(TilesContainer obj) {

        if(obj == this) {

            InstantiateGroundTiles.instance.CreateTiles(transform, gameObject);
        }        
    }










}

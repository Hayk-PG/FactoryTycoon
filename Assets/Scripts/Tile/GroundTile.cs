using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroundTile : MainBehaviour,ISideTile {
    
    public override float ColX { get => Col.bounds.size.x; set { } }
    public override float ColY { get => Col.bounds.extents.y; set { } }
    public override float ColZ { get => Col.bounds.size.z; set { } }

    [Header("GROUND TILE")]
    public GameObject GameObjectOnTopOfTheTile;

    /// <summary>
    /// Tile arrow canvas from ISideTile interface
    /// </summary>
    public Canvas ArrowCanvas { get; set; }

    /// <summary>
    /// Checks if arrow button overlaps with tile, and gets destroyed ISideTile Interface
    /// </summary>
    public bool ArrowButtonIsDestroyed { get; set; }

    new void Update() {

        //UNDONE changed extents / 4 to extents / 2

        if (isSpawnUIOpen) {

            CheckIfGameObjectOnTopOfTileExists(!Physics.BoxCast(Col.bounds.center, Col.bounds.extents / /*4*/ 2, Vector3.up, out RaycastHit hit, Quaternion.identity, 0.5f));

            ArrowCanvasActivity(true);
        }
        else {

            ArrowCanvasActivity(false);

            return;
        }
    }

    void CheckIfGameObjectOnTopOfTileExists(bool boxCastReturnsNull) {

        if (boxCastReturnsNull) {

            GameObjectOnTopOfTheTile = null;

            OnUnhover();
        }
    }

    public override void OnHover() {

        base.OnHover();

        if(GameObjectOnTopOfTheTile != null) {

            CanSpawnTheObject = !GameObjectOnTopOfTheTile.GetComponent<MainBehaviour>().GotSpawned ? true : false;
        }       
    }

    public override void OnUnhover() {

        base.OnUnhover();
        CanSpawnTheObject = false;
    }

    public override void OnHoverOccupied() {

        base.OnHoverOccupied();
        CanSpawnTheObject = false;
    }

    /*ISideTile*/

    /// <summary>
    /// Create arrow canvas interface
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="rot"></param>
    /// <param name="arrowName"></param>
    /// <param name="parent"></param>
    public void CreateArrowCanvas(Vector3 pos, Quaternion rot, string arrowName, Transform parent) {

        Canvas arrowCanvas = Instantiate(ObjectsHolder.instance.ArrowWorldButtonPrefab, parent);
        arrowCanvas.GetComponent<ArrowCanvasOverlapBox>().TilesContainer = this;
        ArrowCanvas = arrowCanvas;
        arrowCanvas.name = arrowName;
        arrowCanvas.transform.localPosition = pos;
        arrowCanvas.transform.localRotation = rot;
    }

    /// <summary>
    /// On click side tiles arrow button ISideTile interface 
    /// </summary>
    /// <param name="arrowCanvasExists"></param>
    /// <param name="OnClickButton"></param>
    public void OnClickArrowCanvasButton(bool arrowCanvasExists, Action OnClickButton) {

        if (arrowCanvasExists) {

            ArrowCanvas.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
            ArrowCanvas.GetComponentInChildren<Button>().onClick.AddListener(() => OnClickButton());
        }
    }

    /// <summary>
    /// Controls arrow canvas activity
    /// </summary>
    /// <param name="isActive"></param>
    public void ArrowCanvasActivity(bool isActive) {
        
        if(ArrowCanvas != null) {
            if(ArrowCanvas.gameObject.activeInHierarchy != isActive) {
                ArrowCanvas.gameObject.SetActive(isActive);
            }          
        }
    }

    /*ISideTile*/













}

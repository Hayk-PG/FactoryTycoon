using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSensorsBehaviour : MonoBehaviour
{
    protected OnGroundTilesSpawnUI SpawnUI;
    bool isSpawnUIOpen = true;

    protected Vector3[] sides;

    [Header("CONVEYORS PARTS")]
    [SerializeField] protected MeshRenderer legMeshRend;
    [SerializeField] protected GameObject arrowSprite;

    [Header("ROTATION")]
    [SerializeField] bool autoRotation;

    /// <summary>
    /// If true,conveyor rotates automatically to match the next conveyor
    /// </summary>
    public bool AutoRotation => autoRotation;

    public RaycastHit leftHit;
    public RaycastHit rightHit;
    public RaycastHit backHit;
    public RaycastHit frontHit;
    public RaycastHit downHit;

    protected bool front, back, left, right, down, isConveyorInMiddle;
    public bool Front => front;
    public bool Back => back;
    public bool Left => left;
    public bool Right => right;
    public bool Down => down;
    public bool IsInMiddle => isConveyorInMiddle;




    protected virtual void Awake() {

        SpawnUI = ObjectsHolder.instance.OnGroundTilesSpawnUI;

        sides = new Vector3[5];
    }

    protected virtual void OnEnable() {

        SpawnUI.OnSpawnUiActivity += SpawnUI_OnSpawnUiActivity;
    }
   
    protected virtual void OnDisable() {

        SpawnUI.OnSpawnUiActivity -= SpawnUI_OnSpawnUiActivity;
    }

    protected virtual void Update() {

        /*Is Spawn UI open
         *Ray's direction
         *Raycasts
         *Check if hide coveyor's leg 
         *Auto or manual rotation(Manual rotation is getting controled by subscribing SpawnOnTileController to Spawn UI arrow buttons
         */

        if (isSpawnUIOpen) {

            SidesInitialization();
            BoolsInitialization();

            if (IsInMiddle) {

                if (legMeshRend.enabled) {

                    PeripheralsActivity(false);
                }
            }
            else {

                if (!legMeshRend.enabled) {

                    PeripheralsActivity(true);
                }

                if (!arrowSprite.activeInHierarchy) {
                    arrowSprite.SetActive(true);
                }
            }

            if (AutoRotation) {

                if (Down) {

                    OnDown();
                }

                if (Right) {

                    OnRIght();
                }

                if (Left) {

                    OnLeft();
                }

                if (Back) {

                    OnBack();
                }

                if (Front) {

                    OnFront();
                }
            }
        }
        else {

            if (arrowSprite.activeInHierarchy) {
                arrowSprite.SetActive(false);
            }

            return;
        }
    }

    protected virtual void SidesInitialization() {

        sides[0] = transform.forward;
        sides[1] = -transform.forward;
        sides[2] = transform.right;
        sides[3] = -transform.right;
        sides[4] = -transform.up;
    }

    protected virtual void BoolsInitialization() {

        front = Physics.Raycast(transform.position, sides[0], out frontHit, 0.5f, ObjectsHolder.instance.LayerMasks.ConveyorMask);
        back = Physics.Raycast(transform.position, sides[1], out backHit, 0.5f, ObjectsHolder.instance.LayerMasks.ConveyorMask);
        right = Physics.Raycast(transform.position, sides[2], out rightHit, 0.5f, ObjectsHolder.instance.LayerMasks.ConveyorMask);
        left = Physics.Raycast(transform.position, sides[3], out leftHit, 0.5f, ObjectsHolder.instance.LayerMasks.ConveyorMask);
        down = Physics.Raycast(transform.position, sides[4], out downHit, 1f, ObjectsHolder.instance.LayerMasks.TileMask);
        isConveyorInMiddle = Physics.Raycast(transform.position, sides[0], 0.5f, ObjectsHolder.instance.LayerMasks.ConveyorMask) &&
            Physics.Raycast(transform.position, sides[1], 0.5f, ObjectsHolder.instance.LayerMasks.ConveyorMask);
    }

    protected virtual void PeripheralsActivity(bool isActive) {

        legMeshRend.enabled = isActive;
        arrowSprite.SetActive(isActive);
    }

    protected virtual void OnDown() {

        //if (downHit.collider.GetComponent<IRaySelect>()?.IsOccupied() == false) {

        //    downHit.collider.GetComponent<GroundTile>().GameObjectOnTopOfTheTile = gameObject;
        //}
    }

    protected virtual void OnRIght() {

        if (!Front && Back) {

            if (rightHit.transform.eulerAngles.y != transform.eulerAngles.y + 90) {

                rightHit.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 90, 0);
            }
        }

        if (!Front && !Back) {

            if (transform.eulerAngles.y != rightHit.transform.eulerAngles.y - 90) {

                transform.eulerAngles = new Vector3(0, rightHit.transform.eulerAngles.y - 90, 0);
            }
        }
    }

    protected virtual void OnLeft() {

        if (!Front && Back) {

            if (leftHit.collider.GetComponent<ConveyorCheckSides>().Front && leftHit.collider.GetComponent<ConveyorCheckSides>().frontHit.collider.gameObject == gameObject) {

                if (leftHit.transform.eulerAngles.y != transform.eulerAngles.y - 90) {

                    leftHit.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y - 90, 0);
                }
            }

        }

        if (!Front && !Back) {

            if (transform.eulerAngles.y != leftHit.transform.eulerAngles.y - 90) {

                transform.eulerAngles = new Vector3(0, leftHit.transform.eulerAngles.y - 90, 0);
            }
        }
    }

    protected virtual void OnBack() {

        if (backHit.transform.eulerAngles.y != transform.eulerAngles.y) {

            backHit.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }

        if (Front) {

            if (!frontHit.collider.GetComponent<ConveyorCheckSides>().Back) {

                if (frontHit.transform.eulerAngles.y != transform.eulerAngles.y) {

                    frontHit.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                }
            }
        }
    }

    protected virtual void OnFront() {

        if (frontHit.collider.GetComponent<ConveyorCheckSides>().Left && frontHit.collider.GetComponent<ConveyorCheckSides>().leftHit.collider.gameObject == gameObject) {

            if (frontHit.transform.eulerAngles.y != transform.eulerAngles.y) {

                frontHit.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            }
        }
    }

    private void SpawnUI_OnSpawnUiActivity(bool isOpened) {

        isSpawnUIOpen = isOpened;
    }










}

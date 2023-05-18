using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinkEachOther : MonoBehaviour
{
    OnGroundTilesSpawnUI onGroundTilesSpawnUI;

    [Header("LINK")]
    public MainBehaviour ownMainBehaviour;

    /// <summary>
    /// This game object
    /// </summary>
    public MainBehaviour OwnMainBehaviour => ownMainBehaviour;

    /// <summary>
    /// Connected with LinkEachOther game object 
    /// </summary>
    public MainBehaviour ConnectedMainBehaviour { get; set; }

    [Header("LINE RENDERER PREFAB")]
    [SerializeField] LineRenderer line;

    /// <summary>
    /// Temp line renderer
    /// </summary>
    LineRenderer LineCopy { get; set; }

    /// <summary>
    /// Connection line renderer
    /// </summary>
    LineRenderer ConnectionLine { get; set; }

    [Header("TRANSFORM")]
    [SerializeField] Transform lineStartPoint;
    [SerializeField] Transform lineEndPoint;

    /// <summary>
    /// Is in edit mode
    /// </summary>
    bool IsSpawnUIOpen { get; set; }

    /// <summary>
    /// Mouse on output link
    /// </summary>
    bool PointerOnOutput { get; set; }

    /// <summary>
    /// If true: creates connection line
    /// </summary>
    bool Create { get; set; }


    void Awake() {

        onGroundTilesSpawnUI = ObjectsHolder.instance.OnGroundTilesSpawnUI;
    }

    void OnEnable() {

        onGroundTilesSpawnUI.OnSpawnUiActivity += OnGroundTilesSpawnUI_OnSpawnUiActivity;
    }

    void OnDisable() {

        onGroundTilesSpawnUI.OnSpawnUiActivity -= OnGroundTilesSpawnUI_OnSpawnUiActivity;
    }
   
    void Update() {

        if (IsSpawnUIOpen) {

            bool _hit = Physics.Raycast(Ray().origin, Ray().direction, out RaycastHit hit);
            bool isOnOtherInput = Physics.Raycast(Ray().origin, Ray().direction, out RaycastHit otherInput, Mathf.Infinity, ObjectsHolder.instance.LayerMasks.WorldSpaceUI);

            TempLine(hit.point);

            GetLinkTarget(isOnOtherInput, otherInput);

            CreateConnectionAndLinkGameObject(hit.point);
        }
        else {
            DestroyLineRenderer(LineCopy);
        }
    }

    #region Pointer
    public void PointerDown() {

        PointerOnOutput = true;
    }

    public void PointerUp() {

        PointerOnOutput = false;
    }
    #endregion

    Ray Ray() {

        return Camera.main.ScreenPointToRay(Input.mousePosition);
    }

    void TempLine(Vector3 hitPoint) {

        if (PointerOnOutput) {
            CreateLineCopy();
            LineDirection(0, lineStartPoint.position);
            LineDirection(1, hitPoint);
        }
        else {
            DestroyLineRenderer(LineCopy);
        }
    }

    void GetLinkTarget(bool isOnOtherInput, RaycastHit otherInput) {

        if (LineCopy != null && isOnOtherInput && otherInput.collider.GetComponentInParent<LinkEachOther>() != GetComponentInParent<LinkEachOther>() && IsSpawnUIOpen) {

            if (ConnectedMainBehaviour != otherInput.collider.GetComponentInParent<LinkEachOther>().OwnMainBehaviour) {
                lineEndPoint = otherInput.collider.gameObject.transform;
            }
        }
    }

    void CreateConnectionAndLinkGameObject(Vector3 hitPoint) {

        if (!Create && LineCopy == null && lineEndPoint != null) {

            if ((hitPoint - lineEndPoint.position).magnitude <= 0.15f) {
                CreateConnectionLine();
                ConnectedMainBehaviour = lineEndPoint.GetComponentInParent<LinkEachOther>().OwnMainBehaviour;
                Create = true;
            }
            else {
                lineEndPoint = null;
                ConnectedMainBehaviour = null;
            }
        }
    }

    void CreateConnectionLine() {

        ConnectionLine = Instantiate(line, transform);
        ConnectionLine.name = "ConnectionLine";
        ConnectionLine.SetPosition(0, lineStartPoint.position);
        ConnectionLine.SetPosition(1, lineEndPoint.position);
    }

    void CreateLineCopy() {

        if (LineCopy == null) {
            LineCopy = Instantiate(line, transform);
            LineCopy.name = "LineCopy";
            Create = false;
            ConnectedMainBehaviour = null;
            lineEndPoint = null;
            DestroyLineRenderer(ConnectionLine);
        }
    }

    void LineDirection(int index, Vector3 pos) {

        if (LineCopy != null) {
            LineCopy.SetPosition(index, pos);
        }
    }

    void DestroyLineRenderer(LineRenderer line) {

        if (line != null) {
            Destroy(line.gameObject);
        }
    }

    void OnGroundTilesSpawnUI_OnSpawnUiActivity(bool isOpened) {

        IsSpawnUIOpen = isOpened;
    }












}

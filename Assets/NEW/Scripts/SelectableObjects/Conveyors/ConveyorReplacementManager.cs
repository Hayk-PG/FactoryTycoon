using UnityEngine;

public class ConveyorReplacementManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SelectableObjectInfo _selectableObjectInfo;

    [Header("Conveyor Base Transform")]
    [SerializeField] private Transform _conveyorBase;




    private void Start()
    {
        RequestPlacementValidation();
    }

    private void OnEnable()
    {
        References.Manager.ConveyorCollection.OnCollectionAdd += OnCollectionAdd;
    }

    private void RequestPlacementValidation()
    {
        Vector3 specifiedTilePosition = new Vector3(transform.position.x, 0, transform.position.z);
        References.Manager.ObjectPlacementValidator.RaiseObjectPlacementValidationRequestEvent(specifiedTilePosition, _selectableObjectInfo);
        References.Manager.ObjectPlacementValidator.PlaceSelectedObject();
        References.Manager.ConveyorCollection.Add(transform.position, this);
    }

    private void OnCollectionAdd(Vector3 position, ConveyorReplacementManager conveyor)
    {
        CheckConveyorsOnAdjacentSides();
    }

    private void CheckConveyorsOnAdjacentSides()
    {
        bool haveConveyorsOnAdjacentSides = References.Manager.ConveyorCollection.Dict.ContainsKey(AdjacentPositions()[0]) && References.Manager.ConveyorCollection.Dict.ContainsKey(AdjacentPositions()[1]) ||
                                            References.Manager.ConveyorCollection.Dict.ContainsKey(AdjacentPositions()[2]) && References.Manager.ConveyorCollection.Dict.ContainsKey(AdjacentPositions()[3]);

        SetConveyorBaseActive(!haveConveyorsOnAdjacentSides);
    }

    private Vector3[] AdjacentPositions()
    {
        return new Vector3[]
        {
            transform.position + Vector3.right,
            transform.position + Vector3.left,
            transform.position + Vector3.forward,
            transform.position + Vector3.back
        };
    }

    private void SetConveyorBaseActive(bool isActive)
    {
        _conveyorBase.gameObject.SetActive(isActive);
    }
}

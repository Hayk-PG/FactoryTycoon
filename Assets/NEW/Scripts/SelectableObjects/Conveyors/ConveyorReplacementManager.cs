using UnityEngine;

public class ConveyorReplacementManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SelectableObjectInfo _selectableObjectInfo;




    private void Start()
    {
        RequestPlacementValidation();
    }

    private void RequestPlacementValidation()
    {
        Vector3 specifiedTilePosition = new Vector3(transform.position.x, 0, transform.position.z);
        References.Manager.ObjectPlacementValidator.RaiseObjectPlacementValidationRequestEvent(specifiedTilePosition, _selectableObjectInfo);
        References.Manager.ObjectPlacementValidator.PlaceSelectedObject();
        References.Manager.ConveyorCollection.Add(transform.position, this);
    }
}

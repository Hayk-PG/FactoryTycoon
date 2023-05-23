using UnityEngine;
using Pautik;

public class ConveyorSegment : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SelectableObjectInfo _selectableObjectInfo;

    [Header("Conveyor Base Transform")]
    [SerializeField] private Transform _conveyorBase;

    [Header("Specifications")]
    [SerializeField] private ConveyorDirection _conveyorDirection;
    [SerializeField] private int _id;

    [Header("The Beginning & Ending Parts Of The Conveyor")]
    [SerializeField] private bool _isInputSection;
    [SerializeField] private bool _isOutputSection;

    public Vector3 Direction { get; private set; }
    public int Id => _id;
    public bool IsInputSection => _isInputSection;
    public bool IsOutputSection => _isOutputSection;




    private void Start()
    {
        RequestPlacementValidation();
        SetId();
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

    private void SetId()
    {
        _id = References.Manager.ConveyorCollection.Dict.Count;
    }

    private void OnCollectionAdd(Vector3 position, ConveyorSegment conveyor)
    {
        CheckConveyorsOnAdjacentSides();
    }

    private void CheckConveyorsOnAdjacentSides()
    {
        bool haveConveyorsOnAdjacentSides = HasNeighborConveyer(0) && HasNeighborConveyer(1) || HasNeighborConveyer(2) && HasNeighborConveyer(3);

        SetConveyorBaseActive(!haveConveyorsOnAdjacentSides);
        SetIOSections(!haveConveyorsOnAdjacentSides);
    }

    /// <summary>
    /// Checks if there is a neighboring conveyor segment at the specified adjacent position index in the right, left, forward, or back directions.
    /// </summary>
    /// <param name="adjacentPositionIndex">The index of the adjacent position to check (0 for right, 1 for left, 2 for forward, 3 for back).</param>
    /// <returns>True if there is a neighboring conveyor segment, false otherwise.</returns>
    private bool HasNeighborConveyer(int adjacentPositionIndex)
    {
        return Checker.ContainsKey<Vector3, ConveyorSegment>(References.Manager.ConveyorCollection.Dict, AdjacentPositionCalculator.GetAdjacentPositions(transform.position)[adjacentPositionIndex]);
    }

    private void SetConveyorBaseActive(bool isActive)
    {
        _conveyorBase.gameObject.SetActive(isActive);
    }

    private void SetIOSections(bool? isOutputSection = null, bool? isInputSection = null)
    {
        bool hasOutputSection = isOutputSection.HasValue;
        bool hasInputSection = isInputSection.HasValue;

        if (hasOutputSection)
        {
            if(IsInputSection && isOutputSection.Value)
            {
                return;
            }

            _isOutputSection = isOutputSection.Value;
        }

        if (hasInputSection)
        {
            _isInputSection = isInputSection.Value;
        }
    }

    private void SetConveyorDirection()
    {
        bool hasConveyorInRightDirection = HasNeighborConveyer(0);
        bool hasConveyorInLeftDirection = HasNeighborConveyer(0);
        bool hasConveyorInForwardDirection = HasNeighborConveyer(0);
        bool hasConveyorInBackwardDirection = HasNeighborConveyer(0);

        if (hasConveyorInRightDirection)
        {
            _conveyorDirection = ConveyorDirection.Left;
        }

        if (hasConveyorInLeftDirection)
        {
            _conveyorDirection = ConveyorDirection.Right;
        }

        if (hasConveyorInForwardDirection)
        {
            _conveyorDirection = ConveyorDirection.Backward;
        }

        if (hasConveyorInBackwardDirection)
        {
            _conveyorDirection = ConveyorDirection.Forward;
        }
    }
}

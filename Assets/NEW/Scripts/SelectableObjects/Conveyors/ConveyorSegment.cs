using UnityEngine;
using Pautik;
using System;

public class ConveyorSegment : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SelectableObjectInfo _selectableObjectInfo;
    [SerializeField] private ConveyorRenderer _conveyorRenderer;

    [Header("Conveyor Base Transform")]
    [SerializeField] private Transform _conveyorBase;

    [Header("Specifications")]
    [SerializeField] private ConveyorDirection _conveyorDirection;
    [SerializeField] private int _id;

    [Header("The Beginning & Ending Parts Of The Conveyor")]
    [SerializeField] private bool _isInputSection;
    [SerializeField] private bool _isOutputSection;

    public ConveyorRenderer ConveyorRenderer => _conveyorRenderer;
    public ConveyorDirection ConveyorDirection => _conveyorDirection;
    public Vector3 Direction { get; private set; } = Vector3.left; // Default direction
    public int Id => _id;
    public bool IsInputSection => _isInputSection;
    public bool IsOutputSection => _isOutputSection;




    private void Start()
    {
        // Perform placement validation, set ID, and add GUI element
        RequestPlacementValidation();
        SetId();
        AddGUIElement();
    }

    private void OnEnable()
    {
        // Subscribe to the collection add event
        References.Manager.ConveyorCollection.OnCollectionAdd += OnCollectionAdd;
    }

    // Get the specified tile position and raise placement validation request
    private void RequestPlacementValidation()
    {
        Vector3 specifiedTilePosition = new Vector3(transform.position.x, 0, transform.position.z);
        References.Manager.ObjectPlacementValidator.RaiseObjectPlacementValidationRequestEvent(specifiedTilePosition, _selectableObjectInfo);
        References.Manager.ObjectPlacementValidator.PlaceSelectedObject();
        References.Manager.ConveyorCollection.Add(transform.position, this);
    }

    // Set the ID based on the current collection count
    private void SetId()
    {
        _id = References.Manager.ConveyorCollection.Dict.Count;
    }

    // Add the GUI element to the ConveyorSystemManager
    private void AddGUIElement()
    {
        References.Manager.ConveyorSystemManager.AddGUIElement(this);
    }
    
    private void OnCollectionAdd(Vector3 position, ConveyorSegment conveyor)
    {
        // Check adjacent conveyors when a new conveyor is added to the collection
        CheckConveyorsOnAdjacentSides();
    }
       
    private void CheckConveyorsOnAdjacentSides()
    {
        // Check if there are neighboring conveyors on adjacent sides
        bool haveConveyorsOnAdjacentSides = HasNeighborConveyer(0) && HasNeighborConveyer(1) || HasNeighborConveyer(2) && HasNeighborConveyer(3);

        // Set the conveyor base and input/output sections based on adjacent conveyors
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

    // Set the conveyor base active/inactive based on the isActive flag
    private void SetConveyorBaseActive(bool isActive)
    {
        _conveyorBase.gameObject.SetActive(isActive);
    }

    // Check if there are output and input section values provided
    // Set the output section if provided and handle input/output section conflicts
    // Set the input section if provided
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

    /// <summary>
    /// Set the conveyor direction and corresponding direction vector
    /// </summary>
    /// <param name="conveyorDirection"></param>
    public void SetConveyorDirection(ConveyorDirection conveyorDirection)
    {
        _conveyorDirection = conveyorDirection;

        switch (conveyorDirection)
        {
            case ConveyorDirection.Right: Direction = Vector3.right; break;
            case ConveyorDirection.Left: Direction = Vector3.left; break;
            case ConveyorDirection.Forward: Direction = Vector3.forward; break;
            case ConveyorDirection.Backward: Direction = Vector3.back; break;
        }     
    }
}

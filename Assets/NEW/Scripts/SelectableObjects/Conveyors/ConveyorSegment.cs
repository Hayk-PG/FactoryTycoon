using UnityEngine;
using Pautik;
using System;

public class ConveyorSegment : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SelectableObjectInfo _selectableObjectInfo;
    [SerializeField] private ConveyorRenderer _conveyorRenderer;
    [SerializeField] private ConveyorTrigger _conveyorTrigger;

    [Header("Conveyor Base Transform")]
    [SerializeField] private Transform _conveyorBase;

    [Header("Specifications")]
    [SerializeField] private ConveyorDirection _conveyorDirection;
    [SerializeField] private int _id;

    [Header("The Beginning & Ending Parts Of The Conveyor")]
    [SerializeField] private bool _isInputSection;
    [SerializeField] private bool _isOutputSection;

    public ConveyorRenderer ConveyorRenderer => _conveyorRenderer;
    public ConveyorTrigger ConveyorTrigger => _conveyorTrigger;
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
        bool haveConveyorsOnAdjacentSides = HasNeighborConveyor(0) && HasNeighborConveyor(1) || HasNeighborConveyor(2) && HasNeighborConveyor(3);

        // Set the conveyor base and input/output sections based on adjacent conveyors
        SetConveyorBaseActive(!haveConveyorsOnAdjacentSides);
        SetIOSections(!haveConveyorsOnAdjacentSides);
    }

    /// <summary>
    /// Checks if there is a neighboring conveyor segment at the specified adjacent position index in the right, left, forward, or back directions.
    /// </summary>
    /// <param name="adjacentPositionIndex">The index of the adjacent position to check (0 for right, 1 for left, 2 for forward, 3 for back).</param>
    /// <returns>True if there is a neighboring conveyor segment, false otherwise.</returns>
    private bool HasNeighborConveyor(int adjacentPositionIndex)
    {
        return Checker.ContainsKey(References.Manager.ConveyorCollection.Dict, AdjacentPositionCalculator.GetAdjacentPositions(transform.position)[adjacentPositionIndex]);
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

        // Set the output section if provided and handle conflicts with input section
        if (hasOutputSection)
        {
            if(IsInputSection && isOutputSection.Value)
            {
                // If it is both an input and output section, return without setting the output section
                return;
            }

            _isOutputSection = isOutputSection.Value;
        }

        // Set the input section if provided
        if (hasInputSection)
        {
            _isInputSection = isInputSection.Value;
        }
    }

    /// <summary>
    /// Set the conveyor direction and corresponding direction vector based on the specified ConveyorDirection.
    /// </summary>
    /// <param name="conveyorDirection">The conveyor direction to set.</param>
    public void SetConveyorDirection(ConveyorDirection conveyorDirection)
    {
        _conveyorDirection = conveyorDirection;

        // Set the corresponding direction vector based on the conveyor direction
        switch (conveyorDirection)
        {
            case ConveyorDirection.Right: Direction = Vector3.right; break;
            case ConveyorDirection.Left: Direction = Vector3.left; break;
            case ConveyorDirection.Forward: Direction = Vector3.forward; break;
            case ConveyorDirection.Backward: Direction = Vector3.back; break;
        }     
    }

    /// <summary>
    /// Set the conveyor direction and corresponding direction vector based on the specified direction vector.
    /// </summary>
    /// <param name="direction">The direction vector to set.</param>
    public void SetConveyorDirection(Vector3 direction)
    {
        Direction = direction;

        // Determine the conveyor direction based on the direction vector
        _conveyorDirection = Direction == Vector3.right ? ConveyorDirection.Right : Direction == Vector3.left ? ConveyorDirection.Left :
                             Direction == Vector3.forward ? ConveyorDirection.Forward : ConveyorDirection.Backward;             
    }

    /// <summary>
    /// Updates the direction of the previous conveyor segment based on the new conveyor segment.
    /// </summary>
    /// <param name="previousConveyorSegment">The previous conveyor segment to update.</param>
    /// <param name="direction">The new direction to set for the previous conveyor segment.</param>
    public void UpdatePreviousConveyorDirection(ConveyorSegment previousConveyorSegment, Vector3 direction)
    {
        // Set the conveyor direction of the previous segment if it exists
        previousConveyorSegment?.SetConveyorDirection(direction);
    }

    /// <summary>
    /// Checks if the conveyor can extend based on the number of neighboring conveyors.
    /// </summary>
    /// <returns><c>true</c> if the conveyor can extend; otherwise, <c>false</c>.</returns>
    public bool CanExtendWithFewNeighbors()
    {
        int neighbors = 0;

        for (int i = 0; i < 4; i++)
        {
            // Check if there is a neighboring conveyor in the specified direction
            if (HasNeighborConveyor(i))
            {
                neighbors++;
            }
        }

        return neighbors < 2;
    }
}

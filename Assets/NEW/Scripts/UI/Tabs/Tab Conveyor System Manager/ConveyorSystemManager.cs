using UnityEngine;

public class ConveyorSystemManager : MonoBehaviour
{
    [Header("UI Element")]
    [SerializeField] private ConveyorGUIElement _conveyourGuiElement;
    [SerializeField] private Transform _content;
    [SerializeField] private Btn _btnRightDirection;
    [SerializeField] private Btn _btnLeftDirection;
    [SerializeField] private Btn _btnForwardDirection;
    [SerializeField] private Btn _btnBackwardDirection;

    private ConveyorSegment _selectedConveyor;




    private void OnEnable()
    {
        SubscribeToDirectionButtons();
    }

    private void SubscribeToDirectionButtons()
    {
        _btnRightDirection.OnSelect += () => OnSelectDirection(ConveyorDirection.Right);
        _btnLeftDirection.OnSelect += () => OnSelectDirection(ConveyorDirection.Left);
        _btnForwardDirection.OnSelect += () => OnSelectDirection(ConveyorDirection.Forward);
        _btnBackwardDirection.OnSelect += () => OnSelectDirection(ConveyorDirection.Backward);
    }

    private void OnSelectDirection(ConveyorDirection conveyorDirection)
    {
        bool isConveyorSelected = _selectedConveyor != null;

        if (!isConveyorSelected)
        {
            return;
        }

        _selectedConveyor.SetConveyorDirection(conveyorDirection);
    }

    /// <summary>
    /// Adds a GUI element for the specified conveyor segment to the content area.
    /// </summary>
    /// <param name="conveyorSegment">The conveyor segment to add a GUI element for.</param>
    public void AddGUIElement(ConveyorSegment conveyorSegment)
    {
        ConveyorGUIElement conveyorGUIElement = Instantiate(_conveyourGuiElement, _content);
        conveyorGUIElement.Initialize(conveyorSegment);
        conveyorGUIElement.name = _conveyourGuiElement.name;
    }

    /// <summary>
    /// Sets the selected conveyor segment.
    /// </summary>
    /// <param name="conveyorSegment">The conveyor segment to set as selected.</param>
    public void SetSelectedConveyor(ConveyorSegment conveyorSegment)
    {
        // Toggle highlight for the previously selected conveyor segment
        ToggleConveyorHighlight(_selectedConveyor, false);
        // Toggle highlight for the newly selected conveyor segment
        ToggleConveyorHighlight(conveyorSegment, true);
        // Set the newly selected conveyor segment as the currently selected conveyor
        _selectedConveyor = conveyorSegment;

    }

    /// <summary>
    /// Selects the active conveyor direction based on the conveyor segment's direction.
    /// </summary>
    /// <param name="conveyorSegment">The conveyor segment to set as active.</param>
    public void SelectedActiveConveyorDirection(ConveyorSegment conveyorSegment)
    {
        switch (conveyorSegment.ConveyorDirection)
        {
            case ConveyorDirection.Right: _btnRightDirection.Select(); break;
            case ConveyorDirection.Left: _btnLeftDirection.Select(); break;
            case ConveyorDirection.Forward: _btnForwardDirection.Select(); break;
            case ConveyorDirection.Backward: _btnBackwardDirection.Select(); break;
        }
    }

    /// <summary>
    /// Toggles the highlight state of the conveyor segment.
    /// </summary>
    /// <param name="conveyorSegment">The conveyor segment to toggle.</param>
    /// <param name="isHighlighted">Flag indicating whether to highlight the conveyor segment.</param>
    public void ToggleConveyorHighlight(ConveyorSegment conveyorSegment, bool isHighlighted)
    {
        if(conveyorSegment == null)
        {
            return;
        }

        if (isHighlighted)
        {
            conveyorSegment.ConveyorRenderer.SetHighlightMaterial();
        }
        else
        {
            conveyorSegment.ConveyorRenderer.SetDefaultMaterial();
        }
    }
}

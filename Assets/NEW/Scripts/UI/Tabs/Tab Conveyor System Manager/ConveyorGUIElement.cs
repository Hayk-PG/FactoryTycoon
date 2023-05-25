using UnityEngine;

public class ConveyorGUIElement : MonoBehaviour
{
    [Header("UI Element")]
    [SerializeField] private Btn _btnConveyor;
    [SerializeField] private BtnTxt _btnTxtConveyor;

    private ConveyorSegment _associatedConveyor;




    private void OnEnable()
    {
        _btnConveyor.OnSelect += OnSelect;
    }

    private void OnSelect()
    {
        SetSelectedConveyor();
        SelectedActiveConveyorDirection();
    }

    /// <summary>
    /// Initializes the GUI element with the specified conveyor segment.
    /// </summary>
    /// <param name="conveyorSegment">The conveyor segment to associate with the GUI element.</param>
    public void Initialize(ConveyorSegment conveyorSegment)
    {
        GetAssociatedConveyor(conveyorSegment);
        UpdateButtonText(conveyorSegment);
    }

    private void GetAssociatedConveyor(ConveyorSegment conveyorSegment)
    {
        _associatedConveyor = conveyorSegment;
    }

    private void UpdateButtonText(ConveyorSegment conveyorSegment)
    {
        _btnTxtConveyor.SetButtonTitle($"{conveyorSegment.Id}: Segment");
    }

    private void SetSelectedConveyor()
    {
        References.Manager.ConveyorSystemManager.SetSelectedConveyor(_associatedConveyor);
    }

    private void SelectedActiveConveyorDirection()
    {
        References.Manager.ConveyorSystemManager.SelectedActiveConveyorDirection(_associatedConveyor);
    }
}

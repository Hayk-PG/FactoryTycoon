using UnityEngine;

public class TabEditMode : BaseTabManager
{
    [Header("Edit Mode Switcher")]
    [SerializeField] private Btn _btnSwitcher;




    protected override void OnEnable()
    {
        base.OnEnable();

        _btnSwitcher.OnSelect += OnSelect;
    }

    private void OnSelect()
    {
        OpenCurrentTab();
        ToggleEditMode(true);
    }

    private void ToggleEditMode(bool isEditMode)
    {
        References.Manager.EditModeManager.RaiseEditModeEvent(isEditMode);
    }

    public override void CloseCurrentTab(object[] data = null)
    {
        base.CloseCurrentTab(data);

        ToggleEditMode(false);
    }
}

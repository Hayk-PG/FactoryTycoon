using UnityEngine;
using Pautik;

public class TabConveyorSystemMode : BaseTabManager
{
    [Header("Conveyor System Mode Switcher")]
    [SerializeField] private Btn _btnSwitcher;



    protected override void OnEnable()
    {
        base.OnEnable();

        _btnSwitcher.OnSelect += OnConveyerSystemModeSwitch;
    }

    private void OnConveyerSystemModeSwitch()
    {
        ChangeTab(TabType.EditMode);
        OpenCurrentTab();
    }
}

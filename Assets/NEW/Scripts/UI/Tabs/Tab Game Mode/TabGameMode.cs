using UnityEngine;

public class TabGameMode : BaseTabManager
{
    [Header("Game Mode Switcher")]
    [SerializeField] private Btn _btnGameModeSwitcher;



    protected override void OnEnable()
    {
        base.OnEnable();

        _btnGameModeSwitcher.OnSelect += OnSelect;
    }

    private void OnSelect()
    {
        OpenCurrentTab();
    } 
}

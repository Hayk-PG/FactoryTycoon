using System;
using UnityEngine;

public class EditModeManager : MonoBehaviour
{
    [Header("UI Elemetns")]
    [SerializeField] private Btn _btnEditModeSwitcher;
    [SerializeField] private BtnTxt _btnTxtEditMode;

    [Header("Object Selection Manager")]
    [SerializeField] private ObjectSelectionManager _objectSelectionManager;

    private bool _isEditMode;

    public event Action<bool> OnEditMode;
    
    
    

    private void OnEnable()
    {
        _btnEditModeSwitcher.OnSelect += OnSelect;
    }

    private void OnSelect()
    {
        ToggleEditMode();
        RaiseEditModeEvent();
        ChangeBtnText();
        SetObjectSelectionSubTabActive();
    }

    private void ToggleEditMode()
    {
        _isEditMode = !_isEditMode;
    }

    private void RaiseEditModeEvent()
    {
        OnEditMode?.Invoke(_isEditMode);
    }

    private void ChangeBtnText()
    {
        _btnTxtEditMode.SetButtonTitle(_isEditMode ? $"edit mode" : $"game mode");
    }

    private void SetObjectSelectionSubTabActive()
    {
        _objectSelectionManager.SetActive(_isEditMode);
    }
}

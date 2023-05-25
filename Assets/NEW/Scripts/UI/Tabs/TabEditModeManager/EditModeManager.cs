using System;
using UnityEngine;

public class EditModeManager : MonoBehaviour
{
    public event Action<bool> OnEditMode;
    
    public void RaiseEditModeEvent(bool isEditMode)
    {
        OnEditMode?.Invoke(isEditMode);
    }
}

using System;
using UnityEngine;

public class BaseUIManager : MonoBehaviour, IBaseUIManager
{
    public object[] Data { get; set; }

    public event Action<TabType, ITab, bool, object[]> OnTabChanged;
    
    
    
    
    public void RaiseEvent(TabType targetTabType, ITab linkedTab, bool activate = true, object[] data = null)
    {
        OnTabChanged?.Invoke(targetTabType, linkedTab, activate, data);
    }
}

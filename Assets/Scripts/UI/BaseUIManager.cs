using System;
using UnityEngine;

public class BaseUIManager : MonoBehaviour, IBaseUIManager
{
    public object[] Data { get; set; }

    public event Action<TabType, ITabManager, bool, object[]> OnTabChanged;
    
    
    
    
    public void RaiseEvent(TabType targetTabType, ITabManager linkedTab, bool activate = true, object[] data = null)
    {
        OnTabChanged?.Invoke(targetTabType, linkedTab, activate, data);
    }
}

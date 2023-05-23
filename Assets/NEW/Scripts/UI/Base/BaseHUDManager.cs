using System;
using UnityEngine;

public class BaseHUDManager : MonoBehaviour, IHUDManager
{
    public ITabManager[] _ChildTabs { get; protected set; }
    public object[] Data { get; set; }

    public event Action<TabType, ITabManager, bool, object[]> OnTabChanged;
    
    
    
    
    protected virtual void Awake()
    {
        GetChildTabs();
    }

    // Get all child tab managers
    protected virtual void GetChildTabs()
    {
        _ChildTabs = GetComponentsInChildren<ITabManager>();
    }

    /// <summary>
    /// Raises the tab change event, notifying subscribers about a tab change.
    /// </summary>
    /// <param name="targetTabType">The target tab type that has changed.</param>
    /// <param name="linkedTab">The linked tab manager associated with the tab.</param>
    /// <param name="activate">Flag indicating whether to activate the tab.</param>
    /// <param name="data">Optional data to pass along with the tab change event.</param>
    public void RaiseEvent(TabType targetTabType, ITabManager linkedTab, bool activate = true, object[] data = null)
    {
        OnTabChanged?.Invoke(targetTabType, linkedTab, activate, data);
    }

    /// <summary>
    /// Opens the current tab and closes all other tabs.
    /// </summary>
    /// <param name="currentTab">The current tab manager to keep open.</param>
    public void OpenCurrentAndCloseOthers(ITabManager currentTab)
    {
        if(currentTab == null)
        {
            return;
        }

        foreach (var childTab in _ChildTabs)
        {
            if(childTab == currentTab)
            {
                continue;
            }

            childTab.CloseCurrentTab();
        }
    }
}

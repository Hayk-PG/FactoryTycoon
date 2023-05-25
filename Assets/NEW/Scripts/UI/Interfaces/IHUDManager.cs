using System;

public interface IHUDManager 
{
    object[] Data { get; set; }

    event Action<TabType, ITabManager, bool, object[]> OnTabChanged;

    void RaiseEvent(TabType targetTabType, ITabManager linkedTab, bool activate = true, object[] data = null);
    void OpenCurrentAndCloseOthers(ITabManager currentTab);
}

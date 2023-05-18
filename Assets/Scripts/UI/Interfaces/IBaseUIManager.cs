using System;

public interface IBaseUIManager 
{
    object[] Data { get; set; }

    event Action<TabType, ITab, bool, object[]> OnTabChanged;

    void RaiseEvent(TabType targetTabType, ITab linkedTab, bool activate = true, object[] data = null);
}

using UnityEngine;
using Pautik;

public class BaseUITab : MonoBehaviour, ITab
{
    [Header("UI Elements")]
    [SerializeField] protected CanvasGroup _canvasGroup;

    [Header("Tab Configuration")] 
    [SerializeField] protected TabType _currentTabType;

    protected IBaseUIManager _iBaseUIManager;
    protected object[] _data;

    public ITab ObserverTab { get; set; }
    
    
    
    
    protected virtual void Awake()
    {
        GetBaseUIManager();
    }

    protected virtual void OnEnable()
    {
        SubscribeToBaseUIManager();
    }

    /// <summary>
    /// Get the reference to the base UI manager.
    /// </summary>
    protected void GetBaseUIManager()
    {
        _iBaseUIManager = Get<IBaseUIManager>.From(gameObject);
    }

    /// <summary>
    /// Subscribe to the tab changed event of the base UI manager.
    /// </summary>
    protected virtual void SubscribeToBaseUIManager()
    {
        if (_iBaseUIManager != null)
        {
            _iBaseUIManager.OnTabChanged += OnTabChanged;
        }
    }

    /// <summary>
    /// Handle the tab changed event from the base UI manager.
    /// </summary>
    protected virtual void OnTabChanged(TabType targetTabType, ITab linkedTab, bool activate, object[] data)
    {
        if (!IsCurrentTab(targetTabType))
        {
            return;
        }

        RetrieveData(data);
        ReleaseObserverTab();
        BecomeObserverTab(linkedTab);
    }

    /// <summary>
    /// Check if the current tab matches the target tab type.
    /// </summary>
    protected bool IsCurrentTab(TabType targetTabType)
    {
        return targetTabType == _currentTabType;
    }

    /// <summary>
    /// Retrieve data passed from another tab.
    /// </summary>
    protected virtual void RetrieveData(object[] data)
    {
        if (data == null)
        {
            return;
        }

        _data = data;
    }

    /// <summary>
    /// Release the observer tab reference.
    /// </summary>
    protected virtual void ReleaseObserverTab()
    {
        ObserverTab?.CloseCurrentTab();
        ObserverTab = null;
    }

    /// <summary>
    /// Assigns the current tab as an observer to the linked tab, establishing a one-way communication relationship.
    /// The current tab will receive updates and notifications from the linked tab.
    /// </summary>
    protected virtual void BecomeObserverTab(ITab linkedTab)
    {
        linkedTab.ObserverTab = this;
    }

    /// <summary>
    /// Change to a different tab.
    /// </summary>
    protected virtual void ChangeTab(TabType targetTabType, bool activate = true, object[] data = null)
    {
        _iBaseUIManager.RaiseEvent(targetTabType, this, activate, data);
    }

    /// <summary>
    /// Close the current tab.
    /// </summary>
    public void CloseCurrentTab(object[] data = null)
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, false);
        RetrieveData(data);
    }
}

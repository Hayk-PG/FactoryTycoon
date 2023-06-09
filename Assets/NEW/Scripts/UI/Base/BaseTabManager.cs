using UnityEngine;
using Pautik;

[RequireComponent(typeof(CanvasGroup))]
public class BaseTabManager : MonoBehaviour, ITabManager
{
    [Header("UI Elements")]
    [SerializeField] protected CanvasGroup _canvasGroup;

    [Header("Tab Configuration")] 
    [SerializeField] protected TabType _currentTabType;

    protected IHUDManager _hudManager;
    protected object[] _data;

    public bool IsCurrentTabOpen
    {
        get => _canvasGroup.alpha > 0.1f;
    }
    public ITabManager ObserverTab { get; set; }
    
    
    
    
    protected virtual void Awake()
    {
        GetBaseHUDManager();
    }

    protected virtual void OnEnable()
    {
        SubscribeToBaseHUDManager();
    }

    /// <summary>
    /// Get the reference to the base HUD manager.
    /// </summary>
    protected void GetBaseHUDManager()
    {
        _hudManager = Get<IHUDManager>.From(gameObject);
    }

    /// <summary>
    /// Close all other child tabs except the current tab
    /// </summary>
    protected virtual void OpenCurrentTab()
    {
        if (IsCurrentTabOpen)
        {
            return;
        }
       
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, true);
        _hudManager.OpenCurrentAndCloseOthers(this);
    }

    /// <summary>
    /// Subscribe to the tab changed event of the base HUD manager.
    /// </summary>
    protected virtual void SubscribeToBaseHUDManager()
    {
        if (_hudManager != null)
        {
            _hudManager.OnTabChanged += OnTabChanged;
        }
    }

    /// <summary>
    /// Handle the tab changed event from the base HUD manager.
    /// </summary>
    protected virtual void OnTabChanged(TabType targetTabType, ITabManager linkedTab, bool activate, object[] data)
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
    protected virtual void BecomeObserverTab(ITabManager linkedTab)
    {
        linkedTab.ObserverTab = this;
    }

    /// <summary>
    /// Change to a different tab.
    /// </summary>
    protected virtual void ChangeTab(TabType targetTabType, bool activate = true, object[] data = null)
    {
        _hudManager.RaiseEvent(targetTabType, this, activate, data);
    }

    /// <summary>
    /// Close the current tab.
    /// </summary>
    public virtual void CloseCurrentTab(object[] data = null)
    {
        if (!IsCurrentTabOpen)
        {
            return;
        }

        GlobalFunctions.CanvasGroupActivity(_canvasGroup, false);
        RetrieveData(data);
    }
}

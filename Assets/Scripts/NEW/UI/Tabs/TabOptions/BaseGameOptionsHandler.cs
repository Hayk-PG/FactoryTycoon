using UnityEngine;

public abstract class BaseGameOptionsHandler : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] protected Btn _btnGameOptions;
    
    
    
    
    protected virtual void OnEnable()
    {
        SubscribeToBtnEvents();
    }

    protected virtual void SubscribeToBtnEvents()
    {
        if(_btnGameOptions == null)
        {
            return;
        }

        _btnGameOptions.onSelect += OnSelect;
        _btnGameOptions.onDeselect += OnDeselect;
    }

    /// <summary>
    /// Called when the game option button is selected.
    /// </summary>
    protected virtual void OnSelect()
    {
        Execute();
    }

    /// <summary>
    /// Called when the game option button is deselected.
    /// </summary>
    protected virtual void OnDeselect()
    {
        // Implement the logic for when the game option button is deselected
    }

    /// <summary>
    /// Executes the specific action associated with the game option.
    /// </summary>
    protected abstract void Execute();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MachineUIController : MonoBehaviour,IGetCanvasGroup
{
    public event System.Action<int> OnClickMachineCtrlButton;

    [SerializeField] protected Transform buttonsContainer;
    [SerializeField] Button[] machineCntrlButtons;

    protected Button[] MachineCtrl_Buttons
    {
        get
        {
            return machineCntrlButtons;
        }
        set
        {
            machineCntrlButtons = value;
        }
    }
    public bool IsCanvasGroupInteractable
    {
        get
        {
            return GetComponent<CanvasGroup>().interactable;
        }
    }

    void Awake()
    {
        MachineCtrl_Buttons = buttonsContainer.GetComponentsInChildren<Button>();
    }


    void Update()
    {
        if(IsCanvasGroupInteractable)
        {
            OnClickMachineCtrlButtons();
        }       
    }

    void OnClickMachineCtrlButtons()
    {
        for (int i = 0; i < MachineCtrl_Buttons.Length; i++)
        {
            int index = (int)MachineCtrl_Buttons[i].GetComponent<MachineButtonInfo>().MACHINE_TOOL;
            MachineCtrl_Buttons[i].onClick.RemoveAllListeners();
            MachineCtrl_Buttons[i].onClick.AddListener(() => { OnClickMachineCtrlButton?.Invoke(index); });
        }
    }





}

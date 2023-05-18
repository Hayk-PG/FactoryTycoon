using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QCMachineUIController : MachineUIController, IGetCanvasGroup
{
    public event Action<int> OnQualityCheckEvery_Int_Item;

    [SerializeField] QualityController qc;

    [SerializeField] Text goodPartsText;
    [SerializeField] Text badPartsText;

    int GoodPartsNumber
    {
        get
        {
            return int.Parse(goodPartsText.text);
        }
        set
        {
            goodPartsText.text = value.ToString();
        }
    }
    int BadPartsNumber
    {
        get
        {
            return int.Parse(badPartsText.text);
        }
        set
        {
            badPartsText.text = value.ToString();
        }
    }
    int ItemsNumber { get; set; }


    void Awake()
    {

    }

    void OnEnable()
    {
        qc.OnPartsCount += Qc_OnPartsCount;
    }

    void OnDisable()
    {
        qc.OnPartsCount -= Qc_OnPartsCount;
    }
    
    void Update()
    {
        if(IsCanvasGroupInteractable)
        {
            OnClickChooseButtons();
        }      
    }

    void OnClickChooseButtons()
    {
        for (int i = 0; i < MachineCtrl_Buttons.Length; i++)
        {
            int index = i;
            MachineCtrl_Buttons[index].onClick.RemoveAllListeners();
            MachineCtrl_Buttons[index].onClick.AddListener(()=> 
            {
                OnQualityCheckEvery_Int_Item?.Invoke(4);
            });
        }
    }

    void Qc_OnPartsCount(int arg1, int arg2)
    {
        GoodPartsNumber = arg1;
        BadPartsNumber = arg2;
    }

    



}

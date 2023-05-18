using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QualityController : MonoBehaviour,IMachineLightsController
{
    public event Action<int, int> OnPartsCount;
    public event Action<bool> OnSendMachineStatus;

    [SerializeField] QCMachineUIController QCMachineUI;

    int triggeredItemsNumber;
    int ItemsNumber { get; set; }

    int GoodPartsCount { get; set; }
    int BadPartsCount { get; set; }


    void OnEnable()
    {
        QCMachineUI.OnQualityCheckEvery_Int_Item += QCMachineUI_OnQualityCheckEvery_Int_Item;
    }
    
    void OnDIsable()
    {
        QCMachineUI.OnQualityCheckEvery_Int_Item -= QCMachineUI_OnQualityCheckEvery_Int_Item;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<BaseItemDeformation>() != null)
        {
            ItemCheck(other.GetComponent<BaseItemDeformation>(), out int matchedValues, out int BlendShapesLength);
            Output(matchedValues, BlendShapesLength);

            if (triggeredItemsNumber >= ItemsNumber - 1)
            {                               
                OnPartsCount?.Invoke(GoodPartsCount, BadPartsCount);
                triggeredItemsNumber = 0;
            }
            else
            {
                triggeredItemsNumber++;                
            }
        }
    }

    void ItemCheck(BaseItemDeformation BaseItemDeformation, out int matchedValues, out int BlendShapesLength)
    {
        BaseItemDeformation item = BaseItemDeformation;

        matchedValues = 0;

        for (int i = 0; i < item.GetBlendShapes().Length; i++)
        {
            if (item.GetBlendShapes()[i] >= item.ItemGoodValuesMin[i] && item.GetBlendShapes()[i] <= item.ItemGoodValuesMax[i])
            {
                matchedValues++;
            }
        }

        BlendShapesLength = item.GetBlendShapes().Length;
    }

    void Output(int matchedValues, int BlendShapesLength)
    {
        if (matchedValues == BlendShapesLength)
        {
            OnSendMachineStatus?.Invoke(true);
            GoodPartsCount++;
        }
        else
        {
            OnSendMachineStatus?.Invoke(false);
            BadPartsCount++;
        }
    }

    void QCMachineUI_OnQualityCheckEvery_Int_Item(int obj)
    {
        ItemsNumber = obj;
    }












}

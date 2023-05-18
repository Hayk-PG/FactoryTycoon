using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemVariation : MonoBehaviour, IItemVariation<MachineController, ItemVariation.ITEM_VARIATION>
{
    public enum ITEM_VARIATION { PRESSED, HOLED}
    public ITEM_VARIATION Item_Variation;

    /// <summary>
    /// The machine,in which the item got loaded or unloaded from
    /// </summary>
    public IGetInstance<MachineController> GetRunningMachine { get; set; }

    [Header("SAME TYPE ITEMS")]
    [SerializeField] ItemVariation[] SameTypeItems;


    /// <summary>
    /// IITEMVaritation Replace loaded item with the new correct one
    /// </summary>
    /// <param name="ItemVariationEnum"></param>
    /// <param name="localPos"></param>
    /// <param name="localEulerAngles"></param>
    /// <param name="parent"></param>
    /// <param name="RunningMachine"></param>
    public void ReplaceTheItemWithItemVariation(ITEM_VARIATION ItemVariationEnum, Vector3 localPos, Vector3 localEulerAngles, Transform parent, MachineController RunningMachine) {

        Item_Variation = ItemVariationEnum;
        int index = (int)ItemVariationEnum;

        if (index <= SameTypeItems.Length - 1) {

            ItemVariation activeTypeItemCopy = Instantiate(SameTypeItems[index], parent);
            //activeTypeItemCopy.GetRunningMachine = RunningMachine;
            activeTypeItemCopy.transform.localPosition = localPos;
            activeTypeItemCopy.transform.localEulerAngles = localEulerAngles;
        }        
    }









}

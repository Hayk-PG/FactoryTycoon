using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemVariation<T1, T2>
{  
    IGetInstance<T1> GetRunningMachine { get; set; }

    /// <summary>
    /// IITEMVaritation Replace loaded item with the new correct one
    /// </summary>
    /// <param name="ItemVariationEnum"></param>
    /// <param name="localPos"></param>
    /// <param name="localEulerAngles"></param>
    /// <param name="parent"></param>
    /// <param name="RunningMachine"></param>
    void ReplaceTheItemWithItemVariation(T2 ItemVariationEnum, Vector3 localPos, Vector3 localEulerAngles, Transform parent, T1 RunningMachine);






}

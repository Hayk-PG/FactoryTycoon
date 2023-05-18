using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseItemDeformation : MonoBehaviour
{
    [Header("MATERIAL")]
    [SerializeField] Material cleanedUpMaterial;

    [Header("QUALITY CHECK")]
    [SerializeField] float[] itemGoodValuesMin;
    [SerializeField] float[] itemGoodValuesMax;
    public SkinnedMeshRenderer Skin
    {
        get
        {
            return GetComponent<SkinnedMeshRenderer>();
        }
    }
    public Material SkinMaterial
    {
        get
        {
            return Skin.material;
        }
        set
        {
            Skin.material = value;
        }
    }

    /// <summary>
    /// Machine in which item's got loaded 
    /// </summary>
    public IMachineController Machine { get; set; }

    public float[] ItemGoodValuesMin => itemGoodValuesMin;
    public float[] ItemGoodValuesMax => itemGoodValuesMax;



    public void SetBlendShapeValue(int index, float value)
    {
        if(index <= Skin.sharedMesh.blendShapeCount - 1)
        {
            Skin.SetBlendShapeWeight(index, value * 100);
        } 
    }

    public float[] GetBlendShapes()
    {
        float[] blendShape = new float[Skin.sharedMesh.blendShapeCount];

        for (int i = 0; i < blendShape.Length; i++)
        {
            blendShape[i] = Skin.GetBlendShapeWeight(i);
        }

        return blendShape;
    }

    public void CleanUp()
    {
        SkinMaterial = cleanedUpMaterial;
    }

   

















}

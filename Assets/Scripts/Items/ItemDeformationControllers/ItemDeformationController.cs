using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDeformationController : MonoBehaviour,IItemDeformation
{
    public MachineToolController.MACHINE_TOOL machine_tool;
   
    [Header("COMPONENTS")]
    [SerializeField] protected Animator anim;

    [Header("ANIMATION CLIP NAMES")]
    [SerializeField] protected string[] deformationAnimationName;

    [Header("CONDITION")]
    [SerializeField] protected bool playDeformation;

    [Header("VALUE")]
    [SerializeField] protected float stageTwoStartTime;
    [SerializeField] protected float deformationValue;
    [SerializeField] protected bool hasDeformationFinished;
   
    #region IItemDeformation
    public float DeformationValue
    {
        get
        {
            return deformationValue;
        }
        set
        {
            deformationValue = value;
        }
    }
  
    public bool StartDeformation
    {
        get
        {
            return playDeformation;
        }
        set
        {
            playDeformation = value;
        }
    }

    public IMachineController Machine { get; set; }
    public Renderer ItemMeshRend
    {
        get
        {
            if(GetComponent<SkinnedMeshRenderer>() != null)
                return GetComponent<SkinnedMeshRenderer>();

            if (GetComponent<MeshRenderer>() != null)
                return GetComponent<MeshRenderer>();

            else
                return null;
        }
    }

    public bool HasDeformationFinished
    {
        get
        {
            return hasDeformationFinished;
        }
        set
        {
            hasDeformationFinished = value;
        }
    }

    public event Action<bool> OnFinishItemDeformation;
    #endregion

    [SerializeField] protected float[] blendShapesWeight;


    protected void Awake()
    {
        blendShapesWeight = new float[GetComponent<SkinnedMeshRenderer>().sharedMesh.blendShapeCount];
    }

    protected virtual void Update() {

        if (playDeformation) {

            StartCoroutine(AnimationCoroutine());           
        }

        Test();
    }

    protected virtual IEnumerator AnimationCoroutine() {     

        HasDeformationFinished = false;

        if (!anim.enabled) {
            anim.enabled = true;
        }

        if(AnimationName() != null)
        {
            yield return new WaitForSeconds(stageTwoStartTime);

            anim.Play(AnimationName(), 0, DeformationValue);

            yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime == DeformationValue);
            yield return null;          
        }

        yield return null;

        anim.enabled = false;
        playDeformation = false;
        HasDeformationFinished = true;
        OnFinishItemDeformation?.Invoke(HasDeformationFinished);
    }

    protected virtual string AnimationName()
    {
        if (machine_tool == MachineToolController.MACHINE_TOOL.PRERSS_HORIZONTAL)
            return System.Array.Find(deformationAnimationName, item => item == "PressHorizontal");

        if (machine_tool == MachineToolController.MACHINE_TOOL.PRESS_VERTICAL)
            return System.Array.Find(deformationAnimationName, item => item == "PressVertical");

        else
            return null;
    }

    protected void Test()
    {       
        if(blendShapesWeight.Length > 0)
        {
            for (int i = 0; i < GetComponent<SkinnedMeshRenderer>().sharedMesh.blendShapeCount; i++)
            {
                GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(i, blendShapesWeight[i]);
            }            
        }
    }








}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineToolBase : MonoBehaviour, IMachineUIValues 
{
    #region ImachineUIValues INTERFACE
    [Header("EVENTS")]
    [SerializeField] protected TransformBySlider slider;

    [Header("VALUES")]
    [SerializeField] protected float minLimit;
    [SerializeField] protected float maxLimit;

    public event Action<float, float> SetSliderLimitationEvent;
    public virtual TransformBySlider Slider => slider;   
    public virtual float MinLimit {
        get {
            return minLimit;
        }
        set {

        }
    }
    public virtual float MaxLimit {
        get {
            return maxLimit;
        }
        set {

        }
    }
    #endregion

    

    protected virtual void Start() {

        SetSliderLimitationEvent?.Invoke(MinLimit, MaxLimit);
    }

    protected virtual void OnEnable() {

        Slider.OnSliderValueChanged += Slider_OnSliderValueChanged;           
    }
  
    protected virtual void OnDisable() {

        Slider.OnSliderValueChanged -= Slider_OnSliderValueChanged;
    }

    protected virtual void Slider_OnSliderValueChanged(float obj) {

        throw new NotImplementedException();
    }

    










}

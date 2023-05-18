using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TransformBySlider : MonoBehaviour
{
    public event Action<float> OnSliderValueChanged;

    [Header("TRANSFORM")]
    [SerializeField] Transform controllingObj;

    [Header("UI")]
    public Slider slider;
    [SerializeField] Text valueText;

    float SliderMinValue {
        get {
            return slider.minValue;
        }
        set {
            slider.minValue = value;
        }
    }
    float SliderMaxValue {
        get {
            return slider.maxValue;
        }
        set {
            slider.maxValue = value;
        }
    }


    void OnEnable() {

        controllingObj.GetComponent<IMachineUIValues>().SetSliderLimitationEvent += TransformBySlider_SetSliderLimitationEvent;     
    }

    void OnDisable() {

        controllingObj.GetComponent<IMachineUIValues>().SetSliderLimitationEvent -= TransformBySlider_SetSliderLimitationEvent;
    }

    void TransformBySlider_SetSliderLimitationEvent(float minValue, float maxValue) {

        SliderMinValue = minValue;
        SliderMaxValue = maxValue;
    }

    public void SliderValue() {

        OnSliderValueChanged?.Invoke(slider.value);
        valueText.text = slider.value.ToString();
    }











}

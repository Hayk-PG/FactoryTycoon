using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenCameraUIController : MonoBehaviour
{
    public event Action<float> OnRotateCamera;

    [Header("UI")]
    [SerializeField] Slider cameraRotateSlider;
    [SerializeField] Text sliderValueText;

    public float CameraSliderValue
    {
        get
        {
            return cameraRotateSlider.value;
        }
        set
        {
            cameraRotateSlider.value = value;
        }
    }
    public string SliderValueText
    {
        get
        {
            return sliderValueText.text;
        }
        set
        {
            sliderValueText.text = value;
        }
    }


    public void OnSliderValueChanged()
    {
        if(CameraSliderValue < 90)
        {
            CameraSliderValue = 0;
        }
        if(CameraSliderValue > 90 && CameraSliderValue < 180)
        {
            CameraSliderValue = 90;
        }
        if(CameraSliderValue > 180 && CameraSliderValue < 270)
        {
            CameraSliderValue = 180;
        }
        if(CameraSliderValue > 270 && CameraSliderValue < 360)
        {
            CameraSliderValue = 270;
        }

        OnRotateCamera?.Invoke(CameraSliderValue);
        SliderValueText = CameraSliderValue.ToString();
    }

}

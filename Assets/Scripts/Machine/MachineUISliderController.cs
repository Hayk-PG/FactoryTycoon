using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachineUISliderController : MonoBehaviour
{
    [Header("SPRITE")]
    [SerializeField] Sprite[] sliderSprite;
    [SerializeField] Sprite[] sliderButtonSprite;

    [Header("UI")]
    [SerializeField] Slider slider;
    [SerializeField] Button button;
    [SerializeField] Text sliderValueText;

    [Header("COLOR")]
    [SerializeField] Color32 normalColor;
    [SerializeField] Color32 highlightedColor;

    public int SliderIndex
    {
        get
        {
            return transform.parent != null ? transform.GetSiblingIndex() : 0;
        }
    }
    public float SliderValue
    {
        get
        {
            return slider.value;
        }
        set
        {
            slider.value = value;
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

    Sprite SliderSprite
    {
        get
        {
            return slider.transform.Find("Background").GetComponent<Image>().sprite;
        }
        set
        {
            slider.transform.Find("Background").GetComponent<Image>().sprite = value;
        }
    }
    Sprite ButtonSprite
    {
        get
        {
            return button.image.sprite;
        }
        set
        {
            button.image.sprite = value;
        }
    }
    Color SliderColor
    {
        get
        {
            return slider.transform.Find("Background").GetComponent<Image>().color;
        }
        set
        {
            slider.transform.Find("Background").GetComponent<Image>().color = value;
        }
    }
    Color ButtonColor
    {
        get
        {
            return button.image.color;
        }
        set
        {
            button.image.color = value;
        }
    }


    void Awake()
    {
        if(SliderIndex > 0)
        {
            slider.interactable = false;
        }
    }

    void Update()
    {
        UISprite();
        OnClickSliderButton();        
    }

    public void SliderValueChanged()
    {
        SliderValueText = SliderValue.ToString();
    }

    void OnClickSliderButton()
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => 
        {
            SlidersActivity(false);
            button.GetComponentInParent<Slider>().interactable = true;
        });
    }

    void SlidersActivity(bool isActive)
    {
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            transform.parent.GetChild(i).GetComponent<Slider>().interactable = isActive;
        }
    }

    void UISprite()
    {
        if (slider.interactable)
        {
            if (SliderSprite == sliderSprite[0])
                SliderSprite = sliderSprite[1];

            if (SliderColor == normalColor)
                SliderColor = highlightedColor;

            if(ButtonSprite == sliderButtonSprite[0])
                ButtonSprite = sliderButtonSprite[1];

            if (ButtonColor == normalColor)
                ButtonColor = highlightedColor;
        }
        else
        {
            if (SliderSprite == sliderSprite[1])
                SliderSprite = sliderSprite[0];

            if (SliderColor == highlightedColor)
                SliderColor = normalColor;

            if (ButtonSprite == sliderButtonSprite[1])
                ButtonSprite = sliderButtonSprite[0];

            if (ButtonColor == highlightedColor)
                ButtonColor = normalColor;
        }
    }



}

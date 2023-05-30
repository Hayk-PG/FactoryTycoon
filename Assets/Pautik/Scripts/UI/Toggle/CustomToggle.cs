using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomToggle : MonoBehaviour
{
    [Header("Toggle Component")]
    [SerializeField] private Toggle _toggle;

    [Header("Tmp Texts")]
    [SerializeField] private TMP_Text _label;

    [Header("Label")]
    [SerializeField] private string _labelIfOn;
    [SerializeField] private string _labelIfOff;

    [SerializeField] private bool _updateLabelOnValueChange;

    public event Action<bool> OnValueChange;




    private void Awake()
    {
        UpdateLabel(_updateLabelOnValueChange);
    }

    public void OnValueChanged()
    {
        OnValueChange?.Invoke(_toggle.isOn);

        UpdateLabel(_updateLabelOnValueChange);
    }

    private void UpdateLabel(bool updateLabelOnValueChange)
    {
        if (!updateLabelOnValueChange)
        {
            return;
        }

        _label.text = _toggle.isOn ? _labelIfOn : _labelIfOff;
    }
}

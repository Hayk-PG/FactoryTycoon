using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotsUITextController : MonoBehaviour, IGetCanvasGroup
{
    public event Action<Text> OnUpdateRobotPartsValuesFromUITextController;

    [Header("COMPONENTS")]
    [SerializeField] RobotController robotController;
    [SerializeField] NumericPadController numPadController;

    [Header("TEXT")]
    [SerializeField] Text currentValueText;
    [SerializeField] Text robotPartAngleModeText;
    [SerializeField] Text valueText;

    [Header("SEND")]  
    public Button sendButton;
    public bool IsCanvasGroupInteractable
    {
        get
        {
            return GetComponent<CanvasGroup>();
        }
    }

    /// <summary>
    /// Getting pressed robot parts button value text from RobotUIButtonsController
    /// </summary>
    public Text RobotPartsButtonsValueText { get; set; }


    void Awake() {

        robotPartAngleModeText.text = "DP"; 
    }

    void OnEnable()
    {
        numPadController.OnSendNumber += NumPadController_OnSendNumber;
    }

    void OnDisable()
    {
        numPadController.OnSendNumber -= NumPadController_OnSendNumber;
    }
    
    void Update() {

        if (IsCanvasGroupInteractable)
        {
            ParameterText();
        }
    }

    void ParameterText() {

        currentValueText.text = robotController != null ? robotController.selectedPart.GetComponent<RobotPartsParametersBase>().CurrentAngle.ToString() : "Empty";
    }

    void NumPadController_OnSendNumber(float value)
    {
        switch (robotPartAngleModeText.text)
        {
            case "DP": OnEnterValue(value, valueText, "SP", 0); break;
            case "SP": OnEnterValue(value, valueText, "LP", 1); break;
            case "LP": OnEnterValue(value, valueText, "DP", 2); break;
        }

        OnUpdateRobotPartsValuesFromUITextController?.Invoke(RobotPartsButtonsValueText);
        numPadController.Erase();
    }
    
    void OnEnterValue(float value, Text valueText, string text, int index) {

        if(index == 0) { robotController.selectedPart.GetComponent<IRobotParts>().DefaultPosAngle = value; }
        if(index == 1) { robotController.selectedPart.GetComponent<IRobotParts>().SecondPosAngle = value; }
        if(index == 2) { robotController.selectedPart.GetComponent<IRobotParts>().LastPosAngle = value; }

        valueText.text = 0.ToString();
        robotPartAngleModeText.text = text;
    }

    public void ResetInputField() {

        robotPartAngleModeText.text = "DP";
    }







}

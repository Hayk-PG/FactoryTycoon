using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumericPadController : MonoBehaviour
{
    public event Action<float> OnSendNumber;

    /// <summary>
    /// SpawnUI Event
    /// </summary>
    OnGroundTilesSpawnUI tilesSpawnUI;
    bool spawnUIOpen;

    [Header("UI")]
    [SerializeField] Text screenText;
    [SerializeField] Button[] buttons;

    string GetScreenText
    {
        get
        {
            return screenText.text;
        }
        set
        {
            screenText.text = value.Length > 11 ? value.Substring(0, 11) : value;
        }
    }

    [Header("VALUE")]
    string pressedOperatorsSymbol;
    float outFakeNumber;
    List<float> numbersList = new List<float>();

    [Header("CONDITION")]
    bool isOperatorPressed;

    [Header("ANIMATOR")]
    [SerializeField] Animator anim;

    void Awake() {

        tilesSpawnUI = ObjectsHolder.instance.OnGroundTilesSpawnUI;
    }

    void OnEnable() {

        tilesSpawnUI.OnSpawnUiActivity += TilesSpawnUI_OnSpawnUiActivity;
    }

    void OnDisable() {

        tilesSpawnUI.OnSpawnUiActivity -= TilesSpawnUI_OnSpawnUiActivity;
    }

    void Update() {

        if (spawnUIOpen) {

            OnClickButtons();

            TextsDefaultValue();
        }
        else {
            return;
        }
    }

    void TextsDefaultValue() {

        if (GetScreenText.Length < 1) {
            GetScreenText = "0";
        }
    }

    void OnClickButtons() {

        foreach (var button in buttons) {

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => {

                string value = button.GetComponentInChildren<Text>().text;

                PlayValueTextAnimation(value);
                AddNumbersToText(value);
                AddDotToText(value);
                GetResult(value);
                Send(value);
                C(value);
            });
        }
    }

    void AddNumbersToText(string value) {

        if (float.TryParse(value, out outFakeNumber)) {

            if (GetScreenText.Length <= 1 && GetScreenText == "0" || isOperatorPressed) {
                GetScreenText = outFakeNumber.ToString();
                isOperatorPressed = false;
            }
            else {
                GetScreenText += outFakeNumber;
            }
        }
    }

    void AddDotToText(string value) {

        if (value == ".") {
            if (isOperatorPressed) {
                GetScreenText = value;
                isOperatorPressed = false;
            }
            else {
                GetScreenText += value;
            }
        }
    }

    void GetResult(string value) {

        if (!float.TryParse(value, out outFakeNumber) && value != "." && value != "C") {

            if (!isOperatorPressed) {

                isOperatorPressed = true;
                float.TryParse(GetScreenText, out outFakeNumber);

                numbersList.Add(outFakeNumber);

                if (numbersList.Count < 2) {

                }
                else {
                    Operators(value);

                    GetScreenText = numbersList[0].ToString();
                    numbersList.RemoveAt(1);
                }
            }

            if (value != "=") pressedOperatorsSymbol = value;
        }
    }

    void Operators(string _operator)
    {
        if (pressedOperatorsSymbol == "") {
            if (_operator == "+")
                numbersList[0] += numbersList[1];
            if (_operator == "-")
                numbersList[0] -= numbersList[1];
            if (_operator == "*")
                numbersList[0] *= numbersList[1];
            if (_operator == "/")
                numbersList[0] /= numbersList[1];

        }
        if (pressedOperatorsSymbol == "+") {
            if (_operator == "+")
                numbersList[0] += numbersList[1];
            if (_operator == "-")
                numbersList[0] += numbersList[1];
            if (_operator == "*")
                numbersList[0] += numbersList[1];
            if (_operator == "/")
                numbersList[0] += numbersList[1];
            if (_operator == "=") {
                numbersList[0] += numbersList[1];
            }
        }
        if (pressedOperatorsSymbol == "-") {
            if (_operator == "+")
                numbersList[0] -= numbersList[1];
            if (_operator == "-")
                numbersList[0] -= numbersList[1];
            if (_operator == "*")
                numbersList[0] -= numbersList[1];
            if (_operator == "/")
                numbersList[0] -= numbersList[1];
            if (_operator == "=") {
                numbersList[0] -= numbersList[1];
            }
        }
        if (pressedOperatorsSymbol == "*") {
            if (_operator == "+")
                numbersList[0] *= numbersList[1];
            if (_operator == "-")
                numbersList[0] *= numbersList[1];
            if (_operator == "*")
                numbersList[0] *= numbersList[1];
            if (_operator == "/")
                numbersList[0] *= numbersList[1];
            if (_operator == "=") {
                numbersList[0] *= numbersList[1];
            }
        }
        if (pressedOperatorsSymbol == "/") {
            if (_operator == "+")
                numbersList[0] /= numbersList[1];
            if (_operator == "-")
                numbersList[0] /= numbersList[1];
            if (_operator == "*")
                numbersList[0] /= numbersList[1];
            if (_operator == "/")
                numbersList[0] /= numbersList[1];
            if (_operator == "=") {
                numbersList[0] /= numbersList[1];
            }
        }
    }

    void Send(string value) {

        if (value == "SEND") {
            float.TryParse(GetScreenText, out float num);
            OnSendNumber?.Invoke(num);
        }
    }

    void C(string value) {

        if (value == "C") {

            Erase();
        }
    }

    public void Erase()
    {
        GetScreenText = "0";
        numbersList = new List<float>();
        outFakeNumber = 0;
        PlayValueTextAnimation("Erase");
    }

    void PlayValueTextAnimation(string value)
    {
        if (value == "+" || value == "*" || value == "-" || value == "/" || value == "C" || value == "SEND" || value == "Erase")
        {
            anim.SetTrigger("play");
        }
    }

    void TilesSpawnUI_OnSpawnUiActivity(bool isOpen) {
        
        if(spawnUIOpen != isOpen) {
            spawnUIOpen = isOpen;
        }
    }




}

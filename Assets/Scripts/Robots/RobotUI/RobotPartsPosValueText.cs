using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotPartsPosValueText : MonoBehaviour
{
    [SerializeField] Text robotPartsPosText;
    public Text RobotPartsPosText { get { return robotPartsPosText; } }


    void Awake() {

        RobotPartsPosText.text = /*"DP:" + 0 + " SP:" + 0 + " LP:" + 0;*/ "CHECK";
    }








}

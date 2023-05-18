using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachineUIRotate : MonoBehaviour
{
    public event Action<int> OnClickToRotateAnItem;

    [SerializeField] int angle = 0;
    [SerializeField] Text angleText;

    

    void Update()
    {
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(() => 
        {
            angle = angle < 360 ? angle += 90 : 0;
            angleText.text = angle.ToString();           
            OnClickToRotateAnItem?.Invoke(angle);
        });
    }


}

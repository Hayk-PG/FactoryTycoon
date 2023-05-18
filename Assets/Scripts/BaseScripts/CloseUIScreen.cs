using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseUIScreen : MonoBehaviour
{
   public void CloseScreen(GameObject screen) {

        screen.SetActive(false);
    }
}

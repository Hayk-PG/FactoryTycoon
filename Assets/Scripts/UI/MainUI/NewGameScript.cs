using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameScript : MonoBehaviour
{
    [SerializeField] MainUIScriptsHolder mainScriptsHolder;

    internal void NewGame(CanvasGroup mainMenuButtonsContainer) {

        mainMenuButtonsContainer.alpha = 0;
        mainMenuButtonsContainer.interactable = false;
        mainMenuButtonsContainer.blocksRaycasts = false;
    }

}

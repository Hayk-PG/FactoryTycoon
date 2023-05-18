using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// MainMenu buttons controller class
/// </summary>
public class MainMenuButtons : MonoBehaviour
{
    public event System.Action<CanvasGroup> OnClickNewGameButton;
    public event System.Action OnClickLoadGameButton;
    public event System.Action OnClickSaveGameButton;
     
    [Header("MAIN MENU BUTTONS CONTAINER")]
    [SerializeField] Transform mainMenuButtonsContainer;
    public Transform MainMenuButtonsContainer => mainMenuButtonsContainer;

    /// <summary>
    /// MainMenu buttons
    /// <para>0. New game</para>
    /// <para>1. Load game</para>
    /// <para>2. Save game</para>
    /// <para>3. Quit</para>
    /// </summary>
    Button[] Buttons { get; set; }

    void Awake() {

        SetButtonsArray();
    }

    void Update() {

        OnClickButtons(Buttons != null);
    }

    void SetButtonsArray() {

        Buttons = new Button[MainMenuButtonsContainer.childCount];

        for (int i = 0; i < Buttons.Length; i++) {

            Buttons[i] = MainMenuButtonsContainer.GetChild(i)?.GetComponent<Button>();
        }
    }

    void OnClickButtons(bool buttonsArrayIsNotNull) {

        if (buttonsArrayIsNotNull) {

            for (int i = 0; i < Buttons.Length; i++) {

                int index = i;

                Buttons[index].onClick.RemoveAllListeners();
                Buttons[index].onClick.AddListener(() => MainMenuButtonEvents(index));
            }
        }
    }

    void MainMenuButtonEvents(int index) {

        switch (index) {

            case 0: OnClickNewGameButton?.Invoke(MainMenuButtonsContainer.GetComponent<CanvasGroup>()); break;
            case 1: OnClickLoadGameButton?.Invoke(); break;
            case 2: OnClickSaveGameButton?.Invoke(); break;
            case 3: Application.Quit(); break;
        }
        
    }











}

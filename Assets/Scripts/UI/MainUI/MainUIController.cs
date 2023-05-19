using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIController : MonoBehaviour
{
    public static MainUIController instance;

    [SerializeField] MainUIScriptsHolder mainScriptsHolder;


    void Awake() {
        
        if(instance != null) {
            Destroy(gameObject);           
        }
        else {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void OnEnable() {

        mainScriptsHolder.MainMenuButtons.OnClickNewGameButton += MainMenuButtons_OnClickNewGameButton;
        mainScriptsHolder.MainMenuButtons.OnClickLoadGameButton += MainMenuButtons_OnClickLoadButton;
        mainScriptsHolder.MainMenuButtons.OnClickSaveGameButton += MainMenuButtons_OnClickSaveButton;        
    }
   
    void OnDisable() {

        mainScriptsHolder.MainMenuButtons.OnClickNewGameButton -= MainMenuButtons_OnClickNewGameButton;
        mainScriptsHolder.MainMenuButtons.OnClickLoadGameButton -= MainMenuButtons_OnClickLoadButton;
        mainScriptsHolder.MainMenuButtons.OnClickSaveGameButton -= MainMenuButtons_OnClickSaveButton;
    }

    void MainMenuButtons_OnClickNewGameButton(CanvasGroup obj) {

        mainScriptsHolder.NewGameScript.NewGame(obj);

        IEnumerator ChangeSceneCoroutine = mainScriptsHolder.SceneTransition.TransitionCoroutine(null, 2, mainScriptsHolder.SceneTransition.GameScene, 1, () => 
        {
            if(FindObjectOfType<InstantiateGroundTiles>() != null) {
                FindObjectOfType<InstantiateGroundTiles>().CreateTiles(null, null);
            }
        });

        StopAllCoroutines();
        StartCoroutine(ChangeSceneCoroutine);       
    }

    void MainMenuButtons_OnClickLoadButton() {

        IEnumerator ChangeSceneCoroutine = mainScriptsHolder.SceneTransition.TransitionCoroutine(null, 2, mainScriptsHolder.SceneTransition.GameScene, 1, () => {
            if (FindObjectOfType<InstantiateGroundTiles>() != null) {
                mainScriptsHolder.SaveLoadGameObjects.LoadObject();
                mainScriptsHolder.AutoSave.AutoLoad_AutoSavedGameObjectsData();
            }
        });

        StopAllCoroutines();
        StartCoroutine(ChangeSceneCoroutine);
    }

    void MainMenuButtons_OnClickSaveButton() {

        mainScriptsHolder.SaveLoadGameObjects.SaveObject();
    }







}

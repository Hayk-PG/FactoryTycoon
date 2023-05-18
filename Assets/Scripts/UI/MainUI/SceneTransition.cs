using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    /// <summary>
    /// These gameobjects activities depends on which scene is the current scene
    /// </summary>
    [SerializeField] GameObject[] objs;

    /// <summary>
    /// Current scene
    /// </summary>
    internal Scene CurrentScene {

        get {
            return SceneManager.GetActiveScene();
        }
    }

    /// <summary>
    /// Menu scene name
    /// </summary>
    internal string MenuScene => "Menu";

    /// <summary>
    /// Game scene name
    /// </summary>
    internal string GameScene => "Game";
    

    void Update() {

        if (CurrentScene.name == MenuScene) {

            ObjsDuringSceneChanges(false);
        }
        if(CurrentScene.name == GameScene) {

            ObjsDuringSceneChanges(true);
        }        
    }

    /// <summary>
    /// Hide/unhide gameObjects during scene changes
    /// </summary>
    /// <param name="active"></param>
    void ObjsDuringSceneChanges(bool active) {

        if(objs != null && objs[objs.Length - 1].activeInHierarchy != active) {

            foreach (var obj in objs) {

                obj.SetActive(active);
            }
        }
    }

    /// <summary>
    /// Changing the scene, 0: Menu 1: New Game
    /// </summary>
    /// <param name="sceneIndex"></param>
    internal void ChangeScene(string sceneName) {

        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Coroutine that controls scene changing process
    /// </summary>
    /// <param name="waitForSecond"></param>
    /// <param name="waitUntil"></param>
    /// <param name="Code"></param>
    /// <returns></returns>
   internal IEnumerator TransitionCoroutine(Action LodaingScreen, float waitForSecondsToChangeTheScene, string sceneName,float waitForMainFucntion, Action MainFunction) {

        yield return null;
        LodaingScreen?.Invoke();
        yield return new WaitForSeconds(waitForSecondsToChangeTheScene);
        ChangeScene(sceneName);
        yield return new WaitUntil(()=> CurrentScene.name == sceneName);
        yield return new WaitForSeconds(waitForMainFucntion);
        MainFunction?.Invoke();
    }

   


}

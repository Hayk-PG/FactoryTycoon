using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Main UI script components
/// </summary>
public class MainUIScriptsHolder : MonoBehaviour
{
    [SerializeField] MainUIController mainUIController;
    [SerializeField] SceneTransition sceneTransition;
    [SerializeField] GameObjectsPersistenceManager saveLoadGameObjects;
    [SerializeField] MainMenuButtons mainMenuButtons;
    [SerializeField] NewGameScript newGameScript;
    [SerializeField] AutoSaveGameObjectsData autoSave;

 
    internal MainUIController MainUIController => mainUIController;
    internal SceneTransition SceneTransition => sceneTransition;
    internal GameObjectsPersistenceManager SaveLoadGameObjects => saveLoadGameObjects;
    internal MainMenuButtons MainMenuButtons => mainMenuButtons;
    internal NewGameScript NewGameScript => newGameScript;
    internal AutoSaveGameObjectsData AutoSave => autoSave;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIOpenCloseCanvas : OpenCloseCanvas
{
    [Header("CANVAS GROUPS")]
    [SerializeField] CanvasGroup menuButtonsContainerCanvasGroup;
    [SerializeField] CanvasGroup fakeCanvasGroup;

    /// <summary>
    /// Getting menuButtonsContainer gameobject's CanvasGroup, and assigning it to the canvasGroups array's second value, which is inherited from  OpenCloseCanvas class 
    /// </summary>
    CanvasGroup MenuButtonsContainerCanvasGroup {

        get {
            return canvasGroups[1];
        }
        set {
            canvasGroups[1] = value;
        }
    }
    
    /// <summary>
    /// Finding SpawnUI gameobject, and assigning it's CanvasGroup to the canvasGroups array's first value, which is inherited from  OpenCloseCanvas class 
    /// </summary>
    CanvasGroup SpawnUICanvasGroup {

        get {
            return canvasGroups[0];
        }
        set {
            canvasGroups[0] = value;
        }
    }


    protected override void Awake() {

        base.Awake();

        MenuButtonsContainerCanvasGroup = menuButtonsContainerCanvasGroup;
    }

    protected override void Update() {

        base.Update();

        if(FindObjectOfType<InitializeAllSpawnableObjectsButton>() != null) {
            if(SpawnUICanvasGroup != FindObjectOfType<InitializeAllSpawnableObjectsButton>().GetComponent<CanvasGroup>()) {
                SpawnUICanvasGroup = FindObjectOfType<InitializeAllSpawnableObjectsButton>().GetComponent<CanvasGroup>();
            }
        }
        else {
            if(SpawnUICanvasGroup != fakeCanvasGroup) {
                SpawnUICanvasGroup = fakeCanvasGroup;
            }
        }
    }



}

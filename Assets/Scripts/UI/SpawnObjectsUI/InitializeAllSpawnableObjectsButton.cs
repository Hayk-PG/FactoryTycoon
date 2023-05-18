using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitializeAllSpawnableObjectsButton : MonoBehaviour
{
    public delegate void OnClickToChooseObjectToSpawn(ISpawnableObjects Obj);
    public event OnClickToChooseObjectToSpawn OnClickToChooseObject;

    public delegate void OnClickCreateSpawnableObjectsButton(bool isButtonPressed);
    public event OnClickCreateSpawnableObjectsButton OnClickCreateButton;

    [Header("BUTTONS CONTAINER")]
    [SerializeField] Transform ButtonsContainer;

    [Header("BUTTONS")]
    [SerializeField] Button createSpawnableObjectsButton;


    void Start() {

        foreach (ISpawnableObjects obj in ObjectsHolder.instance.SpawnableObjects) {

            obj.CreateRelatedButtonInSpawnUI(ButtonsContainer);
        }
    }

    void Update() {

        OnClickButtonsFromButtonsContainer();

        _OnClickCreateButton();

        if (InEditorMode.IsEnterKeyPressed) {
            OnClickCreateButton?.Invoke(true);
        }        
    }

    void OnClickButtonsFromButtonsContainer() {

        if (ButtonsContainer.childCount >= 1) {

            for (int child = 0; child < ButtonsContainer.childCount; child++) {

                int index = child;

                ButtonsContainer.GetChild(child).GetComponent<Button>().onClick.RemoveAllListeners();
                ButtonsContainer.GetChild(child).GetComponent<Button>().onClick.AddListener(() => OnClickToChooseObject?.Invoke(ObjectsHolder.instance.SpawnableObjects[index]));
            }
        }
    }

    void _OnClickCreateButton() {

        createSpawnableObjectsButton.onClick.RemoveAllListeners();
        createSpawnableObjectsButton.onClick.AddListener(() => OnClickCreateButton?.Invoke(true));
    }


}

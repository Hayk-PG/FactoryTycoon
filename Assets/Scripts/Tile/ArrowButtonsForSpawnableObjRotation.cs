using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowButtonsForSpawnableObjRotation : MonoBehaviour
{
    public delegate void OnArrowButtons(float angle);
    public event OnArrowButtons OnClickArrowButtonsToRotateSpawnableOjects;

    [Header("BUTTONS")]
    [SerializeField] Button[] rotationArrowsButtons;

    /// <summary>
    /// Rotation arrow buttons
    /// <para>Rotate left</para>
    /// <para>Rotate right</para>
    /// </summary>
    public Button[] RotationArrowsButtons => rotationArrowsButtons;


    void Update() {

        if (InEditorMode.IsLeftKeyPressed) {
            OnClickArrowButtonsToRotateSpawnableOjects?.Invoke(-90);
        }
        if (InEditorMode.IsRightKeyPressed) {
            OnClickArrowButtonsToRotateSpawnableOjects?.Invoke(90);
        }

        OnClickToRotateChoosenObj();
    }

    void OnClickToRotateChoosenObj() {

        for (int b = 0; b < RotationArrowsButtons.Length; b++) {

            int index = b;

            RotationArrowsButtons[index].onClick.RemoveAllListeners();
            rotationArrowsButtons[index].onClick.AddListener(() => OnClickArrowButtonsToRotateSpawnableOjects?.Invoke(index == 0 ? -90 : 90));
        }
    }




}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenCloseCanvas : MonoBehaviour
{
    delegate void OnCanvasOpenClose(CanvasGroup canvas);
    OnCanvasOpenClose CacheCanvases { get; set; }
    OnCanvasOpenClose ReopenCachedCanvases { get; set; }


    [SerializeField] protected Button[] buttons;
    [SerializeField] protected CanvasGroup[] canvasGroups;

    protected List<CanvasGroup> canvasesWereAlreadyOpened = new List<CanvasGroup>();

    protected bool[] IsOpened { get; set; }


    protected virtual void Awake() {

        CacheCanvases = CacheAlreadyOpenedCanvases;
        ReopenCachedCanvases = _ReopenCachedCanvases;

        IsOpened = new bool[canvasGroups.Length];
    }

    protected virtual void Update() {

        for (int b = 0; b < buttons.Length; b++) {

            int buttonIndex = b;
            buttons[buttonIndex].onClick.RemoveAllListeners();
            buttons[buttonIndex].onClick.AddListener(() => OnCanvasGroup(canvasGroups[buttonIndex], buttonIndex));
        }
    }
  
    protected void OnCanvasGroup(CanvasGroup canvasGroup, int canvasGroupIndex) {

        IsOpened[canvasGroupIndex] = !IsOpened[canvasGroupIndex];

        if (IsOpened[canvasGroupIndex]) {

            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        else {

            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
       
        OnCanvasOpenClose Method = IsOpened[canvasGroupIndex] ? CacheCanvases : ReopenCachedCanvases;
        Method(canvasGroup);
    }

    void CacheAlreadyOpenedCanvases(CanvasGroup canvas) {

        foreach (CanvasGroup otherCanvas in FindObjectsOfType<CanvasGroup>()) {

            if (otherCanvas != canvas && otherCanvas.alpha >= 1) {

                if (!canvasesWereAlreadyOpened.Contains(otherCanvas)) {

                    canvasesWereAlreadyOpened.Add(otherCanvas);
                    otherCanvas.alpha = 0;
                    otherCanvas.interactable = false;
                    otherCanvas.blocksRaycasts = false;
                }
            }
        }
    }

    void _ReopenCachedCanvases(CanvasGroup canvas) {

        for (int c = 0; c < canvasesWereAlreadyOpened.Count; c++) {

            canvasesWereAlreadyOpened[c].alpha = 1;
            canvasesWereAlreadyOpened[c].interactable = true;
            canvasesWereAlreadyOpened[c].blocksRaycasts = true;
            canvasesWereAlreadyOpened.Remove(canvasesWereAlreadyOpened[c]);
        }
    }








}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RobotsUIButtonsController : MonoBehaviour, IIndirectControll,IGetCanvasGroup
{
    OnGroundTilesSpawnUI onGroundTileSpawnUI;

    [Header("COMPONENTS")]
    [SerializeField] RobotsUITextController robotsUiText;

    public delegate void OnClickRobotParts(int index);
    public event OnClickRobotParts _OnClickRobotParts;

    public delegate void OnClickRobotPartsUpdatePosValues(Text text);
    public event OnClickRobotPartsUpdatePosValues _OnClickRobotPartsUpdatePosValues;

    public delegate void OnRotateRobotParts(bool isPointerOn);
    public delegate void OnResetTransform();
    public event OnRotateRobotParts OnIncreaseRotAngle;
    public event OnRotateRobotParts OnDecreaseRotAngle;
    public event OnResetTransform OnClickResetPartTransform;

    public delegate void OnClickToControlRobot(bool on);
    public event OnClickToControlRobot OnControlRobot;
    public event OnClickToControlRobot OnManualCatch;
    public event OnClickToControlRobot OnRobotManualMode;

    public delegate void OnTemplates(int buttonIndex);
    public event OnTemplates OnClickTemplates;

    public delegate void OnTemplatesParameters();
    public event OnTemplatesParameters OnSaveTemplate;
    //public event OnTemplatesParameters OnLoadTemplate;

    public event System.Action<GameObject> OnLoadTemplate;
    public event System.Action<GameObject> OnClickTemplatesClose;

    [Header("BUTTONS")]
    [SerializeField] Button[] partsButtons;
    [SerializeField] Button increaseRot, decreaseRot, resetTransform;
    [SerializeField] Button[] robotActivityButtons;
    [SerializeField] Button catchButton, releaseButton;
    [SerializeField] Button[] templateButtons;
    [SerializeField] Button resetButton;
    [SerializeField] Button[] save_Load_Close_TemplateButtons;

    [Header("CLOSE THE SCREEN")]
    public Button closeTheScreen;

    [Header("BUTTONS SPRITES")]
    [SerializeField] Sprite pressedButtonSprite;
    [SerializeField] Sprite nonPressedButtonSprite;
    [SerializeField] Sprite pressedButtonSprite45x45;
    [SerializeField] Sprite nonPressedButtonSprite45x45;

    [Header("BUTTONS TEXT COLOR")]
    [SerializeField] Color32 pressedButtonColor;
    [SerializeField] Color32 nonPressedButtonColor;

    /// <summary>
    /// Robot starts/stopes rotating by clicking start/stop button
    /// </summary>
    bool ClickedToRotate { get; set; }

    /// <summary>
    /// Stopped by IIndirect Stop()
    /// </summary>
    bool RobotIndirectStop { get; set; }

    /// <summary>
    /// Break OnSpawnUiActivity
    /// </summary>
    bool isOpen { get; set; }

    public bool IsCanvasGroupInteractable
    {
        get
        {
            return GetComponent<CanvasGroup>();
        }
    }

    void Awake() {

        onGroundTileSpawnUI = ObjectsHolder.instance.OnGroundTilesSpawnUI;

        robotsUiText.RobotPartsButtonsValueText = partsButtons[0].GetComponent<RobotPartsPosValueText>().RobotPartsPosText;
    }

    void OnEnable() {

        onGroundTileSpawnUI.OnSpawnUiActivity += OnGroundTileSpawnUI_OnSpawnUiActivity;
    }

    void OnDisable() {

        onGroundTileSpawnUI.OnSpawnUiActivity -= OnGroundTileSpawnUI_OnSpawnUiActivity;
    }

    void Update() {

        if (IsCanvasGroupInteractable)
        {
            OnCloseTheScreenButton();

            OnClickPartButtons();

            OnClickPartResetTransformButton();

            OnClickStartStopButtons();

            OnClickCatchReleaseButtons();

            _OnClickTemplateButtons();

            OnClickSaveLoadButtons();
        }
    }

    void OnCloseTheScreenButton()
    {
        closeTheScreen.onClick.RemoveAllListeners();
        closeTheScreen.onClick.AddListener(() => {

            GetComponent<CanvasGroup>().alpha = 0;
            GetComponent<CanvasGroup>().interactable = false;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        });
    }

    void OnClickPartButtons() {

        for (int i = 0; i < partsButtons.Length; i++) {

            int index = i;

            partsButtons[i].onClick.RemoveAllListeners();
            partsButtons[i].onClick.AddListener(() => 
            {
                _OnClickRobotParts?.Invoke(index);
                robotsUiText.RobotPartsButtonsValueText = partsButtons[index].GetComponent<RobotPartsPosValueText>().RobotPartsPosText;
                _OnClickRobotPartsUpdatePosValues?.Invoke(partsButtons[index].GetComponent<RobotPartsPosValueText>().RobotPartsPosText);
                robotsUiText.ResetInputField(); ButtonsSprite(partsButtons, partsButtons[index], nonPressedButtonSprite, pressedButtonSprite, true, true);
            });
        }
    }

    void ButtonsSprite(Button[] buttons, Button pressedButton, Sprite nonPressedButtonSprite, Sprite pressedButtonSprite, bool changeSprite, bool includeTexts) {


        foreach (var button in buttons) {

            if (changeSprite)
                button.image.sprite = nonPressedButtonSprite;
            else
                button.image.color = nonPressedButtonColor;

            if (includeTexts)
            {
                foreach (var text in button.GetComponentsInChildren<Text>())
                {
                    text.color = nonPressedButtonColor;
                }
            }
        }

        if (changeSprite)
            pressedButton.image.sprite = pressedButtonSprite;
        else
            pressedButton.image.color = nonPressedButtonColor;

        if (includeTexts)
        {
            foreach (var text in pressedButton.GetComponentsInChildren<Text>())
            {
                text.color = pressedButtonColor;
            }
        }
    }

    void OnClickPartResetTransformButton() {

        resetTransform.onClick.RemoveAllListeners();
        resetTransform.onClick.AddListener(() => OnClickResetPartTransform?.Invoke());
    }

    void OnClickStartStopButtons() {

        for (int i = 0; i < robotActivityButtons.Length; i++)
        {
            int index = i;
            robotActivityButtons[index].onClick.RemoveAllListeners();

            if(index == 0)
            {
                robotActivityButtons[index].onClick.AddListener(() => { OnControlRobot?.Invoke(true); ButtonsSprite(robotActivityButtons, robotActivityButtons[index], nonPressedButtonSprite45x45, pressedButtonSprite45x45, true, true); OnRobotManualMode?.Invoke(false); ClickedToRotate = true; });
            }
            else
            {
                robotActivityButtons[index].onClick.AddListener(() => { OnControlRobot?.Invoke(false); ButtonsSprite(robotActivityButtons, robotActivityButtons[index], nonPressedButtonSprite45x45, pressedButtonSprite45x45, true, true); OnRobotManualMode?.Invoke(true); ClickedToRotate = false; });
            }
        }
    }

    void OnClickCatchReleaseButtons() {

        catchButton.onClick.RemoveAllListeners();
        catchButton.onClick.AddListener(() => { OnManualCatch?.Invoke(true); OnRobotManualMode?.Invoke(true); });

        releaseButton.onClick.RemoveAllListeners();
        releaseButton.onClick.AddListener(() => { OnManualCatch?.Invoke(false); OnRobotManualMode?.Invoke(true); });
    }

    void _OnClickTemplateButtons() {

        for (int t = 0; t < templateButtons.Length; t++) {

            int index = t;
            templateButtons[index].onClick.RemoveAllListeners();
            templateButtons[index].onClick.AddListener(() => { OnClickTemplates?.Invoke(index); { save_Load_Close_TemplateButtons[0].transform.parent.gameObject.SetActive(true); } });
        }
    }

    void OnClickSaveLoadButtons() {

        for (int i = 0; i < save_Load_Close_TemplateButtons.Length; i++) {

            int index = i;
            save_Load_Close_TemplateButtons[index].onClick.RemoveAllListeners();

            if(index == 0) {
                save_Load_Close_TemplateButtons[index].onClick.AddListener(() => OnSaveTemplate?.Invoke());
            }
            if(index == 1) {
                save_Load_Close_TemplateButtons[index].onClick.AddListener(() => OnLoadTemplate?.Invoke(save_Load_Close_TemplateButtons[index].transform.parent.gameObject));
            }
            if(index == 2) {
                save_Load_Close_TemplateButtons[index].onClick.AddListener(() => OnClickTemplatesClose?.Invoke(save_Load_Close_TemplateButtons[index].transform.parent.gameObject));
            }
        }
    }

    public void OnPointerDown(GameObject buttonName) {

        if (buttonName.name == increaseRot.name) { OnIncreaseRotAngle?.Invoke(true); }
        if (buttonName.name == decreaseRot.name) { OnDecreaseRotAngle?.Invoke(true); }
    }

    public void OnPointerUp(GameObject buttonName) {

        if (buttonName.name == increaseRot.name) { OnIncreaseRotAngle?.Invoke(false); }
        if (buttonName.name == decreaseRot.name) { OnDecreaseRotAngle?.Invoke(false); }
    }

    /// <summary>
    /// Stops robot rotation (IIndirect)
    /// </summary>
    public void Stop() {

        if (ClickedToRotate) {

            OnControlRobot?.Invoke(false);
            OnRobotManualMode?.Invoke(true);
            RobotIndirectStop = true;
        }
        else {
            return;
        }
    }

    /// <summary>
    /// Starts robot rotation (IIndirect)
    /// </summary>
    public void Run() {

        if (ClickedToRotate) {

            OnControlRobot?.Invoke(true);
            OnRobotManualMode?.Invoke(false);
            RobotIndirectStop = false;
        }
        else {
            return;
        }
    }

    void OnGroundTileSpawnUI_OnSpawnUiActivity(bool isOpened) {

        if (isOpened && !isOpen && ClickedToRotate) {

            OnControlRobot?.Invoke(false);
            OnRobotManualMode?.Invoke(true);

            isOpen = true;
        }
        if (!isOpened && isOpen && ClickedToRotate && !RobotIndirectStop) {

            Run();
            isOpen = false;
        }
    }







}

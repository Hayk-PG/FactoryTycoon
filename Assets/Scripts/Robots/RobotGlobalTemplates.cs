using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotGlobalTemplates : MonoBehaviour
{
    public enum TEMPLATES { TEMPLATE_1, TEMPLATE_2, TEMPLATE_3}
    public TEMPLATES templates;

    [Header("OBJECT REFERENCE FOR ROBOT")]
    [SerializeField] ObjectsReferenceForRobot objReferenceForRobot;

    Dictionary<int, List<float>> template_1_params = new Dictionary<int, List<float>>();
    Dictionary<int, List<float>> template_2_params = new Dictionary<int, List<float>>();
    Dictionary<int, List<float>> template_3_params = new Dictionary<int, List<float>>();
    public Dictionary<int, List<float>> Template_1_params => template_1_params;
    public Dictionary<int, List<float>> Template_2_params => template_2_params;
    public Dictionary<int, List<float>> Template_3_params => template_3_params;


    void OnEnable() {

        objReferenceForRobot.RobotUiButtonsController.OnClickTemplates += RobotUiButtonsController_OnClickTemplates;
        objReferenceForRobot.RobotUiButtonsController.OnSaveTemplate += RobotUiButtonsController_OnSaveTemplate;
        objReferenceForRobot.RobotUiButtonsController.OnLoadTemplate += RobotUiButtonsController_OnLoadTemplate;
        objReferenceForRobot.RobotUiButtonsController.OnClickTemplatesClose += RobotUiButtonsController_OnClickTemplatesClose;
    }

    void OnDisable() {

        objReferenceForRobot.RobotUiButtonsController.OnClickTemplates -= RobotUiButtonsController_OnClickTemplates;
        objReferenceForRobot.RobotUiButtonsController.OnSaveTemplate -= RobotUiButtonsController_OnSaveTemplate;
        objReferenceForRobot.RobotUiButtonsController.OnLoadTemplate -= RobotUiButtonsController_OnLoadTemplate;
        objReferenceForRobot.RobotUiButtonsController.OnClickTemplatesClose -= RobotUiButtonsController_OnClickTemplatesClose;
    }

    //Click on template button
    //Click on save template button to save template datas dictionary to json
    //Intizialize template dictionary
    //Load template datas from json
    //Close templates screen

    void RobotUiButtonsController_OnClickTemplates(int buttonIndex) {
       
        if(buttonIndex < 3) {

            templates = (TEMPLATES)buttonIndex;
        }       
    }

    void RobotUiButtonsController_OnSaveTemplate() {

        switch (templates) {

            case TEMPLATES.TEMPLATE_1: Templates(Template_1_params, 0); break;
            case TEMPLATES.TEMPLATE_2: Templates(Template_2_params, 1); break;
            case TEMPLATES.TEMPLATE_3: Templates(Template_3_params, 2); break;
        }
    }

    void Templates(Dictionary<int, List<float>> templateDataDictionary, int templatesIndex) {

        //Clear dictionary
        //Add into the dictionary
        //Initialize ISaveRobotTemplatesParameters interface array of dictionaries

        templateDataDictionary.Clear();

        for (int i = 0; i < objReferenceForRobot.RobotController.robotMovingParts.Length; i++) {

            RobotPartsParametersBase angle = objReferenceForRobot.RobotController.robotMovingParts[i].GetComponent<RobotPartsParametersBase>();
            List<float> dataList = new List<float>() { angle.DefaultPosAngle, angle.SecondPosAngle, angle.LastPosAngle };

            templateDataDictionary.Add(i, dataList);
        }

        ObjectsHolder.instance.SaveRobotTemplates.GetComponent<ISaveRobotGlobalTemplatesParameters>().RobotTemplatesArray[templatesIndex] = templateDataDictionary;
        ObjectsHolder.instance.SaveRobotTemplates.SaveTemplates(templatesIndex);
    }

    void RobotUiButtonsController_OnLoadTemplate(GameObject obj) {

        int index = templates == TEMPLATES.TEMPLATE_1 ? 0 : templates == TEMPLATES.TEMPLATE_2 ? 1 : 2;

        ObjectsHolder.instance.SaveRobotTemplates.LoadTemplateFromJSON(index);

        Dictionary<int, List<float>> DictionaryFromRobotTemplatesArray = ObjectsHolder.instance.SaveRobotTemplates.GetComponent<ISaveRobotGlobalTemplatesParameters>().RobotTemplatesArray[index];
        RobotController robotController = objReferenceForRobot.RobotController;

        if(DictionaryFromRobotTemplatesArray != null) {

            foreach (var dictionary in DictionaryFromRobotTemplatesArray) {

                print(dictionary.Value[0] + "/" + dictionary.Value[1] + "/" + dictionary.Value[2]);

                robotController.robotMovingParts[dictionary.Key].GetComponent<RobotPartsParametersBase>().DefaultPosAngle = dictionary.Value[0];
                robotController.robotMovingParts[dictionary.Key].GetComponent<RobotPartsParametersBase>().SecondPosAngle = dictionary.Value[1];
                robotController.robotMovingParts[dictionary.Key].GetComponent<RobotPartsParametersBase>().LastPosAngle = dictionary.Value[2];
            }
        }

        obj.SetActive(false);
    }

    void RobotUiButtonsController_OnClickTemplatesClose(GameObject obj) {

        obj.SetActive(false);
    }

  






}

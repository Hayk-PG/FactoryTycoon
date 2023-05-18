using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSaveGameObjectsData : MonoBehaviour
{
    #region SaveItemInstantiatePlatesData

    const string SaveInstPlateDataFileName = "InstPlateData";
    public SaveItemInstantiatePlatesData SaveInstPlateData;

    [Serializable]
    public class SaveItemInstantiatePlatesData {

        public float SliderValue;
        public Vector3 InstPlateLocalPos;
        public Vector3 InstPlateLocalEulerAngles;

        public SaveItemInstantiatePlatesData(float SliderValue, Vector3 InstPlateLocalPos, Vector3 InstPlateLocalEulerAngles) {

            this.SliderValue = SliderValue;
            this.InstPlateLocalPos = InstPlateLocalPos;
            this.InstPlateLocalEulerAngles = InstPlateLocalEulerAngles;
        }
    }

    void SaveItemInstantiatePlates() {

        if(FindObjectsOfType<Machine>().Length > 0) {

            SaveItemInstantiatePlatesData[] SaveData = new SaveItemInstantiatePlatesData[FindObjectsOfType<Machine>().Length];

            for (int i = 0; i < FindObjectsOfType<Machine>().Length; i++) {

                Machine machine = FindObjectsOfType<Machine>()[i];
                SaveData[i] = new SaveItemInstantiatePlatesData(machine.SliderValue, machine.LocalPosition, machine.LocalEulerAngles);
            }

            JSON_BaseController.SaveToJSON(SaveData, SaveInstPlateDataFileName, null);
        }      
    }

    void LoadItemInstantiatePlates() {

        if (FindObjectsOfType<Machine>().Length > 0) {

            SaveItemInstantiatePlatesData[] LoadData = null;

            JSON_BaseController.LoadFromJSON(SaveInstPlateDataFileName, null, out LoadData, () => {

                for (int i = 0; i < LoadData.Length; i++) {

                    Vector3 localPos = LoadData[i].InstPlateLocalPos;
                    Vector3 localEulerAngles = LoadData[i].InstPlateLocalEulerAngles;
                    float sliderValue = LoadData[i].SliderValue;

                    for (int o = 0; o < FindObjectsOfType<Machine>().Length; o++) {

                        if(FindObjectsOfType<Machine>()[o].LocalPosition == localPos) {
                            FindObjectsOfType<Machine>()[o].SliderValue = sliderValue;
                        }
                    }
                }
            });
        }
    }

    #endregion

    void OnDisable() {

        SaveItemInstantiatePlates();
    }

    public void AutoLoad_AutoSavedGameObjectsData() {

        LoadItemInstantiatePlates();
    }










}

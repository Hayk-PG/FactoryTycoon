using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using System.Linq;

public class SaveRoboGlobalTemplates : MonoBehaviour, ISaveRobotGlobalTemplatesParameters {

    const string fileName = "rob_temp";

    /// <summary>
    /// An array of dictionaries of robot parts global parameters
    /// </summary>
    public Dictionary<int, List<float>>[] RobotTemplatesArray { get; set; }

    void Awake() {

        RobotTemplatesArray = new Dictionary<int, List<float>>[3];       
    }

    void Start() {

        //Load saved dictionaries from JSON at the start of the game
        LoadTemplateFromJSON(0);
        LoadTemplateFromJSON(1);
        LoadTemplateFromJSON(2);
    }

    /// <summary>
    /// Save templates to json
    /// </summary>
    /// <param name="index"></param>
    public void SaveTemplates(int index) {

        DataClass[] DataClass = new DataClass[RobotTemplatesArray[index].Count];

        foreach (var dictionary in RobotTemplatesArray[index]) {

            DataClass[dictionary.Key] = new DataClass(dictionary.Value[0], dictionary.Value[1], dictionary.Value[2]);
        }

        JSON_BaseController.SaveToJSON(DataClass, fileName, index.ToString());
    }

    /// <summary>
    /// Load templates from json
    /// </summary>
    /// <param name="index"></param>
    public void LoadTemplateFromJSON(int index) {

        RobotTemplatesArray[index] = new Dictionary<int, List<float>>();

        DataClass[] DataClass = null;

        JSON_BaseController.LoadFromJSON(fileName, index.ToString(), out DataClass, delegate {

            for (int i = 0; i < DataClass.Length; i++) {

                List<float> dataList = new List<float>() { DataClass[i].def, DataClass[i].sec, DataClass[i].last };
                RobotTemplatesArray[index].Add(i, dataList);
            }
        });      
    }
  
    [Serializable]
    public class DataClass {

        public float def;
        public float sec;
        public float last;

        public DataClass(float def, float sec, float last) {

            this.def = def;
            this.sec = sec;
            this.last = last;
        }
    }
















}

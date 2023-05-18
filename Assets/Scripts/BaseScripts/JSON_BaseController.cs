using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JSON_BaseController 
{
    /// <summary>
    /// File name
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    static string GetPath(string fileName) {

        return Application.persistentDataPath + "/" + fileName;
    }

    /// <summary>
    /// Save an array to json.file: Index(optional)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="fileName"></param>
    public static void SaveToJSON<T>(T[] data, string fileName, string index) {

        string json = JsonHelper.ToJson<T>(data, true);
        FileStream file = new FileStream(GetPath(fileName + index), FileMode.Create);
        using (StreamWriter writer = new StreamWriter(file)) {

            writer.Write(json);
        }
    }

    /// <summary>
    /// Save to json.file: Index(optional) 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="fileName"></param>
    /// <param name="index"></param>
    public static void SaveToJSON<T>(T data, string fileName, string index) {

        string json = JsonUtility.ToJson(data);
        FileStream file = new FileStream(GetPath(fileName + index), FileMode.Create);
        using (StreamWriter writer = new StreamWriter(file)) {

            writer.Write(json);
        }
    }

    /// <summary>
    /// Load saved datas from json into the scene: Initializing SavedDatas array with all datas, and sending back 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fileName"></param>
    /// <param name="SavedObjectsDataClass"></param>
    /// <param name="ManipulateWithSavedDatas"></param>
    public static void LoadFromJSON<T>(string fileName, string index, out T[] SavedDatas, Action ManipulateWithSavedDatas) {

        SavedDatas = null;

        if (File.Exists(GetPath(fileName + index))) {

            using (StreamReader reader = new StreamReader(GetPath(fileName + index))) {

                string json = reader.ReadToEnd();

                SavedDatas = JsonHelper.FromJson<T>(json);

                ManipulateWithSavedDatas();
            }
        }
    }
    
    /// <summary>
    /// Json custom made helper: Saves arrays and lists to json
    /// </summary>
    public static class JsonHelper {
        public static T[] FromJson<T>(string json) {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array) {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint) {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [Serializable]
        private class Wrapper<T> {
            public T[] Items;
        }
    }

    













}

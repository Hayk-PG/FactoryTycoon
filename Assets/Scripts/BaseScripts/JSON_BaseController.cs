using System;
using System.IO;
using UnityEngine;

public static class JSON_BaseController 
{
    public static string GetPath(string fileName) 
    {
        return Application.persistentDataPath + "/" + fileName;
    }

    public static void SaveToJSON<T>(T[] data, string fileName, string index) 
    {
        string json = JsonHelper.ToJson<T>(data, true);
        FileStream file = new FileStream(GetPath(fileName + index), FileMode.Create);
        using (StreamWriter writer = new StreamWriter(file)) 
        {
            writer.Write(json);
        }
    }

    public static void SaveToJSON<T>(T data, string fileName, string index) 
    {
        string json = JsonUtility.ToJson(data);
        FileStream file = new FileStream(GetPath(fileName + index), FileMode.Create);
        using (StreamWriter writer = new StreamWriter(file)) 
        {
            writer.Write(json);
        }
    }

    public static void LoadFromJSON<T>(string fileName, string index, out T[] data, Action Execute) {

        data = null;

        if (File.Exists(GetPath(fileName + index))) {

            using (StreamReader reader = new StreamReader(GetPath(fileName + index)))
            {
                string json = reader.ReadToEnd();
                data = JsonHelper.FromJson<T>(json);
                Execute();
            }
        }
    }
    
    public static class JsonHelper 
    {
        public static T[] FromJson<T>(string json) 
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array) 
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint) 
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [Serializable]
        private class Wrapper<T> 
        {
            public T[] Items;
        }
    }
}

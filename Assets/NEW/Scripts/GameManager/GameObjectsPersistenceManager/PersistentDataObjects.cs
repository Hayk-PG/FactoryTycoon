using UnityEngine;
using System;

[Serializable]
public class PersistentDataObjects : MonoBehaviour
{
    public int Index { get; private set; }
    public string ObjectName { get; private set; }
    public string ParentName { get; private set; }
    public Vector3 LocalPosition { get; private set; }
    public Vector3 LocalEulerAngles { get; private set; }




    public PersistentDataObjects(int index, string objectName, string parentName, Vector3 localPosition, Vector3 localEulerAngles)
    {
        Index = index;
        ObjectName = objectName;
        ParentName = parentName;
        LocalPosition = localPosition;
        LocalEulerAngles = localEulerAngles;
    }
}

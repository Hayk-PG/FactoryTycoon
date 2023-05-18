using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAutoSaveData 
{
    Vector3 LocalPosition { get; set; }
    Vector3 LocalEulerAngles { get; set; }
}

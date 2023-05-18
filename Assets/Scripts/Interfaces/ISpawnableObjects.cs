using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawnableObjects
{
    /// <summary>
    /// Has this object spawned
    /// </summary>
    bool GotSpawned { get; set; }

    /// <summary>
    /// Can other gameObject spawn on this gameObject
    /// </summary>
    bool CanSpawnTheObject { get; set; }

    /// <summary>
    /// Can this object get spawned
    /// </summary>
    bool CanThisGameObjectGetSpawned { get; set;  }

    /// <summary>
    /// This is the Vector3 point, where choosen gameobject will spawn 
    /// </summary>
    Vector3 OnThisGameObjectSpawnPosition { get; set; }

    UnityEngine.UI.Button CreateRelatedButtonInSpawnUI(Transform parent);
}

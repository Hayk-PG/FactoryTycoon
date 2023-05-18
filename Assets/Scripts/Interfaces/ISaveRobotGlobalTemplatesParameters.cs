using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveRobotGlobalTemplatesParameters 
{
    /// <summary>
    /// An array of dictionaries of robot parts global parameters
    /// </summary>
    Dictionary<int, List<float>>[] RobotTemplatesArray { get; set; }
}

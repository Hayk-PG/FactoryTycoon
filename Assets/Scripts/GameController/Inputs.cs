using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs : MonoBehaviour
{
    /// <summary>
    /// Left mouse button is clicked once
    /// </summary>
    public bool IsLeftMouseClickedOnce { get { return Input.GetMouseButtonDown(0); } }

    /// <summary>
    /// Left mouse button is clicked and holded
    /// </summary>
    public bool IsLeftMouseHolded { get { return Input.GetMouseButton(0); } }

    /// <summary>
    /// Right mouse button is clicked once
    /// </summary>
    public bool IsRightMouseClickedOnce { get { return Input.GetMouseButtonDown(1); } }
}

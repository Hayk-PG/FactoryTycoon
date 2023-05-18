using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InEditorMode
{
    /// <summary>
    /// Enter key 
    /// </summary>
    public static bool IsEnterKeyPressed {
        get {
            return Input.GetKeyDown(KeyCode.Return);
        }
    }

    /// <summary>
    /// Q key
    /// </summary>
    public static bool IsLeftKeyPressed {
        get {
            return Input.GetKeyDown(KeyCode.Q);
        }
    }

    /// <summary>
    /// E key
    /// </summary>
    public static bool IsRightKeyPressed {
        get {
            return Input.GetKeyDown(KeyCode.E);
        }
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISideTile 
{
    /// <summary>
    /// Checks if arrow button overlaps with tile, and gets destroyed ISideTile Interface
    /// </summary>
    bool ArrowButtonIsDestroyed { get; set; }

    /// <summary>
    /// Create arrow canvas ISideTile interface 
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="rot"></param>
    /// <param name="arrowName"></param>
    /// <param name="parent"></param>
    void CreateArrowCanvas(Vector3 pos, Quaternion rot, string arrowName, Transform parent);

    /// <summary>
    /// On click side tiles arrow button ISideTile interface 
    /// </summary>
    /// <param name="arrowCanvasExists"></param>
    /// <param name="OnClickButton"></param>
    void OnClickArrowCanvasButton(bool arrowCanvasExists, System.Action OnClickButton);

    /// <summary>
    /// Controls arrow canvas activity
    /// </summary>
    /// <param name="isActive"></param>
    void ArrowCanvasActivity(bool isActive);
}

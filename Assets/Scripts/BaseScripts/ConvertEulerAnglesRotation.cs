using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvertEulerAnglesRotation : MonoBehaviour
{
    protected float ConvertedAxis(float axis) {

        axis = axis > 180 ? axis - 360 : axis;
        return axis;
    }


















}

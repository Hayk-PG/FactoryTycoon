using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenCameraController : MonoBehaviour
{
    [SerializeField] ScreenCameraUIController camUIController;

    void OnEnable()
    {
        camUIController.OnRotateCamera += CamUIController_OnRotateCamera;
    }

    void OnDisable()
    {
        camUIController.OnRotateCamera -= CamUIController_OnRotateCamera;
    }

    void CamUIController_OnRotateCamera(float obj)
    {
        transform.localEulerAngles = new Vector3(0, obj, 0);
    }
}

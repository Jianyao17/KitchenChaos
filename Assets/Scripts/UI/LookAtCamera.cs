using System;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private enum UIMode
    {
        LookAtCamera,
        LookAtCameraInverted,
        CameraForward,
        CameraForwardInverted,
    }

    [SerializeField] private UIMode mode;
    
    private void LateUpdate()
    {
        switch (mode)
        {
            case UIMode.LookAtCamera:
                transform.LookAt(Camera.main.transform);
                break;
            case UIMode.LookAtCameraInverted:
                var dirFromCamera = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + dirFromCamera);
                break;
            case UIMode.CameraForward:
                transform.forward = Camera.main.transform.forward;
                break;
            case UIMode.CameraForwardInverted:
                transform.forward = -Camera.main.transform.forward;
                break;
        }
    }
}
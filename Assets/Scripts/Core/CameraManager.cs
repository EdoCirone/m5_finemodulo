using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private List<Cinemachine.CinemachineVirtualCamera> virtualCameras;
    private int defaultCameraIndex = 0;
    private int currentCameraIndex = 0;

    void Start()
    {
        UpdateCameraPriority();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            currentCameraIndex = (currentCameraIndex + 1) % virtualCameras.Count;
            UpdateCameraPriority();
        }
    }

    void UpdateCameraPriority()
    {
        for (int i = 0; i < virtualCameras.Count; i++)
        {
            virtualCameras[i].Priority = (i == currentCameraIndex) ? 10 : 0;
        }
    }
}

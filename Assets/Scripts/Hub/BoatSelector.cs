using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatSelector : MonoBehaviour
{
    [SerializeField] CinemachineBrain _brain;
    [SerializeField] CinemachineVirtualCamera[] _virtualCameras;
    int _currentCameraIndex;
    CinemachineVirtualCamera _currentCamera;
    private void Awake()
    {
        foreach (var cam in _virtualCameras)
        {
            cam.gameObject.SetActive(false);
        }
        _currentCameraIndex = 0;
        _currentCamera = _virtualCameras[_currentCameraIndex];
        _currentCamera.gameObject.SetActive(true);
    }
    public void SwitchCamera(bool next)
    {
        _currentCameraIndex += next ? 1 : _virtualCameras.Length - 1;
        _currentCameraIndex %= _virtualCameras.Length;
        _currentCamera.gameObject.SetActive(false);
        _currentCamera = _virtualCameras[_currentCameraIndex];
        _currentCamera.gameObject.SetActive(true);
    }
}

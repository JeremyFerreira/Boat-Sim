using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoatSelector : MonoBehaviour
{
    [SerializeField] CinemachineBrain _brain;
    [SerializeField] CinemachineVirtualCamera[] _virtualCameras;
    [SerializeField] CinemachineVirtualCamera[] _boatVirtualCameras;
    int _currentCameraIndex;
    CinemachineVirtualCamera _currentCamera;
    [SerializeField] GameObject[] _buttonsNavigate;
    private void Awake()
    {
        foreach (var cam in _virtualCameras)
        {
            cam.gameObject.SetActive(false);
        }
        foreach (var button in _buttonsNavigate)
        {
            button.SetActive(false);
        }
        _buttonsNavigate[0].SetActive(true);
        _currentCameraIndex = 0;
        _currentCamera = _virtualCameras[_currentCameraIndex];
        _currentCamera.gameObject.SetActive(true);
    }
    public void SwitchCamera(bool next)
    {
        _buttonsNavigate[_currentCameraIndex].SetActive(false);
        _currentCameraIndex += next ? 1 : _virtualCameras.Length - 1;
        _currentCameraIndex %= _virtualCameras.Length;
        _currentCamera.gameObject.SetActive(false);
        _currentCamera = _virtualCameras[_currentCameraIndex];
        _currentCamera.gameObject.SetActive(true);
        _buttonsNavigate[_currentCameraIndex].SetActive(true);

    }
    public void OpenScene(int index)
    {
        _currentCamera?.gameObject.SetActive(false);
        _boatVirtualCameras[_currentCameraIndex].gameObject.SetActive(true);

        StartCoroutine(WaitForEndOfBlend(index));
    }
    IEnumerator WaitForEndOfBlend(int index)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(index);
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            if (async.progress >= 0.9f)
            {
                if(!_brain.IsBlending)
                {
                    async.allowSceneActivation = true;
                }
            }
            yield return null;
        }
        while (!_brain.IsBlending)
        {
            Debug.Log("yo");
            yield return null;
        }
        async.allowSceneActivation = true;
        
    }
}

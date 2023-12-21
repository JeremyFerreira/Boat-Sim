using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoatSelector : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera _startingCamera;
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

        _startingCamera.gameObject.SetActive(true);
        _currentCameraIndex = 0;
    }

    private void Start()
    {
        StartCoroutine(WwaitForEndOfBlendUI());
    }
    public void SwitchCamera(bool next)
    {
        Debug.Log("je passe");
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
        yield return new WaitForSeconds(1.5f);
        while (!_brain.IsBlending)
        {
            yield return null;
        }
       // SceneManager.UnloadSceneAsync(0);
        //SceneManager.LoadScene(index);
        async.allowSceneActivation = true;

    }

    IEnumerator WwaitForEndOfBlendUI ()
    {
        _currentCamera = _virtualCameras[(int)Mathf.Repeat(_currentCameraIndex, _virtualCameras.Length)];
        _currentCamera.gameObject.SetActive(true);
        _startingCamera.gameObject.SetActive(false);
        do
        {
            yield return new WaitForEndOfFrame(); 
        } while (_brain.IsBlending) ;
        _buttonsNavigate[(int)Mathf.Repeat(_currentCameraIndex, _virtualCameras.Length)].SetActive(true);
    }
}

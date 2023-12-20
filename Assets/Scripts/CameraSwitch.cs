using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField]
    private InputButtonScriptableObject _cameraSwitch;
    [SerializeField]
    private List<GameObject> _cameras;
    [SerializeField]
    private float _coolDownInSec;

    private int index;
    private bool _canActive;
    private void OnEnable()
    {
        _cameraSwitch.OnValueChanged += SwitchCamera;
    }

    private void OnDisable()
    {
        _cameraSwitch.OnValueChanged -= SwitchCamera;
    }

    // Start is called before the first frame update
    void Start()
    {
        index = 0;
        _canActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SwitchCamera (bool value)
    {
        if(!_canActive) { return; }
        _canActive = false;
        _cameras[index].SetActive(false);
        index++;
        index = (int)Mathf.Repeat(index, _cameras.Count);

        _cameras[index].SetActive(true);
        StartCoroutine(CoroutineCoolDown());
    }

    IEnumerator CoroutineCoolDown ()
    {
        yield return new WaitForSeconds(_coolDownInSec);
        _canActive = true;
    }


}

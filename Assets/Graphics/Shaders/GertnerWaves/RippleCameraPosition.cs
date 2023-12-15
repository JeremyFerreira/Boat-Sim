using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleCameraPosition : MonoBehaviour
{
    [SerializeField] private GameObject rippleCamera;
    [SerializeField] private float _cameraHeight;

    // Update is called once per frame
    void Update()
    {
        rippleCamera.transform.position = transform.position + Vector3.up * _cameraHeight;
        Shader.SetGlobalVector("_PlayerPos", rippleCamera.transform.position);
        //ripples.Emit(transform.position + transform.forward, transform.forward, 2, 3, Color.white);
    }
    [EasyButtons.Button]
    private void PlaceCamera()
    {
        rippleCamera.transform.position = transform.position + Vector3.up * _cameraHeight;
        Shader.SetGlobalVector("_PlayerPos", rippleCamera.transform.position);
        //ripples.Emit(transform.position + transform.forward, transform.forward, 2, 3, Color.white);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class VoileController : MonoBehaviour
{
    [SerializeField] private WindSO _windData;
    [SerializeField] private Slider _voileAngleSlider;
    [SerializeField] private float _voileMaxAngle;
    [SerializeField] private float _voileAngle;
    [SerializeField] private Transform[] _voileTransforms;
    [SerializeField] private Toggle _voileAutoToggle;
    [SerializeField] private Slider _voileOpenSlider;
    [SerializeField] private Animator _voileAnimator;
    [SerializeField] private VoilePhysic _voilePhysic;

    [SerializeField]
    private MoveBoatElement _moveSallingElement;
    [SerializeField]
    private MoveBoatElement _rotateSallingElement;
    private void OnEnable()
    {
        _voileOpenSlider.onValueChanged.AddListener(OpenVoile);
        _voileAngleSlider.onValueChanged.AddListener(RotateVoileBasedOnInput01);

        _moveSallingElement._eventMoveElement += OpenVoile;
        _rotateSallingElement._eventMoveElement += RotateVoileBasedOnInput01;
        _moveSallingElement.OnEnableFunc();
        _rotateSallingElement.OnEnableFunc();
    }
    private void OnDisable()
    {
        _voileOpenSlider.onValueChanged.RemoveListener(OpenVoile);
        _voileAngleSlider.onValueChanged.RemoveListener(RotateVoileBasedOnInput01);

        _moveSallingElement.OnDisableFunc();
        _rotateSallingElement.OnDisableFunc();
        _moveSallingElement._eventMoveElement -= OpenVoile;
        _rotateSallingElement._eventMoveElement -= RotateVoileBasedOnInput01;
    }
    void Start()
    {
        _voileAnimator = GetComponent<Animator>();
        //OpenVoile(0f);
    }
    [EasyButtons.Button]
    public void OpenVoile(float value)
    {
        Debug.Log(value);
        _voileAnimator.Play("VoileOpen", 0, value);
        _voilePhysic.VoileSurfaceMultiplier = value;
    }
    private void Update()
    {
        if(_voileAutoToggle.isOn)
        {
            RotateVoileBasedOnWindDirection();
        }
        else
        {
            _rotateSallingElement.UpdateFunc();
        }
        _moveSallingElement.UpdateFunc();
    }
    void RotateVoileBasedOnWindDirection()
    {
        Vector3 flatForward = MathExtension.FlatVectorXZ(transform.forward);
        Vector3 windLeft = Vector3.Cross(_windData.WindDirection, Vector3.up);
        float angleBetweenForwardAndWind = MathExtension.AngleBetweenVectorsInDeg(flatForward, _windData.WindDirection);
        if(Vector3.Dot(flatForward,windLeft)>0)
        {
            angleBetweenForwardAndWind *= -1;
        }
        float angle = Mathf.Clamp(angleBetweenForwardAndWind, -_voileMaxAngle, _voileMaxAngle);
        Vector3 direction = MathExtension.RotateVectorOnXZPlane(MathExtension.FlatVectorXZ(transform.forward), angle * Mathf.Deg2Rad);
        Vector3 normalDirection = Vector3.Cross(direction, Vector3.up);
        Vector3 voileForward = Vector3.Cross(transform.up, normalDirection);
        foreach (Transform t in _voileTransforms)
        {
            if(voileForward!=Vector3.zero)
            {
                t.transform.rotation = Quaternion.Slerp(t.transform.rotation,Quaternion.LookRotation(voileForward, transform.up),Time.deltaTime *10f);
            }
        }
    }
    void RotateVoileBasedOnInput01(float value)
    {
        float angle = Mathf.Lerp(-_voileMaxAngle, _voileMaxAngle, 1 - value);
        Vector3 direction = MathExtension.RotateVectorOnXZPlane(MathExtension.FlatVectorXZ(transform.forward), angle * Mathf.Deg2Rad);
        Vector3 normalDirection = Vector3.Cross(direction, Vector3.up);
        Vector3 voileForward = Vector3.Cross(transform.up, normalDirection);
        foreach (Transform t in _voileTransforms)
        {
            t.transform.rotation = Quaternion.LookRotation(voileForward, transform.up);
        }
    }
}

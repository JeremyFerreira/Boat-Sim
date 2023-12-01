using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoileController : MonoBehaviour
{
    [SerializeField] private WindSO _windData;
    [SerializeField] private Slider _voileSlider;
    [SerializeField] private float _voileMaxAngle;
    [SerializeField] private float _voileAngle;
    [SerializeField] private Transform[] _voileTransforms;
    [SerializeField] private Toggle _voilAutoToggle;
    private void OnEnable()
    {
        _voileSlider.onValueChanged.AddListener(RotateVoileBasedOnInput01);

    }
    private void OnDisable()
    {
        _voileSlider.onValueChanged.RemoveListener(RotateVoileBasedOnInput01);
    }
    private void Update()
    {
        if(_voilAutoToggle.isOn)
        {
            RotateVoileBasedOnWindDirection();
        }
    }
    void RotateVoileBasedOnWindDirection()
    {
        Vector3 windNormal = Vector3.Cross(_windData.WindDirection, Vector3.up);
        Vector3 voileForward = Vector3.Cross(transform.up, windNormal);
        foreach (Transform t in _voileTransforms)
        {
            t.transform.rotation = Quaternion.LookRotation(voileForward, transform.up);
        }
    }
    void RotateVoileBasedOnInput01(float value)
    {
        if (!_voilAutoToggle.isOn)
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
}

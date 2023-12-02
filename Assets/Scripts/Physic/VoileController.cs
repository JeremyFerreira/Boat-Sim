using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoileController : MonoBehaviour
{
    [SerializeField] private WindSO _windData;
    [SerializeField] private Slider _voileAngleSlider;
    [SerializeField] private float _voileMaxAngle;
    [SerializeField] private float _voileAngle;
    [SerializeField] private Transform[] _voileTransforms;
    [SerializeField] private Toggle _voileAutoToggle;
    private void Update()
    {
        if(_voileAutoToggle.isOn)
        {
            RotateVoileBasedOnWindDirection();
        }
        else
        {
            RotateVoileBasedOnInput01(_voileAngleSlider.value);
        }
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
            t.transform.rotation = Quaternion.Slerp(t.transform.rotation,Quaternion.LookRotation(voileForward, transform.up),Time.deltaTime *10f);
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

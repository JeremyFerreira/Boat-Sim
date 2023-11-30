using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GouvernailController : MonoBehaviour
{
    [SerializeField] private Rigidbody _boatRigidbody;
    [SerializeField] private float _maxGouvernailAngle;
    [SerializeField] private float _rotationForce;
    [SerializeField] private Transform _gouvernailTransform;
    private float _gouvernailAngle;
    [SerializeField] private Slider _sliderGouvernail;
    // Start is called before the first frame update
    void Start()
    {
        _sliderGouvernail.onValueChanged.AddListener(SetGouvernailAngle);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //gouvernail
        //normal direction of gouvernail
        Vector3 gouvernailNormal = _gouvernailAngle - 180 > 0 ? _gouvernailTransform.right : -_gouvernailTransform.right;
        float magnitudeForce = Vector3.Dot(transform.forward, gouvernailNormal);
        Vector3 forceDirection = _gouvernailAngle - 180 > 0 ? -transform.right : transform.right;
        _boatRigidbody.AddForceAtPosition(_rotationForce * forceDirection * magnitudeForce, _gouvernailTransform.position);
    }
    void SetGouvernailAngle(float value)
    {
        _gouvernailAngle = Mathf.Deg2Rad * Mathf.Lerp(-_maxGouvernailAngle, _maxGouvernailAngle, value);
        _gouvernailTransform.transform.rotation = Quaternion.LookRotation(MathExtension.RotateVectorOnXZPlane(transform.forward, _gouvernailAngle), transform.up);
    }
}

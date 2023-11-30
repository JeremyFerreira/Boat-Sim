using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.Controls.AxisControl;

public class VoilePhysic : MonoBehaviour
{
    [SerializeField] private float _windForceMultiplier;
    [SerializeField] private float _voileFlatSurface;
    [SerializeField] private Rigidbody _boatRigidbody;
    [SerializeField] private WindController _windController;
    private void OnValidate()
    {
        _windController = FindAnyObjectByType<WindController>();
    }
    float GetForceByWind()
    {
        return GetVoileSurfaceBasedOnWindDirection() * GetForceMultiplierByWindDirection() * _windController.GetWindSpeed();
    }
    //the amount of surface that will be effective by wind
    float GetVoileSurfaceBasedOnWindDirection()
    {
        return Mathf.Clamp01(Vector3.Dot(_windController.GetWindDirection(),GetVoileDirection()));
    }
    float GetForceMultiplierByWindDirection()
    {
        return _windForceMultiplier * (Vector3.Dot(_boatRigidbody.transform.forward, _windController.GetWindDirection())+1)/2f;
    }
    Vector3 GetVoileDirection()
    {
        return transform.forward;
    }
    private void FixedUpdate()
    {
        _boatRigidbody.AddForce(_boatRigidbody.transform.forward * GetForceByWind(),ForceMode.Acceleration);
        transform.forward = _windController.GetWindDirection();
    }
}

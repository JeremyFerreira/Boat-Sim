using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoilePhysic : MonoBehaviour
{
    [SerializeField] private float _windForceMultiplier;
    [SerializeField] private float _voileFlatSurface;
    [SerializeField] private Rigidbody _boatRigidbody;
    [SerializeField] private WindSO _windData;
    [SerializeField] private Transform[] _voileTransforms;
    
    float GetForceByWind()
    {
        return GetVoileSurfaceBasedOnWindDirection() * GetForceMultiplierByWindDirection() * _windData.WindForce;
    }
    //the amount of surface that will be effective by wind
    float GetVoileSurfaceBasedOnWindDirection()
    {
        return Mathf.Clamp01(Vector3.Dot(_windData.WindDirection,GetVoileDirection())) * _voileFlatSurface;
    }
    float GetForceMultiplierByWindDirection()
    {
        return _windForceMultiplier * (Vector3.Dot(_boatRigidbody.transform.forward, _windData.WindDirection) + 1)/2f;
    }
    Vector3 GetVoileDirection()
    {
        return _voileTransforms[0].forward;
    }
    private void FixedUpdate()
    {
        _boatRigidbody.AddForce(_boatRigidbody.transform.forward * GetForceByWind(),ForceMode.Acceleration);
    }
    
}

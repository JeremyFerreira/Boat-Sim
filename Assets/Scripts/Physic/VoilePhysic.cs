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
    private float _voileSurfaceMultiplier;
    public float VoileSurfaceMultiplier { get { return _voileSurfaceMultiplier;} set { _voileSurfaceMultiplier = value;} }

    [SerializeField] private float _maxSpeed;


    float GetForceByWind()
    {
        if(new Vector3(_boatRigidbody.velocity.x, 0, _boatRigidbody.velocity.z).magnitude * 3.6f > _maxSpeed)
        {
            return 0;
        }
        return GetVoileSurfaceBasedOnWindDirection() * GetForceMultiplierByWindDirection() * _windData.WindForce;
    }
    //the amount of surface that will be effective by wind
    float GetVoileSurfaceBasedOnWindDirection()
    {
        return Mathf.Clamp((Vector3.Dot(_windData.WindDirection,GetVoileDirection())+1)/2f,0.2f,1) * _voileFlatSurface * _voileSurfaceMultiplier;
    }
    float GetForceMultiplierByWindDirection()
    {
        return _windForceMultiplier * /*(Vector3.Dot(_boatRigidbody.transform.forward, _windData.WindDirection) + 1)/2f*/ 1;
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

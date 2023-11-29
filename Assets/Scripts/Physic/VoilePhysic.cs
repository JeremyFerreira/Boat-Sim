using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoilePhysic : MonoBehaviour
{
    [SerializeField] private float _windForceMultiplier;
    [SerializeField] private float _voileFlatSurface;
    [SerializeField] private Rigidbody _boatRigidbody;
    float GetForceByWind()
    {
        return 1;
    }
    private void FixedUpdate()
    {
        _boatRigidbody.AddForce(transform.forward * GetForceByWind() * _windForceMultiplier,ForceMode.Acceleration);
    }
}

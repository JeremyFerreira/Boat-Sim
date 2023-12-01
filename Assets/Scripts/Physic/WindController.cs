using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindController : MonoBehaviour
{
    [Header("properties")]
    [SerializeField] float maxWindSpeed;
    [SerializeField] float directionChangeRate;
    [SerializeField] float linearAngularAcceleration;
    [SerializeField] float windAcceleration;
    [SerializeField] float randomAccelerationWindSpeed;
    float windAngle;
    Vector3 windDirection;
    float windForce01;
    float windForce;

    [Header("References")]
    [SerializeField] WindSO _windData;
    [SerializeField] Transform boat;
    

    //Getters
    public float GetWindSpeed() { return windForce; }
    public Vector3 GetWindDirection() 
    {
        if (windDirection != Vector3.zero)
        { 
            return windDirection.normalized;
        }
        else 
        { 
            windDirection = Vector3.forward;
            return Vector3.forward;
        }
    }

    void Update()
    {
        CalculateWindDirection();
        CalculateWindSpeed();
    }
    void CalculateWindDirection()
    {
        windAngle += linearAngularAcceleration * (Mathf.PerlinNoise(boat.position.x * directionChangeRate, boat.position.z * directionChangeRate) - 0.5f) / 2;
        _windData.WindAngle = windAngle;
        windDirection = MathExtension.RotateVectorOnXZPlane(Vector3.forward, windAngle);
        _windData.WindDirection = windDirection;
    }
    void CalculateWindSpeed()
    {
        windForce01 += windAcceleration * (Mathf.PerlinNoise1D(randomAccelerationWindSpeed * Time.time) - 0.5f) / 2f;
        windForce01 = Mathf.Clamp(windForce01, 0.5f, 1);
        windForce = Mathf.Lerp(0, maxWindSpeed, windForce01);
        _windData.WindForce = windForce01;
    }
}

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
    float windSpeed01;
    float windSpeed;
    
    [Header("References")]
    [SerializeField] Transform boat;
    Transform cameraTransform;
    [SerializeField] RectTransform windImage;

    //Getters
    public float GetWindSpeed() { return windSpeed; }
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

    void Awake()
    { 
        cameraTransform = Camera.main.transform;
    }
    void Update()
    {
        CalculateWindDirection();
        UpdtateWindInterface();
        CalculateWindSpeed();
    }
    void CalculateWindDirection()
    {
        windAngle += linearAngularAcceleration * (Mathf.PerlinNoise(boat.position.y * directionChangeRate, boat.position.z * directionChangeRate) - 0.5f) / 2;
        windDirection = MathExtension.RotateVectorOnXZPlane(Vector3.forward, windAngle);
    }
    void UpdtateWindInterface()
    {
        windImage.eulerAngles = new Vector3(0, 0, windAngle * Mathf.Rad2Deg + cameraTransform.eulerAngles.y);
        windImage.localScale = Vector3.one*windSpeed01;
    }
    void CalculateWindSpeed()
    {
        windSpeed01 += windAcceleration * (Mathf.PerlinNoise1D(randomAccelerationWindSpeed * Time.time) - 0.5f) / 2f;
        windSpeed01 = Mathf.Clamp(windSpeed01, 0.5f, 1);
        windSpeed = Mathf.Lerp(0, maxWindSpeed, windSpeed01);
    }
}

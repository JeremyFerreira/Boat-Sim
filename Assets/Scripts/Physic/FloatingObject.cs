using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FloatingObject : MonoBehaviour
{
    [SerializeField] private WaterElevation _waterElevation;
    [SerializeField] private FloatingPoint[] _floatingPoints;
    [SerializeField] private float _floatingForce;
    [SerializeField] private float _waterLinearDrag;
    [SerializeField] private float _airLinearDrag;
    [SerializeField] private float _waterAngularDrag;
    [SerializeField] private float _airAngularDrag;
    private Rigidbody _objectRigidBody;
    [SerializeField] private float _averageHeight;
    [SerializeField] private Transform _centerOfmass;

    private void OnValidate()
    {
        _objectRigidBody = GetComponent<Rigidbody>();
        _waterElevation = FindAnyObjectByType<WaterElevation>();
        _floatingPoints = GetComponentsInChildren<FloatingPoint>();

    }
    // Start is called before the first frame update
    void Awake()
    {
        _objectRigidBody = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        _objectRigidBody.centerOfMass = _centerOfmass.localPosition;
        foreach (FloatingPoint floatingPoint in _floatingPoints)
        {
            float heightBetweenPointAndWater = _waterElevation.GetElevation(floatingPoint.transform.position.x, floatingPoint.transform.position.z) - floatingPoint.transform.position.y;
            if (heightBetweenPointAndWater > 0)
            {
                _objectRigidBody.AddForceAtPosition(Vector3.up * Mathf.Sqrt(heightBetweenPointAndWater) * _floatingForce / _floatingPoints.Length, floatingPoint.transform.position, ForceMode.Acceleration);
            }
        }
        //calculate Drag
        float ratioMassUnderwater = GetRatioMassUnderWater();
        float ratioMassInAir = 1-ratioMassUnderwater;
        float verticalDrag = Mathf.Lerp(_waterLinearDrag,_airLinearDrag, ratioMassInAir);
        _objectRigidBody.velocity = new Vector3(_objectRigidBody.velocity.x, _objectRigidBody.velocity.y * verticalDrag, _objectRigidBody.velocity.z);
        _objectRigidBody.angularDrag =  ratioMassUnderwater * _waterAngularDrag + ratioMassInAir * _airAngularDrag;
    }

    //Big Approximation! (would be cool to calculate based on rotation)
    float GetRatioMassUnderWater()
    {
        float centerOfmassDisatnceToWaterSurface = _waterElevation.GetElevation(transform.position.x, transform.position.z) - _objectRigidBody.worldCenterOfMass.y;
        float halfHeight = _averageHeight / 2f;
        centerOfmassDisatnceToWaterSurface = Mathf.Clamp(centerOfmassDisatnceToWaterSurface, -halfHeight, halfHeight);
        
        return Mathf.Lerp(0,1, ((centerOfmassDisatnceToWaterSurface/halfHeight)+1)/2f);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        foreach (FloatingPoint floatingPoint in _floatingPoints)
        {
            float heightBetweenPointAndWater = -(floatingPoint.transform.position.y - _waterElevation.GetElevation(floatingPoint.transform.position.x, floatingPoint.transform.position.z));
            if (heightBetweenPointAndWater > 0)
            {
                heightBetweenPointAndWater = Mathf.Sqrt(heightBetweenPointAndWater);
            }
            else
            {
                heightBetweenPointAndWater = -Mathf.Sqrt(-heightBetweenPointAndWater);
            }
            Vector3 directionForce = _waterElevation.GetNormal(floatingPoint.transform.position.x, floatingPoint.transform.position.z) * heightBetweenPointAndWater;
            Gizmos.DrawLine(floatingPoint.transform.position, floatingPoint.transform.position + directionForce*5);
        }
#if UNITY_EDITOR
        Handles.Label(transform.position+Vector3.up,GetRatioMassUnderWater().ToString());
#endif 
    }
}

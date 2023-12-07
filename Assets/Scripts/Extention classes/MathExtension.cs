using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathExtension
{
    public static float TAU = 6.2831855f;
    public static Vector3 RotateVectorOnXZPlane(Vector3 vectorToRotate, float angleInRad)
    {
        return new Vector3(vectorToRotate.x * Mathf.Cos(angleInRad) - vectorToRotate.z * Mathf.Sin(angleInRad), 0f, vectorToRotate.z * Mathf.Cos(angleInRad) + vectorToRotate.x * Mathf.Sin(angleInRad)).normalized * vectorToRotate.magnitude;
    }
    public static Vector3 RotateVectorOnXYPlane(Vector3 vectorToRotate, float angle)
    {
        return new Vector3(vectorToRotate.x * Mathf.Cos(angle) - vectorToRotate.y * Mathf.Sin(angle), vectorToRotate.y * Mathf.Cos(angle) + vectorToRotate.x * Mathf.Sin(angle),0f).normalized * vectorToRotate.magnitude;
    }
    public static float AngleBetweenVectorsInDeg(Vector3 vector1, Vector3 vector2)
    {
        //The angle between two vectors a and b is found with the formula angle = cos-1 [ (a. b) / (|a| |b|) ]
        float angle = Mathf.Acos(Vector3.Dot(vector1,vector2)/(vector1.magnitude*vector2.magnitude));
        return angle*Mathf.Rad2Deg;
    }
    public static float AngleBetweenVectors(Vector3 vector1, Vector3 vector2)
    {
        //The angle between two vectors a and b is found with the formula angle = cos-1 [ (a. b) / (|a| |b|) ]
        float angle = Mathf.Acos(Vector3.Dot(vector1, vector2) / (vector1.magnitude * vector2.magnitude));
        return angle;
    }
    public static Vector3 FlatVectorXZ(Vector3 vector)
    {
        return new Vector3(vector.x, 0,vector.z);
    }
    public static Vector3 FlatVectorXY(Vector3 vector)
    {
        return new Vector3(vector.x,vector.y, 0f);
    }
    public static Vector3 GetPositionAtDistanceOfPoint(Vector3 point, Vector3 destinationPoint, float distance) 
    {
        Vector3 direction = (destinationPoint - point).normalized;
        return destinationPoint - direction * distance;
    }
    public static float HyperbolicTangent(float x)
    {
        return (Mathf.Exp(x)-Mathf.Exp(-x))/(Mathf.Exp(x) + Mathf.Exp(-x));
    }
}

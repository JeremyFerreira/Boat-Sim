using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "WindData")]
public class WindSO : ScriptableObject
{
    private Vector3 _windDirection = Vector3.forward;
    public Vector3 WindDirection { get { return _windDirection; } set { _windDirection = value.normalized; OnWindDirectionValueChanged?.Invoke(_windDirection); } }
    private float _windForce = 0.5f;
    public float WindForce { get { return _windForce; } set { _windForce = Mathf.Clamp01(value); OnWindForceValueChanged?.Invoke(_windForce); } }
    private float _windAngle = 0;
    public float WindAngle { get { return _windAngle; } set { _windAngle = value; } }
    public UnityAction<float> OnWindForceValueChanged;
    public UnityAction<Vector3> OnWindDirectionValueChanged;
}

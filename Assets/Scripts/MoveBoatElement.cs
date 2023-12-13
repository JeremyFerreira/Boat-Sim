using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class MoveBoatElement
{

    [SerializeField]
    private InputFloatScriptableObject _inputForMove;
    public Action<float> _eventMoveElement;
    [SerializeField]
    [Range(0.1f, 1f)]
    private float _maxSpeed;
    [SerializeField]
    private float _acceleration;

    private float _percent;
    private float _speed;
    private float _way;
    float refVel = 0;
    public MoveBoatElement ()
    {
        _percent = 0f;
        _speed = 0f;
        _way = 0;
    }
    public void OnEnableFunc()
    {
        _inputForMove.OnValueChanged += SetWay;
    }

    public void OnDisableFunc()
    {
        _inputForMove.OnValueChanged -= SetWay;
    }

    private void SetWay(float way)
    {
        _way = way;
    }

    public int UpdateFunc()
    {
        if (_way != 0 || (_way == 0 && _speed != 0))
        {
            CalculateSpeed(_way);
            CalculatePercent();

            _eventMoveElement?.Invoke(_percent);
            return 1;
        }
        return 0;
    }

    private void CalculatePercent()
    {
        _percent = Mathf.Clamp(_percent + _maxSpeed * _speed * Time.deltaTime, 0, 1);
    }
    private void CalculateSpeed(float way)
    {
        if (way != 0)
        {
            _speed = Mathf.Round(Mathf.SmoothDamp(_speed, way, ref refVel, _acceleration) * 100)/100;
            return;
        }
        else
        {
            _speed = Mathf.SmoothDamp(_speed, 0, ref refVel, _acceleration);
            if(Mathf.Abs(_speed) < 0.001) { _speed = 0; }
        }
        
    }
}

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
    private float _maxSpeed;

    private float _percent;
    private float _speed;
    private float _way;

    public MoveBoatElement ()
    {
        _percent = 0.5f;
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
        if (_way == way) { return; }
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

 /*   IEnumerator CoroutineMoveRudder()
    {
        CalculateSpeed(_way);
        CalculatePercentRudder();

        _eventMoveElement?.Invoke(_percentRudder);

        if (_speed == 0 && _way == 0)
        {
            yield return null; 
        }

        yield return new WaitForEndOfFrame();

        StartCoroutine(CoroutineMoveRudder());
    }*/

    private void CalculatePercent()
    {
        _percent = Mathf.Clamp(_percent + _maxSpeed * _speed * Time.deltaTime, 0, 1);
    }
    private void CalculateSpeed(float way)
    {
        if (way != 0)
        {
            _speed = Mathf.Clamp(_speed + (Time.deltaTime * way), -1, 1);
            return;
        }
        else
        {
            if (_speed > 0)
            {
                _speed = Mathf.Clamp(_speed - Time.deltaTime, 0, 1);
                return;
            }
            _speed = Mathf.Clamp(_speed + Time.deltaTime, -1, 0);
        }
    }
}

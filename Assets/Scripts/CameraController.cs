using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using System;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private WaterElevation _waterEvaluation;
    [SerializeField]
    public CinemachineVirtualCamera _virtualCamera;
    [HideInInspector]
    public CinemachineTransposer _transposer;
    [HideInInspector]
    public CinemachineComposer _composer;

    [SerializeField]
    private Vector2 _minForceMouse;
    [SerializeField]
    private Vector3 _offsetCenter;
    [SerializeField]
    private OffsetTracketObject _offsetTracketObject;

    [Space]
    [Header("ZOOMCAMERA")]
    [SerializeField]
    private InputFloatScriptableObject _zoomCamera;
    [SerializeField]
    private FloatData_SO _distanceBoat;
    [SerializeField]
    public float _distanceMax, _distanceMin;
    [SerializeField]
    private float _fovMax, _fovMin;
    [SerializeField]
    [Range(0, 100)]
    public float _actualDistance;
    private float _targetDistance;
    [SerializeField]
    private float _accelerationDistance;
    [SerializeField]
    private float _speedDistance;

    [Space]
    [Header("MOVECAMERA")]
    [SerializeField]
    private InputVectorScriptableObject _moveCamera;
    [SerializeField]
    [Range(-90f, 90f)]
    public float _minLatitude, _maxLatitude;
    [SerializeField]
    [Range(-90f, 90f)]
    public float _actualLatitude;
    [SerializeField]
    [Range(-180f, 180f)]
    public float _actualLongitude;
    private Vector2 _targetAngels;
    [SerializeField]
    private float _accelerationAngels;
    [SerializeField]
    private float _speedAngels;
    [SerializeField]
    private float _speedLatitudeMultiply;
    [SerializeField]
    private float _offsetAboveWater;

    [Space]
    [Header("FOV")]
    [SerializeField]
    private FOVHandel _fovHandel;

    private void Start()
    {
        _transposer = _virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        _composer = _virtualCamera.GetCinemachineComponent<CinemachineComposer>();

        _targetDistance = _actualDistance;
        _targetAngels = new Vector2(_actualLongitude, _actualLatitude);
        _offsetTracketObject.Init();
    }

    private void OnEnable()
    {
        _zoomCamera.OnValueChanged += CalculTargetDistance;
        _moveCamera.OnValueChanged += CalculTargetAngels;
    }

    private void OnDisable()
    {
        _zoomCamera.OnValueChanged -= CalculTargetDistance;
        _moveCamera.OnValueChanged -= CalculTargetAngels;
    }

    private void FixedUpdate()
    {
        CalculateActualDistance(_targetDistance);
        CalculateActualAngels(_targetAngels);
        SetFov();
        SetTrackedOffset();
        SetCameraPostion(_actualDistance / _distanceMax, _actualLatitude / _maxLatitude, _actualLongitude / 180f);
    }

    public void SetComponentRef()
    {
        _transposer = _virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        _composer = _virtualCamera.GetCinemachineComponent<CinemachineComposer>();
    }

    [EasyButtons.Button]
    public void SetCameraPosition()
    {
        SetComponentRef();
        _distanceBoat.SetValue = _actualDistance;
        SetCameraPostion(_actualDistance / _distanceMax, _actualLatitude / _maxLatitude, _actualLongitude / 180f);
    }

    public float GetActualDistance()
    {
        Vector3 lookAtPoint_WP = _virtualCamera.LookAt.transform.position + _composer.m_TrackedObjectOffset;

        return Vector3.Distance(lookAtPoint_WP, _virtualCamera.transform.position);
    }

    /// <summary>
    /// Fonction qui retourne un point sur une sphere grace a trois parametre
    /// </summary>
    /// <param name="distance">Value between 0 and 1</param>
    /// <param name="latitude">Value between 0 and 1</param>
    /// <param name="longitude">Value between 0 and 1</param>
    /// <returns>Point sur sphere</returns>
    private Vector3 FindPointWithAngels(float distance, float latitude, float longitude)
    {
        latitude = Mathf.Clamp((latitude * _maxLatitude), _minLatitude, _maxLatitude);
        longitude = Mathf.Repeat((longitude * 180f) +180f, 360f) - 180f;
        distance = Mathf.Clamp((distance * _distanceMax), _distanceMin, _distanceMax);

        latitude *= Mathf.Deg2Rad;
        longitude *= Mathf.Deg2Rad;
        float x = distance * Mathf.Cos(latitude) * Mathf.Cos(longitude);
        float y = distance * Mathf.Sin(latitude);
        float z = distance * Mathf.Cos(latitude) * Mathf.Sin(longitude);

        return new Vector3(x, y, z);
    }

    private void SetCameraPostion(float distance, float latitude, float longitude)
    {
        Vector3 localPosition = FindPointWithAngels(distance, latitude, longitude);
        float yWater = _waterEvaluation.GetElevation(localPosition.x, localPosition.y);
        if (localPosition.y - _offsetAboveWater < yWater)
        {
           localPosition.y = yWater + _offsetAboveWater;
        }
        _transposer.m_FollowOffset = localPosition + _offsetCenter;
    }

    public void SetFov()
    {
        //float newFOV = Mathf.Lerp(_fovMin, _fovMax, _actualDistance / _distanceMax);
        _virtualCamera.m_Lens.FieldOfView = _fovHandel.GetFOv;
    }

    public void SetTrackedOffset ()
    {
        _composer.m_TrackedObjectOffset = _offsetTracketObject.CalculateOffsetTracketObject(_composer.m_TrackedObjectOffset, _actualDistance, _actualLongitude, _actualLatitude);
    }


    #region SMOOTHDAMP FUNC DISTANCE ANGLES
    float refVelDistance = 0;
    private void CalculateActualDistance (float target)
    {
        _actualDistance = Mathf.SmoothDamp(_actualDistance, target, ref refVelDistance, _accelerationDistance);
        _distanceBoat.SetValue = _actualDistance;
    }

    float refVelLongitude = 0;
    float refVelLatitude = 0;
    private void CalculateActualAngels(Vector2 target)
    {
        _actualLongitude = Mathf.SmoothDamp(_actualLongitude, target.x, ref refVelLongitude, _accelerationAngels);
        _actualLatitude = Mathf.SmoothDamp(_actualLatitude, target.y, ref refVelLatitude, _accelerationAngels);
    }
    #endregion

    #region TARGET CALCUL DISTANCE ANGELS
    private void CalculTargetDistance(float value)
    {
        if(value == 0) { return; }
        value = Mathf.Clamp(value, -1, 1);
        _targetDistance += value * Mathf.Lerp(_speedDistance * 0.5f, _speedDistance,_actualDistance/_distanceMax) * Time.deltaTime;
        _targetDistance = Mathf.Clamp(_targetDistance, _distanceMin, _distanceMax);
    }

    private void CalculTargetAngels(Vector2 value)
    {
        //Permet que si la souris fait un petit mouvement aucun mouvement sur la camera sera fait.
        if (Mathf.Abs(value.x) < _minForceMouse.x) { value.x = 0; }
        if (Mathf.Abs(value.y) < _minForceMouse.y) { value.y = 0; }
        if (value == Vector2.zero || value.magnitude < 0.1f) { return; }

        value = value.normalized;

        _targetAngels += new Vector2(value.x * (_speedAngels * (1-Mathf.Lerp(0, 1, Mathf.Abs(_actualLatitude)/90))) *Time.deltaTime,
            value.y * (_speedAngels *_speedLatitudeMultiply) * Time.deltaTime);

        _targetAngels.x = RepeatCustom(_targetAngels.x, -180, 180);
        _targetAngels.y = Mathf.Clamp(_targetAngels.y, _minLatitude,_maxLatitude);
    }
    #endregion

 

    //Permet de faire des loop entre deux valeur si valeur < min valeur sera = a max
    //Ajouter recalcul de _acturalLongitude. Ca valeur est recalculer pour eviter que au smoothdamp la valeur aille du mauvais sense
    private float RepeatCustom (float value, float min, float max)
    {
        if (value < min)
        {
            value = max + (Mathf.Abs(min) - Mathf.Abs(value));
            _actualLongitude = 180 + (Mathf.Abs(min) - Mathf.Abs(_actualLongitude));
        }
        else if (value > max)
        {
            value = min - (Mathf.Abs(max) - Mathf.Abs(value));
            _actualLongitude = -180 - (Mathf.Abs(max) - Mathf.Abs(_actualLongitude));
        }

        return value;
    }


}

[Serializable]
public struct OffsetTracketObjectData
{
    [SerializeField]
    private float minDistance, maxDistance;
    [SerializeField]
    private float minLongitude, maxLongitude;
    [SerializeField]
    private float minLatitude, maxLatitude;
    [SerializeField]
    private Vector3 _offsetResult;

    private bool CheckCondition (float distance, float longitude, float latitude)
    {
        return (distance >= minDistance && distance <= maxDistance) 
            && (longitude >= minLongitude && longitude <= maxLongitude)
            && (latitude >= minLatitude && latitude <= maxLatitude);
    }

    public bool GetOffsetResult (float distance, float longitude, float latitude, out Vector3 offsetResult)
    {
        offsetResult = _offsetResult;
        return CheckCondition(distance, longitude, latitude);
    }
}

[Serializable]
public class OffsetTracketObject
{
    [SerializeField]
    private List<OffsetTracketObjectData> _datas;
    [SerializeField]
    private float _acceleration;
    private Vector3 _vel = Vector3.zero;
    private Vector3 targetOffsetObject;
    [SerializeField]
    private Vector3 defaultOffset;

    public void Init ()
    {
        targetOffsetObject = defaultOffset;
    }

    private Vector3 GetTargetOffset (float distance, float longitude, float latitude)
    {
        Vector3 target = Vector3.zero;
        for(int i = 0; i < _datas.Count;i++)
        {
            if (_datas[i].GetOffsetResult(distance, longitude, latitude, out target) )
            {
                return target;
            }
        }
        target = defaultOffset;
        return target;
    }

    public Vector3 CalculateOffsetTracketObject (Vector3 offsetObject, float distance, float longitude, float latitude)
    {
        targetOffsetObject = GetTargetOffset(distance, longitude, latitude);

        return Vector3.SmoothDamp(offsetObject, targetOffsetObject, ref _vel, _acceleration);
    }
}

[Serializable]
public class FOVHandel
{
    [SerializeField]
    private float _maxFov;
    [SerializeField]
    private float _acceleration;
    [SerializeField]
    private List<FOVModules> _modules;

    private float _actualFov;
    private float vel;

    public float GetFOv { get { return CalculateFov(CalculateTargetFov()); } }

    private float CalculateFov(float targetFov)
    {
        _actualFov = Mathf.SmoothDamp(_actualFov, targetFov, ref vel, _acceleration);
        return _actualFov;
    }

    private float CalculateTargetFov ()
    {
       float targetFov = 0;
       for (int i = 0; i< _modules.Count; i++)
       {
            targetFov += _modules[i].GetFOV;
       }
        targetFov = Mathf.Clamp(targetFov, 0, _maxFov);
        return targetFov;
    }
}

[Serializable]
public class FOVModules
{
    [SerializeField]
    [Tooltip("Valeur qui vas influencer la fov ajouter")]
    private FloatData_SO _influenceValue;
    [SerializeField]
    private float _minInfluence, _maxInfluence;
    [SerializeField]
    private float _minFovAdd, _maxFovAdd;
    [SerializeField]
    private AnimationCurve _curve;

    public float GetFOV { get { return CalculateFov(); } }

    private float CalculateFov ()
    {
        return Mathf.Lerp(_minFovAdd, _maxFovAdd, _curve.Evaluate(Mathf.InverseLerp(_minInfluence, _maxInfluence, _influenceValue.GetValue)));
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(CameraController))]
public class RefreshPostionCamera : Editor
{
    CameraController myScript;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (myScript == null)
        {
            myScript = (CameraController)target;
        }

        myScript.SetComponentRef();
        myScript.SetTrackedOffset();
        myScript.SetFov();
        myScript.SetCameraPosition();

    }
}
#endif



/*#if UNITY_EDITOR

[CustomEditor(typeof(CameraController))]
public class MonScriptEditor : Editor
{
    float actualDist;
    CameraController myScript;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(myScript == null)
        {
            myScript = (CameraController)target;
            myScript._transposer = myScript._virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            myScript._composer = myScript._virtualCamera.GetCinemachineComponent<CinemachineComposer>();
            actualDist = myScript.GetActualDistance();
        }

        GUILayout.Label("ActualDist: " + actualDist.ToString());
        GUILayout.BeginHorizontal();
        GUILayout.Label("Min: " + myScript._distanceMin.ToString()); 
        float newActualDist = GUILayout.HorizontalSlider(actualDist, myScript._distanceMin, myScript._distanceMax);
        GUILayout.Label("Max: " + myScript._distanceMax.ToString());
        GUILayout.EndHorizontal();
    }
}
#endif*/
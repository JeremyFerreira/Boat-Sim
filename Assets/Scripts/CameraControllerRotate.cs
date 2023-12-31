
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class CameraControllerRotate : MonoBehaviour
{

    [SerializeField]
    private WaterElevation _waterEvaluation;
    [SerializeField]
    public float _offsetAboveWater;
    [SerializeField]
    private InputVectorScriptableObject _moveCamera;
    [SerializeField]
    public Vector2 _actualAngel;
    [SerializeField]
    private Vector2 _minAngel, _maxAngel;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _acceleration;
    [SerializeField]
    private Vector2 _minForceMouse;
    [SerializeField]
    private bool _repeat;

    private Vector2 _targetAngel;
    private Vector2 _oldAngel;
    private Vector2 vel;

    public Vector3 _initialPosition;

    public bool editor;

    private void OnEnable()
    {
        _moveCamera.OnValueChanged += CalculateTargetAngel;
    }

    private void OnDisable()
    {
       _moveCamera.OnValueChanged -= CalculateTargetAngel;
    }

    // Start is called before the first frame update
    void Start()
    {
        _targetAngel = _actualAngel;
        _initialPosition = transform.position + new Vector3(0,_offsetAboveWater,0);
    }
    private void FixedUpdate()
    {
        CalculateActualAngel();

        SetCameraRotation();
    }

    public void SetCameraRotation ()
    {
        _actualAngel.x = Mathf.Clamp(_actualAngel.x, _minAngel.x, _maxAngel.x);
        _actualAngel.y = Mathf.Clamp(_actualAngel.y, _minAngel.y, _maxAngel.y);
       // transform.position = new Vector3(transform.position.x, _waterEvaluation.GetElevation(transform.position.x, transform.position.y) + _offsetAboveWater, transform.position.z);;
        /*   float yWater = _waterEvaluation.GetElevation(transform.position.x, transform.position.y);
           if (transform.position.y - _offsetAboveWater < yWater)
           {
               transform.position = new Vector3(transform.position.x, yWater + _offsetAboveWater, transform.position.z);
           }
           else
           {
               transform.position = _initialPosition;
           }*/
        Vector3 angelToSet = ((transform.rotation.eulerAngles) - (Vector3) _oldAngel)  + (Vector3)_actualAngel;
        _oldAngel = _actualAngel;
        angelToSet.z = 0;
        transform.rotation = Quaternion.Euler(angelToSet);
        //Quaternion.EulerAngles
    }

    public void CalculateActualAngel ()
    {
        _actualAngel = Vector2.SmoothDamp(_actualAngel, _targetAngel,ref vel, _acceleration);
    }

    public void CalculateTargetAngel (Vector2 value)
    {
        if (Mathf.Abs(value.x) < _minForceMouse.x) { value.x = 0; }
        if (Mathf.Abs(value.y) < _minForceMouse.y) { value.y = 0; }
        if (value == Vector2.zero || value.magnitude < 0.1f) { return; }

        value = value.normalized;

        _targetAngel.x = Mathf.Clamp(_targetAngel.x + (value.y *-1) * Time.deltaTime * _speed, _minAngel.x, _maxAngel.x);
        _targetAngel.y = RepeatCustom(_targetAngel.y + value.x * Time.deltaTime * _speed, _minAngel.y, _maxAngel.y, _repeat);
    }


    //Permet de faire des loop entre deux valeur si valeur < min valeur sera = a max
    //Ajouter recalcul de _acturalLongitude. Ca valeur est recalculer pour eviter que au smoothdamp la valeur aille du mauvais sense
    private float RepeatCustom(float value, float min, float max, bool reapeat)
    {
        if(!_repeat)
        {
            return Mathf.Clamp(value, min, max);
        }

        if (value < min)
        {
            value = max + (Mathf.Abs(min) - Mathf.Abs(value));
            _actualAngel.y = max + (Mathf.Abs(min) - Mathf.Abs(_actualAngel.x));
        }
        else if (value > max)
        {
            value = min - (Mathf.Abs(max) - Mathf.Abs(value));
            _actualAngel.y = min - (Mathf.Abs(max) - Mathf.Abs(_actualAngel.x));
        }

        return value;
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(CameraControllerRotate))]
public class RefreshPostionCameraT : Editor
{
    CameraControllerRotate myScript;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (myScript == null)
        {
            myScript = (CameraControllerRotate)target;
            myScript._initialPosition = myScript.transform.position + new Vector3(0, myScript._offsetAboveWater, 0);
        }
        if(myScript.editor)
        {
            myScript.SetCameraRotation();
        }
    } 
}
#endif
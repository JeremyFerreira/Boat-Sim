using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRoundLook : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 360f)]
    private float _minAngle, _maxAngle;
    [SerializeField]
    private float _actualAngle;
    [SerializeField]
    private float _accelerationDistance;
    [SerializeField]
    private float _sensiDistance;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

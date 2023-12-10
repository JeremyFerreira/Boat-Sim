using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPoint : MonoBehaviour
{
#if UNITY_EDITOR
    [Range(0f, 2f)]
    [SerializeField] private float _gizmosRadius;
    [SerializeField] private WaterElevation _waterElevation;
    private void OnValidate()
    {
        if(_waterElevation != null)
        {
            _waterElevation = FindAnyObjectByType<WaterElevation>();
        }
    }
    private void OnDrawGizmosSelected()
    {
        if(_gizmosRadius > 0)
        {
            if(_waterElevation.GetElevation(transform.position.x,transform.position.z) - transform.position.y > 0)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;
            }
            Gizmos.DrawSphere(transform.position, _gizmosRadius);
        }
    }
#endif
}

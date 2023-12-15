using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagController : MonoBehaviour
{
    [SerializeField] WindSO _windData;
    [SerializeField] Transform[] _flagsTransforms;
    // Start is called before the first frame update
    private void Update()
    {
        RotateFlagsBasedOnWindDirection();
    }
    void RotateFlagsBasedOnWindDirection()
    {
        Vector3 windNormal = Vector3.Cross(_windData.WindDirection, Vector3.up);
        Vector3 voileForward = Vector3.Cross(transform.up, windNormal);
        foreach (Transform t in _flagsTransforms)
        {
            if(voileForward!=Vector3.zero)
            {
                t.transform.rotation = Quaternion.LookRotation(voileForward, transform.up);
            }
        }
    }
}

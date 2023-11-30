using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagController : MonoBehaviour
{
    [SerializeField] WindController _windController;
    [SerializeField] Transform[] _flagsTransforms;
    // Start is called before the first frame update
    private void Update()
    {
        RotateFlagsBasedOnWindDirection();
    }
    void RotateFlagsBasedOnWindDirection()
    {
        Vector3 windNormal = Vector3.Cross(_windController.GetWindDirection(), Vector3.up);
        Vector3 voileForward = Vector3.Cross(transform.up, windNormal);
        foreach (Transform t in _flagsTransforms)
        {
            t.transform.rotation = Quaternion.LookRotation(voileForward, transform.up);
        }
    }
}

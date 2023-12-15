using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFollowBoat : MonoBehaviour
{
    [SerializeField] private Transform _boat;
    // Update is called once per frame
    void Update()
    {
        transform.position = MathExtension.FlatVectorXZ(_boat.position)-MathExtension.FlatVectorXZ(transform.localScale/2f);
    }
}

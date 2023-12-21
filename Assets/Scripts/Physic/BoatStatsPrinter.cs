using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoatStatsPrinter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _boatSpeedtext;
    [SerializeField]
    private FloatData_SO _speedBoat_SO;

    private Rigidbody _boatRigidbody;

    private void OnValidate()
    {
        _boatRigidbody = GetComponent<Rigidbody>();
    }
    private void Awake()
    {
        _boatRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _speedBoat_SO.SetValue = Mathf.RoundToInt(new Vector3(_boatRigidbody.velocity.x, 0, _boatRigidbody.velocity.z).magnitude * 3.6f);
        _boatSpeedtext.text = (Mathf.RoundToInt(new Vector3(_boatRigidbody.velocity.x,0, _boatRigidbody.velocity.z).magnitude * 3.6f)).ToString() + " km/h";
    }

    private void OnDisable()
    {
        _speedBoat_SO.SetValue = 0;
    }
}

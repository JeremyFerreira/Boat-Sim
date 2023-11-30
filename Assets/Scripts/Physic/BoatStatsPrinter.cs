using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoatStatsPrinter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _boatSpeedtext;

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
        _boatSpeedtext.text = (Mathf.RoundToInt((_boatRigidbody.velocity.magnitude * 3.6f)*100f)/100f).ToString() + " km/h";
    }
}

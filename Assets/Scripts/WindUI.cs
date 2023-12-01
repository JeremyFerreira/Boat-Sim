using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindUI : MonoBehaviour
{
    [SerializeField] private WindSO _windData;
    [SerializeField] private RectTransform _windImage;
    private Transform _cameraTransform;
    void Awake()
    {
        _cameraTransform = Camera.main.transform;
    }
    private void Update()
    {
        UpdtateWindInterface();
    }

    //To change class
    void UpdtateWindInterface()
    {
        _windImage.eulerAngles = new Vector3(0, 0, _windData.WindAngle * Mathf.Rad2Deg + _cameraTransform.eulerAngles.y);
        _windImage.localScale = Vector3.one * _windData.WindForce;
    }
}

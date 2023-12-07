using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GouvernailController : MonoBehaviour
{
    [SerializeField] private Rigidbody _boatRigidbody;
    [SerializeField] private float _maxGouvernailAngle;
    [SerializeField] private float _rotationForce;
    [SerializeField] private Transform _gouvernailTransform;
    private float _gouvernailAngle;
    [SerializeField] private Slider _sliderGouvernail;
    [SerializeField] private float _dragScale = 2;

    [SerializeField]
    private MoveBoatElement _moveBoatElement;

    // Start is called before the first frame update
    void Start()
    {
        _sliderGouvernail.onValueChanged.AddListener(SetGouvernailAngle);
    }

    private void OnEnable()
    {
        _moveBoatElement.OnEnableFunc();
        _moveBoatElement._eventMoveElement += SetGouvernailAngle;
    }

    private void OnDisable()
    {
        _moveBoatElement.OnDisableFunc();
        _moveBoatElement._eventMoveElement -= SetGouvernailAngle;
    }

    private void Update()
    {
        _moveBoatElement.UpdateFunc();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //gouvernail
        //normal direction of gouvernail
        Vector3 gouvernailNormal = _gouvernailAngle - 180 > 0 ? _gouvernailTransform.right : -_gouvernailTransform.right;
        float magnitudeForce = Vector3.Dot(transform.forward, gouvernailNormal);
        //-180 because jumps to 359 degrees to 0 automatically so remap form -180 to 180
        Vector3 forceDirection = _gouvernailAngle - 180 > 0 ? -transform.right : transform.right;
        //_boatRigidbody.AddForceAtPosition(_rotationForce * forceDirection * magnitudeForce, _gouvernailTransform.position,ForceMode.Acceleration);
        _boatRigidbody.AddTorque(transform.up * _rotationForce * -1 * magnitudeForce);
        //_boatRigidbody.drag = 0.3f + (1-Vector3.Dot(_boatRigidbody.velocity.normalized, transform.forward))*_dragScale;
    }

    public void SetGouvernailAngle(float value)
    {
        //Pour mettre a jour le Slider

        _gouvernailAngle = Mathf.Deg2Rad * Mathf.Lerp(-_maxGouvernailAngle, _maxGouvernailAngle, value);
        _gouvernailTransform.transform.rotation = Quaternion.LookRotation(MathExtension.RotateVectorOnXZPlane(transform.forward, _gouvernailAngle), transform.up);
    }
    private void OnDrawGizmosSelected()
    {
        Vector3 gouvernailNormal = _gouvernailAngle - 180 > 0 ? _gouvernailTransform.right : -_gouvernailTransform.right;
        float magnitudeForce = Vector3.Dot(transform.forward, gouvernailNormal);
        //-180 because jumps to 359 degrees to 0 automatically so remap form -180 to 180
        Vector3 forceDirection = _gouvernailAngle - 180 > 0 ? -transform.right : transform.right;
        Gizmos.DrawLine(_gouvernailTransform.position, _gouvernailTransform.position + _rotationForce * forceDirection * magnitudeForce);
    }
}

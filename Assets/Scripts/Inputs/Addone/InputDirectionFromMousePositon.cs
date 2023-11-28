using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputDirectionFromMousePositon : MonoBehaviour
{
    [SerializeField]
    private Transform _refObjectDirection;

    [SerializeField]
    private UnityEvent<Vector2> _OnValueChange;

    [SerializeField]
    private UnityEvent<Vector3> _OnValueChangeYAxis;

    [SerializeField]
    private bool _CanTwiceSameValue;

    [SerializeField]
    [Range(0f, 1f)]
    private float _deadZone;

    Vector2 oldInputLook = new Vector2(-10,-10);
    [SerializeField]
    private Camera cam;

    Camera _camera;
    private void Awake()
    {
        _camera = Camera.main;
        if(_camera == null)
        {
            _camera = cam;
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void CalculateLookDirection(Vector2 inputLook)
    {
        if(!_CanTwiceSameValue && oldInputLook == inputLook) { return; }
        oldInputLook = inputLook;
        if (InputManager.IsGamepad()) 
        { 
            if(inputLook.magnitude > _deadZone)
            {
                _OnValueChange?.Invoke(inputLook);
            }
            return; 
        }
        StopAllCoroutines();
        StartCoroutine(CoroutineIsMousePosition());
        //CalculateDirectionFromMousePosition(inputLook);
    }

    public void CalculateLookDirectionWithYAxis(Vector2 inputLook)
    {
        if (!_CanTwiceSameValue && oldInputLook == inputLook) { return; }
        oldInputLook = inputLook;
        if (InputManager.IsGamepad()) 
        {
            if (inputLook.magnitude > _deadZone)
            {
                _OnValueChangeYAxis?.Invoke(new Vector3(inputLook.x, 0, inputLook.y));
            }
            return; 
        }
        StopAllCoroutines();
        StartCoroutine(CoroutineIsMousePositionWithYAxis());
        //CalculateDirectionFromMousePositionWithYAxis(inputLook);
    } 

    private void CalculateDirectionFromMousePosition(Vector2 mousePosition)
    {
        Vector3 worldMousePosition = _camera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, _camera.transform.position.y));
        Vector3 direction = new Vector3(worldMousePosition.x, 0, worldMousePosition.z) - new Vector3(_refObjectDirection.position.x, 0, _refObjectDirection.position.z);
        direction = direction.normalized;


        _OnValueChange?.Invoke(new Vector2(direction.x, direction.z));
    }

    private void CalculateDirectionFromMousePositionWithYAxis(Vector2 mousePosition)
    {
        Vector3 worldMousePosition = _camera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, _camera.transform.position.y));
        Vector3 direction = new Vector3(worldMousePosition.x, 0, worldMousePosition.z) - new Vector3(_refObjectDirection.position.x, 0, _refObjectDirection.position.z);
        direction = direction.normalized;

        _OnValueChangeYAxis?.Invoke(direction);
    }

    IEnumerator CoroutineIsMousePosition ()
    {
        do
        {
            CalculateDirectionFromMousePosition(oldInputLook);
            yield return new WaitForEndOfFrame();
        } while (true);
    }

    IEnumerator CoroutineIsMousePositionWithYAxis()
    {
        do
        {
            CalculateDirectionFromMousePositionWithYAxis(oldInputLook);
            yield return new WaitForEndOfFrame();
        } while (true);
    }
}

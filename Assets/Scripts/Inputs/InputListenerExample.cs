using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputListenerExample : MonoBehaviour
{
    [SerializeField] private InputButtonScriptableObject _inputButton;
    [SerializeField] private InputVectorScriptableObject _inputVector;
    public UnityEvent<bool> onButtonPressed;
    public UnityEvent onVectorMoved;

    private void OnEnable()
    {
        _inputButton.OnValueChanged += ButtonMethod;
        _inputVector.OnValueChanged += VectorMethod;
    }
    private void OnDisable()
    {
        _inputButton.OnValueChanged -= ButtonMethod;
        _inputVector.OnValueChanged -= VectorMethod;
    }
    private void ButtonMethod(bool value)
    {
        onButtonPressed?.Invoke(value);
    }
    private void VectorMethod(Vector2 value)
    {
        onVectorMoved?.Invoke();
    }
}

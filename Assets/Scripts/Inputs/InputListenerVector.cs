using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputListenerVector : MonoBehaviour
{
    [SerializeField] InputVectorScriptableObject inputToListen;
    [Tooltip("If true, the listener will listen while the gameObject is active or not")]
    [SerializeField] private bool alwaysListen = true;
    public UnityEvent InputValueTrue;
    public UnityEvent InputValueFalse;
    public UnityEvent<Vector2> InputValueChanged;
    public UnityEvent onDisable;
    [SerializeField] bool listenInputWithDelay = false;

    private void Awake()
    {
        if (alwaysListen)
        {
            inputToListen.OnValueChanged += ListenerBehaviour;
        }
    }


    private void OnEnable()
    {
        if (!alwaysListen)
        {
            if (listenInputWithDelay)
            {
                StartCoroutine(AddListener());
            }
            else
            {
                inputToListen.OnValueChanged += ListenerBehaviour;
            }
        }
    }
    IEnumerator AddListener()
    {
        yield return new WaitForEndOfFrame();
        inputToListen.OnValueChanged += ListenerBehaviour;
    }

    private void OnDisable()
    {
        if (!alwaysListen)
        {
            inputToListen.OnValueChanged -= ListenerBehaviour;
            onDisable?.Invoke();
        }
    }
    private void ListenerBehaviour(Vector2 value)
    {
        if (value != Vector2.zero)
        {
            InputValueTrue?.Invoke();
        }
        else
        {
            InputValueFalse?.Invoke();
        }
       
        InputValueChanged?.Invoke(value);
    }
}

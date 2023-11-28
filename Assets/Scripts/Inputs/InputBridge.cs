using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBridge : MonoBehaviour
{
    private InputManager _inputManager;

    private void Awake()
    {
        _inputManager = InputManager.Instance;
    }

    /*public void DisableInput(InputActionMaps _inputAction)
    {
        switch (_inputAction)
        {
            case InputActionMaps.game:
                _inputManager.DisableGameInput();
                break;
            default:
                Debug.Log(_inputAction + " n est pas definie");
                break;
        }
    }

    public void EnableInput(InputActionMaps _inputAction)
    {
        switch (_inputAction)
        {
            case InputActionMaps.game:
                _inputManager.EnableGameInput();
                break;
            default:
                Debug.Log(_inputAction + " n est pas definie");
                break;
        }
    }*/
}

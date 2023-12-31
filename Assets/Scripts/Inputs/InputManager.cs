using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public static PlayerControls _input { private set; get; }

    [Header("Game")]
    [SerializeField] private InputFloatScriptableObject _moveSalling;
    [SerializeField] private InputFloatScriptableObject _openingSalling;
    [SerializeField] private InputFloatScriptableObject _moveRudder;
    [SerializeField] private InputVectorScriptableObject _moveCamera;
    [SerializeField] private InputFloatScriptableObject _zoomCamera;
    [SerializeField] private InputButtonScriptableObject _pause;
    [SerializeField] private InputButtonScriptableObject _cameraSwitch;

    [Space]
    [Header("Debug")]
    [SerializeField] private InputButtonScriptableObject _cheatMenu;

    private bool _isGamepad { get; set; }
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(this);
        if (_input == null)
        {
            _input = new PlayerControls();
        }
    }


    private void OnEnable()
    {
#if(UNITY_EDITOR)
        SetDebugInput();
        ActiveDebugInput();
#endif
        ActiveGameInputs();
        SetGameInput();
    }
    private void OnDisable()
    {
#if (UNITY_EDITOR)
        UnSetDebugInput();
#endif
        UnSetGameInput();
    }

    #region FUNCTIONS: ACTIVE INPUTS
    public void ActiveGameInputs(bool value = true)
    {
        if (value)
        {
            _input.Game.Enable();
        }
        else
        {
            _input.Game.Disable();
        }
    }
    public void ActiveUIInput(bool value = true)
    {
        if (value)
        {
            _input.UI.Enable();
        }
        else
        {
            _input.UI.Disable();
        }
    }
    public void ActiveDebugInput(bool value = true)
    {
        if (value)
        {
            _input.Debug.Enable();
        }
        else
        {
            _input.Debug.Disable();
        }
    }
    #endregion

    #region FUNCTIONS: SET/UNSET INPUTS
    public void SetGameInput()
    {
        //MoveSailing
        _input.Game.MoveSailing.performed += ctx => _moveSalling.ChangeValue(_input.Game.MoveSailing.ReadValue<float>());
        _input.Game.MoveSailing.canceled += ctx => _moveSalling.ChangeValue(0);

        //OpeningSailing
        _input.Game.OpeningSailing.performed += ctx => _openingSalling.ChangeValue(_input.Game.OpeningSailing.ReadValue<float>());
        _input.Game.OpeningSailing.canceled += ctx => _openingSalling.ChangeValue(0);

        //MoveRudder
        _input.Game.MoveRudder.performed += ctx =>  _moveRudder.ChangeValue(_input.Game.MoveRudder.ReadValue<float>());
        _input.Game.MoveRudder.canceled += ctx => _moveRudder.ChangeValue(0);

        //MoveCamera
        _input.Game.MoveCamera.performed += ctx => _moveCamera.ChangeValue(_input.Game.MoveCamera.ReadValue<Vector2>());
        _input.Game.MoveCamera.canceled += ctx => _moveCamera.ChangeValue(new Vector2(0,0));

        //ZoomCamera
        _input.Game.ZoomCamera.performed += ctx => _zoomCamera.ChangeValue(_input.Game.ZoomCamera.ReadValue<float>());
        _input.Game.ZoomCamera.canceled += ctx => _zoomCamera.ChangeValue(0);

        //Pause  
        _input.Game.Pause.performed += ctx => _pause.ChangeValue(!_pause.value);

        //CameraSwitch  
        _input.Game.SwitchCamera.performed += ctx => _cameraSwitch.ChangeValue(true);
    }
    public void UnSetGameInput()
    {
        //MoveSailing
        _input.Game.MoveSailing.performed -= ctx => _moveSalling.ChangeValue(_input.Game.MoveSailing.ReadValue<float>());
        _input.Game.MoveSailing.canceled -= ctx => _moveSalling.ChangeValue(0);

        //OpeningSailing
        _input.Game.OpeningSailing.performed -= ctx => _openingSalling.ChangeValue(_input.Game.OpeningSailing.ReadValue<float>());
        _input.Game.OpeningSailing.canceled -= ctx => _openingSalling.ChangeValue(0);

        //MoveRudder
        _input.Game.MoveRudder.performed -= ctx => _moveRudder.ChangeValue(_input.Game.OpeningSailing.ReadValue<float>());
        _input.Game.MoveRudder.canceled -= ctx => _moveRudder.ChangeValue(0);

        //MoveCamera
        _input.Game.MoveCamera.performed -= ctx => _moveCamera.ChangeValue(_input.Game.MoveCamera.ReadValue<Vector2>());
        _input.Game.MoveCamera.canceled -= ctx => _moveCamera.ChangeValue(new Vector2(0, 0));

        //ZoomCamera
        _input.Game.ZoomCamera.performed += ctx => _zoomCamera.ChangeValue(_input.Game.ZoomCamera.ReadValue<float>());
        _input.Game.ZoomCamera.canceled += ctx => _zoomCamera.ChangeValue(0);

        //Pause
        _input.Game.Pause.performed -= ctx => _pause.ChangeValue(!_pause.value);
       // _input.Game.Pause.canceled -= ctx => _pause.ChangeValue(false);

        //CameraSwitch  
        _input.Game.SwitchCamera.performed -= ctx => _cameraSwitch.ChangeValue(true);
    }

    public void SetDebugInput()
    {
        //cheatMenu
        _input.Debug.CheatMenu.performed += ctx => _cheatMenu.ChangeValue(!_cheatMenu.value);
    }
    public void UnSetDebugInput()
    {
        //cheatMenu
        _input.Debug.CheatMenu.performed += ctx => _cheatMenu.ChangeValue(!_cheatMenu.value);
    }
    #endregion

    private void Update()
    {
        //find the last Input Device used and set a bool.
        _isGamepad = IsGamepad();
    }

    public static bool IsGamepad()
    {
        InputDevice lastUsedDevice = null;
        float lastEventTime = 0;
        foreach (var device in InputSystem.devices)
        {
            if (device.lastUpdateTime > lastEventTime)
            {
                lastUsedDevice = device;
                lastEventTime = (float)device.lastUpdateTime;
            }
        }

        return lastUsedDevice is Gamepad;
    }
}


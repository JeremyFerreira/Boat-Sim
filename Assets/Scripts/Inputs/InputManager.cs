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
    [SerializeField] private InputButtonScriptableObject _shoot;
    [SerializeField] private InputVectorScriptableObject _move;
    [SerializeField] private InputVectorScriptableObject _look;
    [SerializeField] private InputButtonScriptableObject _pause;
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
        EnableGameInput();
        _input.UI.Enable();
    }
    private void OnDisable()
    {
        _input.UI.Disable();
        DisableGameInput();
    }
    public void ActiveGameInputs(bool value)
    {
        _shoot.IsActive = value;
        _move.IsActive = value;
        _look.IsActive = value;

        if (value)
        {
            _input.UI.Disable();
        }
        else
        {
            _input.UI.Enable();
        }
    }
    public void EnableGameInput()
    {
        _input.Game.Enable();

        //Move
        _input.Game.Move.performed += ctx => _move.ChangeValue(_input.Game.Move.ReadValue<Vector2>());
        _input.Game.Move.canceled += ctx => _move.ChangeValue(Vector2.zero);
        //Shoot
        _input.Game.Shoot.performed += ctx => _shoot.ChangeValue(true);
        _input.Game.Shoot.canceled += ctx => _shoot.ChangeValue(false);
        //Rotate
        _look.IsActive = true;
        _input.Game.Rotate.performed += ctx => _look.ChangeValue(_input.Game.Rotate.ReadValue<Vector2>());
        //Pause
        _input.Game.Pause.performed += ctx => _pause.ChangeValue(true);
        _input.Game.Pause.canceled += ctx => _pause.ChangeValue(false);
        //cheatMenu
        _input.Game.CheatMenu.performed += ctx => _cheatMenu.ChangeValue(true);
        _input.Game.CheatMenu.canceled += ctx => _cheatMenu.ChangeValue(false);

    }
    public void DisableGameInput()
    {
        //Move
        _input.Game.Move.performed -= ctx => _move.ChangeValue(_input.Game.Move.ReadValue<Vector2>());
        _input.Game.Move.canceled -= ctx => _move.ChangeValue(Vector2.zero);
        //Shoot
        _input.Game.Shoot.performed -= ctx => _shoot.ChangeValue(true);
        _input.Game.Shoot.canceled -= ctx => _shoot.ChangeValue(false);
        //Rotate
        _look.IsActive = false;
        _input.Game.Rotate.performed -= ctx => _look.ChangeValue(_input.Game.Rotate.ReadValue<Vector2>());
        //Pause
        _input.Game.Pause.performed -= ctx => _pause.ChangeValue(true);
        _input.Game.Pause.canceled -= ctx => _pause.ChangeValue(false);
        //cheatMenu
        _input.Game.CheatMenu.performed -= ctx => _cheatMenu.ChangeValue(true);
        _input.Game.CheatMenu.canceled -= ctx => _cheatMenu.ChangeValue(false);

        _input.Game.Disable();
    }
    public void EnableUIInput(bool value)
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
    private void Update()
    {
        //find the last Input Device used and set a bool.
        _isGamepad = IsGamepad();


        if (_look.IsActive && !_isGamepad)
        {
            _look.ChangeValue(Mouse.current.position.ReadValue());
        }
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


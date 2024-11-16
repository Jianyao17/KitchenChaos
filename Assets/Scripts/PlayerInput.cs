using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput Instance;
    
    public event EventHandler OnInteractAction; 
    public event EventHandler OnInteractAltAction; 
    public event EventHandler OnPauseAction; 
    
    private PlayerInputActions _inputActions;

    private void Awake()
    {
        _inputActions = new PlayerInputActions();
        if (PlayerPrefs.HasKey("PlayerInputKey"))
            _inputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString("PlayerInputKey"));
        
        _inputActions.Player.Enable();
        _inputActions.Player.Interact.performed += InteractOnPerformed;
        _inputActions.Player.InteractAlternate.performed += InteractAlternateOnPerformed;
        _inputActions.Player.Pause.performed += PauseOnPerformed;
        Instance = this;
    }

    private void OnDestroy()
    {
        _inputActions.Player.Interact.performed -= InteractOnPerformed;
        _inputActions.Player.InteractAlternate.performed -= InteractAlternateOnPerformed;
        _inputActions.Player.Pause.performed -= PauseOnPerformed;
        _inputActions.Player.Disable();
        _inputActions.Dispose();
    }

    private void PauseOnPerformed(InputAction.CallbackContext obj) 
        => OnPauseAction?.Invoke(this, EventArgs.Empty);

    private void InteractAlternateOnPerformed(InputAction.CallbackContext obj) 
        => OnInteractAltAction?.Invoke(this, EventArgs.Empty);

    private void InteractOnPerformed(InputAction.CallbackContext obj) 
        => OnInteractAction?.Invoke(this, EventArgs.Empty);

    public Vector2 GetMoveDirectionNormalized() 
        => _inputActions.Player.Move.ReadValue<Vector2>().normalized;

    public string GetKeyBindText(KeyBindType keyBindType)
    {
        switch (keyBindType)
        {
            case KeyBindType.MoveUp:
                return _inputActions.Player.Move.bindings[1].ToDisplayString();
            case KeyBindType.MoveDown:
                return _inputActions.Player.Move.bindings[2].ToDisplayString();
            case KeyBindType.MoveLeft:
                return _inputActions.Player.Move.bindings[3].ToDisplayString();
            case KeyBindType.MoveRight:
                return _inputActions.Player.Move.bindings[4].ToDisplayString();
            case KeyBindType.Interact:
                return _inputActions.Player.Interact.bindings[0].ToDisplayString();
            case KeyBindType.InteractAlt:
                return _inputActions.Player.InteractAlternate.bindings[0].ToDisplayString(); 
            case KeyBindType.Pause:
                return _inputActions.Player.Pause.bindings[0].ToDisplayString();
        };
        return string.Empty;
    }

    public void RebindKeyBinding(KeyBindType keyBindType, Action OnRebindComplete = null)
    {
        _inputActions.Disable();
        
        InputAction inputAction = new InputAction();
        int bindingIndex = 0;
        
        switch (keyBindType)
        {
            default:
            case KeyBindType.MoveUp:
                inputAction = _inputActions.Player.Move;
                bindingIndex = 1;
                break;
            case KeyBindType.MoveDown:
                inputAction = _inputActions.Player.Move;
                bindingIndex = 2;
                break;
            case KeyBindType.MoveLeft:
                inputAction = _inputActions.Player.Move;
                bindingIndex = 3;
                break;
            case KeyBindType.MoveRight:
                inputAction = _inputActions.Player.Move;
                bindingIndex = 4;
                break;
            case KeyBindType.Interact:
                inputAction = _inputActions.Player.Interact;
                bindingIndex = 0;
                break;
            case KeyBindType.InteractAlt:
                inputAction = _inputActions.Player.InteractAlternate;
                bindingIndex = 0;
                break;
            case KeyBindType.Pause:
                inputAction = _inputActions.Player.Pause;
                bindingIndex = 0;
                break;
        }
        
        var onRebindComplete = new Action<RebindingOperation>(callback =>
        {
            callback.Dispose();
            _inputActions.Enable();
            OnRebindComplete?.Invoke();
            PlayerPrefs.SetString("PlayerInputKey", _inputActions.SaveBindingOverridesAsJson());
            PlayerPrefs.Save();
        });
        
        inputAction
            .PerformInteractiveRebinding(bindingIndex)
            .OnComplete(onRebindComplete).Start();
    }
}

public enum KeyBindType
{
    MoveUp, MoveDown, MoveLeft, MoveRight,
    Interact, InteractAlt, Pause
}
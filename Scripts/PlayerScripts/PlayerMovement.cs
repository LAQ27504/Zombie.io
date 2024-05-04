using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }
    public static PlayerInputActions playerInput;

    public event EventHandler OnInteraction;
    public event EventHandler OnDrop;
    public event EventHandler<OnEquipedWeaponArgs> OnEquipedWeapon;
    public event EventHandler OnReload;
    public class OnEquipedWeaponArgs : EventArgs
    {
        public bool isEquiped;
    }

    public event EventHandler OnFiring;


    public bool isEquiped = false;
    public bool isRunning = false;
    public bool isGameEnd = false;

    private bool hasAddEvent = false;

    private void Awake()
    {
        Instance = this;
        
        playerInput = new PlayerInputActions();
        playerInput.Player.Disable();

        
    }

    public void SetEnable()
    {
        gameObject.SetActive(true);
        playerInput.Player.Enable();
        if (!hasAddEvent)
        {
            playerInput.Player.Running.performed += Running_performed;
            playerInput.Player.Interact.performed += Interact_performed;
            playerInput.Player.Drop.performed += Drop_performed;
            playerInput.Player.Reload.performed += Reload_performed;
        }
    }

    public void SetDisable()
    {
        playerInput.Player.Disable();
        if (hasAddEvent)
        {
            playerInput.Player.Running.performed -= Running_performed;
            playerInput.Player.Interact.performed -= Interact_performed;
            playerInput.Player.Drop.performed -= Drop_performed;
            playerInput.Player.Reload.performed -= Reload_performed;
        }
        gameObject.SetActive(false);
    }

    private void Reload_performed(InputAction.CallbackContext obj)
    {
        OnReload?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {
        if (!isGameEnd)
        {
            HandleEquipedWeapon();
            HandleFiring();
        }
    }

    private void Drop_performed(InputAction.CallbackContext obj)
    {
        OnDrop?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(InputAction.CallbackContext obj)
    {
        OnInteraction?.Invoke(this, EventArgs.Empty);
    }

    private void Running_performed(InputAction.CallbackContext obj)
    {
        isRunning = !isRunning;
    }

    private void HandleEquipedWeapon()
    {
        if (IsRightMouseDown() && !isGameEnd)
        {
            isEquiped = !isEquiped;
            OnEquipedWeapon?.Invoke(this, new OnEquipedWeaponArgs
            {
                isEquiped = isEquiped
            }) ;
        }
    }
    
    public void HandleFiring()
    {
        if (IsLeftMouseDown() && isEquiped && !isGameEnd)
        {
            OnFiring?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsRightMouseDown()
    {
        return Input.GetMouseButtonDown(1);
    }

    public bool IsLeftMouseDown()
    {
        return Input.GetMouseButtonDown(0);
    }

    public Vector2 GetMovementNormalize()
    {
        Vector2 inputVector = playerInput.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }

    
}

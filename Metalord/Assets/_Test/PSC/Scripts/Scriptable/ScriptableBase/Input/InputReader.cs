using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName ="PlayerInputReader")]
public class InputReader : ScriptableObject, PlayerAction.IPlayerActions
{
    private PlayerAction action;

    public event Action<Vector2> MoveEvent;
    public event Action JumpEvent;
    public event Action JumpCancelEvent;
    public event Action InteractEvent;
    public event Action InteractCancelEvent;


    private void OnEnable()
    {
        if (action == null)
        {
            action = new PlayerAction();
            action.Player.SetCallbacks(this);

            action.Player.Enable();
        }

    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            InteractEvent?.Invoke();
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            InteractCancelEvent?.Invoke();
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            JumpEvent?.Invoke();
        }

        if (context.phase == InputActionPhase.Canceled)
        {
            JumpCancelEvent?.Invoke();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }
}

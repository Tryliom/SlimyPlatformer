using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public bool jumpValue;
    public Vector2 moveValue;
    public bool pressXValue;
    public bool pressPauseValue;
    public bool leftDashValue;
    public bool rightDashValue;
    public bool dashValue;

    public void OnMove(InputValue context)
    {
        moveValue = context.Get<Vector2>();
    }

    public void OnJump(InputValue context)
    {
        jumpValue = context.isPressed;
    }
    
    public void OnPressX(InputValue context)
    {
        pressXValue = context.isPressed;
    }
    
    public void OnPressPause(InputValue context)
    {
        pressPauseValue = context.isPressed;
    }
    
    public void OnLeftDash(InputValue context)
    {
        leftDashValue = context.isPressed;
    }
    
    public void OnRightDash(InputValue context)
    {
        rightDashValue = context.isPressed;
    }
    
    public void OnDash(InputValue context)
    {
        dashValue = context.isPressed;
    }
}

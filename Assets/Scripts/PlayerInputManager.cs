using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public bool jumpValue;
    public float moveValue;
    public Vector2 moveWaterValue;
    public bool pressXValue;

    public void OnMove(InputValue context)
    {
        moveValue = context.Get<float>();
    }
    
    public void OnMoveWater(InputValue context)
    {
        moveWaterValue = context.Get<Vector2>();
    }
    
    public void OnJump(InputValue context)
    {
        jumpValue = true;
    }
    
    public void OnPressX(InputValue context)
    {
        pressXValue = true;
    }
}

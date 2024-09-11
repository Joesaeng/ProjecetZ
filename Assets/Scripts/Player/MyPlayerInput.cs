using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MyPlayerInput : MonoBehaviour
{
    public Vector2 MoveInput;
    public Vector2 LookInput;

    private void OnMove(InputValue value)
    {
        MoveInput = value.Get<Vector2>();
    }

    private void OnLook(InputValue value)
    {
        LookInput = value.Get<Vector2>();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public abstract class MoveableCharacter : MonoBehaviour
{
    protected abstract void CalculateMove();
    protected abstract void CalculateLookRotation();
    protected abstract void CheckRotationing();

    protected virtual void Update()
    {
        MovementUpdate();
    }
    private void MovementUpdate()
    {
        CalculateMove();
        CalculateLookRotation();
        CheckRotationing();
    }
}

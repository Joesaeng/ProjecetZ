using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(MyPlayerInput))]
public class PlayerController : MonoBehaviour
{
    MyPlayerInput _input;

    [Header("Player Setting")]
    [SerializeField] Transform LookTF;
    public float MoveSpeed;
    public float SpeedChangeRate;
    public float RotationSpeed;

    public float curMoveSpeed;

    private Vector3 targetDirection;
    private Quaternion targetRotation;

    private void Start()
    {
        _input = GetComponent<MyPlayerInput>();
        Managers.Time.SetPlayerTimeScale(1f);
    }

    private void Update()
    {
        CalculateMove();
        CalculateLookRotation();
    }

    private void FixedUpdate()
    {
        Movement();
        LookRotation();
    }

    private void CalculateMove()
    {
        Vector2 moveInput = _input.MoveInput;
        float targetMoveSpeed = _input.MoveInput == Vector2.zero ? 0f : MoveSpeed * moveInput.magnitude;
        float speedOffset = 0.1f;
        curMoveSpeed = Mathf.Lerp(curMoveSpeed, targetMoveSpeed, SpeedChangeRate * Time.deltaTime * Managers.Time.PlayerTimeScale);

        if (targetMoveSpeed > curMoveSpeed && targetMoveSpeed - curMoveSpeed < speedOffset ||
            targetMoveSpeed < curMoveSpeed && curMoveSpeed - targetMoveSpeed < speedOffset)
            curMoveSpeed = targetMoveSpeed;

        moveInput.Normalize();
        targetDirection = new Vector3(moveInput.x, moveInput.y,0f);
    }

    private void Movement()
    {
        transform.Translate(targetDirection * curMoveSpeed * Time.fixedDeltaTime);
    }

    private void CalculateLookRotation()
    {
        if (_input.LookInput == Vector2.zero)
            return;

        Vector2 lookInput = _input.LookInput.normalized;
        float targetAngle = Mathf.Atan2(lookInput.y, lookInput.x) * Mathf.Rad2Deg;
        targetRotation = Quaternion.Slerp(targetRotation, Quaternion.AngleAxis(targetAngle - 90, Vector3.forward),
            RotationSpeed * Time.deltaTime * Managers.Time.PlayerTimeScale);
    }

    private void LookRotation()
    {
        LookTF.transform.rotation = targetRotation;
    }
}

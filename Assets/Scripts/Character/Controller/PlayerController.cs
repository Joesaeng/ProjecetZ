using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(MyPlayerInput))]
public class PlayerController : MoveableCharacter
{
    Animator _animator;
    CharacterMovement _movement;
    MyPlayerInput _input;
    InteractionChecker _interactionChecker;
    DamageDealer _damageDealer;

    [Header("Player Setting")]
    [SerializeField,Tooltip("플레이어 시야 오브젝트 트랜스폼")] Transform LookLightTF;
    Light2D _lookLight;

    [Header("TEMP")]
    [SerializeField] GameObject InteractionButton;
    [SerializeField] BaseDamageDealerStatus DamageDealerStatus;
    [SerializeField] Transform Weapon;
    public Action<bool> OnAimObject;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _input = GetComponent<MyPlayerInput>();
        _interactionChecker = GetComponent<InteractionChecker>();
        _movement = GetComponent<CharacterMovement>();

        _movement.SetBody(gameObject.FindChild("stick").transform);
        _movement.SetLookBodyScale(1f);

        _lookLight = LookLightTF.GetComponent<Light2D>();

        _interactionChecker.OnInteractionable += HandleInteractionable;
        InteractionButton.SetActive(false);

        SetDamgeDealerByType();
    }

    private void SetDamgeDealerByType()
    {
        DamageDealerStatus.AddDamageDealerComponentByType(Weapon);
        _damageDealer = Weapon.GetComponent<DamageDealer>();
        _damageDealer.SetDamageDealerStatus(DamageDealerStatus);
    }

    protected override void Update()
    {
        base.Update();
        CheckWall_LookLight();
        CheckAttackInput();
        _animator.SetFloat("CurMoveSpeed", _movement.CurMoveSpeed);
    }

    public void HandleInteractionable(bool value)
    {
        InteractionButton.SetActive(value);
    }

    public void Interaction()
    {
        if (_interactionChecker.CurInteractionable != null)
        {
            _interactionChecker.CurInteractionable.Interaction();
        }
    }

    #region Moveable 3형제

    protected override void CalculateMove()
    {
        Vector2 moveInput = _input.MoveInput;
        float targetMoveSpeed = _input.MoveInput == Vector2.zero ? 0f : _movement.MoveSpeed * moveInput.magnitude;

        float speedOffset = 0.1f;
        _movement.CurMoveSpeed = Mathf.Lerp(_movement.CurMoveSpeed, targetMoveSpeed, _movement.SpeedChangeRate * Time.deltaTime * Managers.Time.PlayerTimeScale);

        if (targetMoveSpeed > _movement.CurMoveSpeed && targetMoveSpeed - _movement.CurMoveSpeed < speedOffset ||
            targetMoveSpeed < _movement.CurMoveSpeed && _movement.CurMoveSpeed - targetMoveSpeed < speedOffset)
            _movement.CurMoveSpeed = targetMoveSpeed;

        moveInput.Normalize();
        _movement.TargetDirection = new Vector3(moveInput.x, moveInput.y, 0f);
    }

    protected override void CalculateLookRotation()
    {
        Vector2 lookInput = _input.LookInput == Vector2.zero ? _input.MoveInput.normalized : _input.LookInput.normalized;
        if (lookInput != Vector2.zero)
        {
            _movement.TargetAngle = Mathf.Atan2(lookInput.y, lookInput.x) * Mathf.Rad2Deg;
            _movement.TargetRotation = Quaternion.Slerp(_movement.TargetRotation, Quaternion.AngleAxis(_movement.TargetAngle - 90, Vector3.forward),
                _movement.RotationSpeed * Time.deltaTime * Managers.Time.PlayerTimeScale);
        }
    }

    protected override void CheckRotationing()
    {
        _movement.IsRotationing = _input.LookInput != Vector2.zero || _input.MoveInput != Vector2.zero;
        if (_movement.IsRotationing)
        {
            if (_movement.TargetAngle < 90f && _movement.TargetAngle > -90f)
            {
                Weapon.localScale = new Vector3(1, 1, 1);
            }
            else if (_movement.TargetAngle >= 90f && _movement.TargetAngle <= 180f ||
                _movement.TargetAngle <= -90f && _movement.TargetAngle > -180f)
            {
                Weapon.localScale = new Vector3(1, -1, 1);
            }
        }
    }

    #endregion

    private void CheckWall_LookLight()
    {
        // 시야 빛 활성화
        if (null != Physics2D.OverlapCircle(LookLightTF.position, 0.1f, _movement.WallLayer))
            _lookLight.enabled = false;
        else
            _lookLight.enabled = true;
    }

    private void CheckAttackInput()
    {
        bool prev = _damageDealer.isAttacking;
        _damageDealer.isAttacking = _input.LookInput != Vector2.zero;

        if (prev != _damageDealer.isAttacking)
        {
            OnAimObject?.Invoke(_damageDealer.isAttacking);
        }
    }
}

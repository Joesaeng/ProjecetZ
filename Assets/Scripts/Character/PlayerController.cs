using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(MyPlayerInput))]
public class PlayerController : MonoBehaviour
{
    MyPlayerInput _input;
    Transform _body;

    [SerializeField] LayerMask WallLayer;

    [Header("Player Setting")]
    [SerializeField,Tooltip("플레이어가 바라보는 방향을 지정할 트랜스폼 오브젝트")] Transform LookTF;
    [SerializeField,Tooltip("플레이어 시야 오브젝트 트랜스폼")] Transform LookLightTF;
    Light2D _lookLight;
    [SerializeField,Tooltip("벽과의 충돌처리할 발의 위치 트랜스폼 오브젝트")] Transform FootPoint;
    [SerializeField,Tooltip("벽과의 충돌 감지 원 반지름")] float FootRadius; 
    public float MoveSpeed;
    public float SpeedChangeRate;
    public float RotationSpeed;

    public float curMoveSpeed;

    private Vector3 targetDirection;
    private Quaternion targetRotation;

    private Vector3 bodyScaleLeftLook;
    private Vector3 bodyScaleRightLook;

    public float TargetAngle;

    private void Start()
    {
        _input = GetComponent<MyPlayerInput>();
        _body = gameObject.FindChild("stick").transform;
        Managers.Time.SetPlayerTimeScale(1f);

        bodyScaleLeftLook = new Vector3(-7f, 7f, 1f);
        bodyScaleRightLook = new Vector3(7f, 7f, 1f);

        _lookLight = LookLightTF.GetComponent<Light2D>();
    }

    private void Update()
    {
        CalculateMove();
        CalculateLookRotation();
        CheckWall_LookLight();
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
        targetDirection = new Vector3(moveInput.x, moveInput.y, 0f);

        
    }

    private void Movement()
    {
        CheckWall_Body();
        transform.Translate(targetDirection * curMoveSpeed * Time.fixedDeltaTime);
    }

    private void CheckWall_Body()
    {
        // 이동제한
        Vector2 targetDirVec2 = new Vector2(targetDirection.x, targetDirection.y).normalized * 0.1f;
        Vector2 footPos = new Vector2(FootPoint.position.x,FootPoint.position.y) + targetDirVec2;

        if (Physics2D.CircleCast(footPos, FootRadius, targetDirection, 0.01f, WallLayer))
            curMoveSpeed = 0f;
    }

    private void CheckWall_LookLight()
    {
        // 시야 빛 활성화
        if (null != Physics2D.OverlapCircle(LookLightTF.position, 0.1f, WallLayer))
            _lookLight.enabled = false;
        else
            _lookLight.enabled = true;
    }

    private void CalculateLookRotation()
    {
        Vector2 lookInput = _input.LookInput == Vector2.zero ? _input.MoveInput.normalized : _input.LookInput.normalized;
        float targetAngle = Mathf.Atan2(lookInput.y, lookInput.x) * Mathf.Rad2Deg;
        TargetAngle = Mathf.Atan2(lookInput.y, lookInput.x) * Mathf.Rad2Deg;
        targetRotation = Quaternion.Slerp(targetRotation, Quaternion.AngleAxis(targetAngle - 90, Vector3.forward),
            RotationSpeed * Time.deltaTime * Managers.Time.PlayerTimeScale);
    }

    private void LookRotation()
    {
        if (_input.LookInput != Vector2.zero || _input.MoveInput != Vector2.zero)
        {
            LookTF.transform.rotation = targetRotation;
            if (TargetAngle < 90f && TargetAngle > -90f)
            {
                _body.localScale = bodyScaleRightLook;
            }
            else if (TargetAngle >= 90f && TargetAngle <= 180f ||
                TargetAngle <= -90f && TargetAngle > -180f)
            {
                _body.localScale = bodyScaleLeftLook;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.33f);

        Gizmos.DrawSphere(FootPoint.position + targetDirection * 0.05f, FootRadius);
    }
}

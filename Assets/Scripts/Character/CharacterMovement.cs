using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private Transform _body;
    [SerializeField] public LayerMask WallLayer;
    [SerializeField] public Transform LookTF;
    [SerializeField,Tooltip("벽과의 충돌처리할 발의 위치 트랜스폼 오브젝트")] Transform FootPoint;
    [SerializeField,Tooltip("벽과의 충돌 감지 원 반지름")] float FootRadius;

    public float MoveSpeed;
    public float SpeedChangeRate;
    public float RotationSpeed;

    public float CurMoveSpeed;

    public Vector3 TargetDirection;
    public Quaternion TargetRotation;
    public float TargetAngle;

    private Vector3 bodyScaleLeftLook;
    private Vector3 bodyScaleRightLook;

    public bool IsRotationing;

    private void Start()
    {
        IsRotationing = false;
    }

    public void SetLookBodyScale(float scale)
    {
        bodyScaleLeftLook = new Vector3(-scale, scale, 1f);
        bodyScaleRightLook = new Vector3(scale, scale, 1f);
    }

    public void SetBody(Transform bodyTF)
    {
        _body = bodyTF;
    }

    private void FixedUpdate()
    {
        Movement();
        LookRotation();
    }

    private void Movement()
    {
        CheckWall_Body();
        transform.Translate(TargetDirection * CurMoveSpeed * Time.fixedDeltaTime);
    }

    private void CheckWall_Body()
    {
        // 이동제한
        Vector2 targetDirVec2 = new Vector2(TargetDirection.x, TargetDirection.y).normalized * 0.1f;
        Vector2 footPos = new Vector2(FootPoint.position.x,FootPoint.position.y) + targetDirVec2;

        if (Physics2D.CircleCast(footPos, FootRadius, TargetDirection, 0.01f, WallLayer))
            CurMoveSpeed = 0f;
    }

    private void LookRotation()
    {
        if(IsRotationing)
        {
            LookTF.transform.rotation = TargetRotation;
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
        Gizmos.color = new Color(0, 0, 1, 0.33f);

        Gizmos.DrawWireSphere(FootPoint.position, FootRadius);
    }
}

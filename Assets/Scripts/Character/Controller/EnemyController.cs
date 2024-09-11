using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Purchasing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Windows;

public class EnemyController : MoveableCharacter, IDamageable
{
    [SerializeField] private EnemyStatus enemyStatus;
    public BaseStatus Status => enemyStatus;

    CharacterMovement _movement;
    Animator _animator;

    Vector3 moveDirection;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _movement = GetComponent<CharacterMovement>();

        InitCharacterMovement();
        AssignDamageable();
        AssignNoiseListener();
    }

    private void InitCharacterMovement()
    {
        _movement.MoveSpeed = enemyStatus.MoveSpeed;
        _movement.SetBody(transform);
        _movement.SetLookBodyScale(1);
    }

    private void AssignDamageable()
    {
        Damageable damageable = GetComponent<Damageable>();
        damageable.OnDead += HandleDead;
        damageable.OnKnockback += HandleKnockback;
    }

    private void AssignNoiseListener()
    {
        NoiseListener listener = GetComponentInChildren<NoiseListener>();
        listener.OnListenNoise += HandleListenNoise;
    }
    
    public void HandleListenNoise(Vector3 targetPos)
    {
        // JPS+ 경로 탐색을 시작

        //if (path != null && path.Count > 0)
        //{
        //    // UniTask를 사용해 경로를 따라 이동
        //    FollowPath(path).Forget();
        //}
    }

    // UniTask로 경로를 따라 이동
    //private async UniTaskVoid FollowPath(List<GridNode> path)
    //{
    //    foreach (var node in path)
    //    {
    //        // 각 노드까지 이동
    //        while (Vector3.Distance(transform.position, node.worldPosition) > 0.1f)
    //        {
    //            // 노드 방향으로 이동
    //            Vector3 moveDirection = (node.worldPosition - transform.position).normalized;

    //            // 로컬 충돌 회피 적용
    //            moveDirection = _localAvoidance.GetAvoidanceDirection(transform.position, moveDirection);

    //            // 이동 방향을 CharacterMovement에 전달
    //            _movement.TargetDirection = moveDirection;

    //            // 이동 속도에 맞게 이동
    //            _movement.CurMoveSpeed = enemyStatus.MoveSpeed;

    //            // 프레임마다 이동 업데이트
    //            await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
    //        }
    //    }

    //    // 경로 끝까지 이동 완료 후 속도를 0으로 설정
    //    _movement.CurMoveSpeed = 0f;
    //}

    public void HandleDead()
    {
        // TODO
        // 플레이어에게 경험치를 준다거나, 아이템을 떨군다거나 함
        // 죽는 애니메이션 실행 후
        // 오브젝트를 시체로 변경하여 메모리 최소화
        
    }

    public void HandleKnockback(Vector3 dealerPos, float knockbackPower)
    {
        // TODO
    }

    protected override void CalculateLookRotation()
    {
        Vector2 lookVec = moveDirection == Vector3.zero ? Vector2.zero : moveDirection;
        if(lookVec != Vector2.zero)
        {
            _movement.TargetAngle = Mathf.Atan2(lookVec.y, lookVec.x) * Mathf.Rad2Deg;
            _movement.TargetRotation = Quaternion.Slerp(_movement.TargetRotation, Quaternion.AngleAxis(_movement.TargetAngle - 90, Vector3.forward),
                _movement.RotationSpeed * Time.deltaTime * Managers.Time.GameTimeScale);
        }
    }

    protected override void CalculateMove()
    {
        float targetMoveSpeed = moveDirection == Vector3.zero ? 0f : _movement.MoveSpeed;

        float speedOffset = 0.1f;
        _movement.CurMoveSpeed = Mathf.Lerp(_movement.CurMoveSpeed, targetMoveSpeed, _movement.SpeedChangeRate * Time.deltaTime * Managers.Time.GameTimeScale);

        if (targetMoveSpeed > _movement.CurMoveSpeed && targetMoveSpeed - _movement.CurMoveSpeed < speedOffset ||
            targetMoveSpeed < _movement.CurMoveSpeed && _movement.CurMoveSpeed - targetMoveSpeed < speedOffset)
            _movement.CurMoveSpeed = targetMoveSpeed;

        _movement.TargetDirection = Vector3.Lerp(_movement.TargetDirection, moveDirection, _movement.RotationSpeed * Time.deltaTime * Managers.Time.GameTimeScale);
    }

    protected override void CheckRotationing()
    {
        _movement.IsRotationing = moveDirection != Vector3.zero;
    }
}

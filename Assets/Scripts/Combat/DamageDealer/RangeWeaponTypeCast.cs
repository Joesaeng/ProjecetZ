using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class RangeWeaponTypeCast : RangeWeapon
{
    private RangeWeaponTypeCastStatus status;

    public override BaseDamageDealerStatus Status { get => status; set => status = (RangeWeaponTypeCastStatus)value; }

    private RaycastHit2D[] cachedRaycastHits = new RaycastHit2D[ConstantData.MaxRayCastTargetCount];

    public int RecurciveTargetCount = 10;

    //public override async UniTask InAttack(CancellationToken token)
    //{
    //    try
    //    {
    //        while (!token.IsCancellationRequested)
    //        {
    //            if (isAttacking)
    //            {
    //                // 바라보고 있는 경로 상에 타겟이 있을 경우에만 공격 실행
    //                RaycastHit2D hit = Physics2D.Raycast(owner.position, owner.GetDirection(), Status.AttackRange, targetLayer);
    //                if (hit)
    //                {
    //                    SetAimPos(Vector3.Distance(owner.transform.position, hit.transform.position));
    //                    AttackImpact();
    //                }
    //                else
    //                    SetAimPos(Status.AttackRange);
    //                // 공격 범위 안에 타겟이 있을 경우에만 공격 실행
    //                // if (Physics2D.OverlapCircle(owner.position, status.AttackRange * 0.5f, targetLayer))
    //                // {
    //                //     AttackImpact();
    //                // }
    //                // else
    //                // SetAimPos(Status.AttackRange);
    //                // 인풋이 들어온 경우에는 공격 실행
    //                // AttackImpact();
    //                await UniTask.Delay(System.TimeSpan.FromSeconds(Status.AttackDelay), cancellationToken: token);
    //            }
    //            else
    //            {
    //                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: token);
    //            }
    //        }
    //    }
    //    catch (OperationCanceledException) { };
    //}

    // 공격이 실제로 실행될 때 호출
    public override void AttackImpact()
    {
        DamageMessage newDmg = Status.DamageMessage;
        newDmg.dealerPos = owner.position;

        Muzzle.ShowMuzzle().Forget();

        CastNoise(status.NoiseStrength);
        if (status.PierceCount > RecurciveTargetCount)
        {
            FireRaycastNonAlloc(newDmg).Forget();
        }
        else
        {
            HashSet<Collider2D> piercedTargets = new HashSet<Collider2D>();
            
            FireRaycastRecurcive(owner.transform.position, owner.GetDirection(), status.AttackRange, status.PierceCount, piercedTargets, newDmg).Forget();
        }
        
    }

    ///  레이캐스트를 재귀적으로 호출하여 관통가능한 데미지를 준다.
    private async UniTask FireRaycastRecurcive(Vector3 origin, Vector2 direction, float remainingDistance, int pierceCount, HashSet<Collider2D> piercedTargets, DamageMessage damageMessage)
    {
        if (pierceCount <= 0 || remainingDistance <= 0)
        {
            return;
        }

        RaycastHit2D hit = Physics2D.Raycast(origin,direction,remainingDistance,targetLayer);
        Debug.DrawRay(origin, direction * remainingDistance, Color.red, 0.1f);
        if (hit)
        {
            if (piercedTargets.Contains(hit.collider))
            {
                Vector2 newOrigin2 = hit.point + direction.normalized * 0.1f;
                await FireRaycastRecurcive(newOrigin2, direction, remainingDistance, pierceCount, piercedTargets, damageMessage);
                return;
            }

            piercedTargets.Add(hit.collider);

            var damageable = Managers.Damageable.GetDamageable(hit.collider);
            if (damageable != null)
            {
                damageable.TakeDamage(damageMessage);
            }

            remainingDistance -= hit.distance;
            pierceCount--;
            damageMessage.damage *= 1 - status.DamageReduceByPierce;

            float colliderSize = GetColliderSizeInDirection(hit.collider,direction);
            Vector2 newOrigin = hit.point + direction.normalized * colliderSize;

            Vector2 closestPoint = hit.collider.ClosestPoint(newOrigin);
            newOrigin = closestPoint + direction.normalized * 0.1f;

            remainingDistance -= colliderSize;

            if (remainingDistance > 0.2f)
            {
                await UniTask.Yield(PlayerLoopTiming.Update);
                await FireRaycastRecurcive(newOrigin, direction, remainingDistance, pierceCount, piercedTargets, damageMessage);
            }
        }
    }

    // Collider2D 크기 계산 함수
    private float GetColliderSizeInDirection(Collider2D collider, Vector2 direction)
    {
        if (collider is CapsuleCollider2D capsuleCollider)
        {
            // CapsuleCollider2D의 경우, 방향과 반지름을 고려해 크기를 계산
            float radius = capsuleCollider.size.x / 2f;
            float height = capsuleCollider.size.y / 2f;
            return Mathf.Abs(Vector2.Dot(direction.normalized, new Vector2(radius, height)));
        }
        else
        {
            // 기본적으로는 Bounds를 사용
            Bounds bounds = collider.bounds;
            Vector3 extents = bounds.extents;
            float sizeInDirection = Vector2.Dot(extents, direction.normalized);
            return Mathf.Abs(sizeInDirection); // 절대값 반환
        }
    }

    // NonAlloc을 이용한 관통가능 데미지를 줌
    private async UniTask FireRaycastNonAlloc(DamageMessage damageMessage)
    {
        int hits = Physics2D.RaycastNonAlloc(owner.position,owner.GetDirection(),cachedRaycastHits,status.AttackRange,targetLayer);

        if(hits > 0)
        {
            System.Array.Sort(cachedRaycastHits, 0, hits, new RaycastHit2DComparer());
            SetAimPos(Vector3.Distance(owner.transform.position, cachedRaycastHits[0].point));

            for (int i = 0; i < status.PierceCount && i < hits; ++i)
            {
                var hit = cachedRaycastHits[i];
                
                var damageable = Managers.Damageable.GetDamageable(hit.collider);
                if(damageable != null)
                {
                    damageable.TakeDamage(damageMessage);
                }

                damageMessage.damage *= 1 - status.DamageReduceByPierce;

                await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
            }
        }
    }

    private class RaycastHit2DComparer : IComparer<RaycastHit2D>
    {
        public int Compare(RaycastHit2D x, RaycastHit2D y)
        {
            return x.distance.CompareTo(y.distance); // 거리 순으로 정렬
        }
    }
}

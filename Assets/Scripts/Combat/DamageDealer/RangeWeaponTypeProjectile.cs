using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class RangeWeaponTypeProjectile : RangeWeapon
{
    private Transform WeaponTip;
    private RangeWeaponTypeProjectileStatus status;

    public override BaseDamageDealerStatus Status { get => status; set => status = (RangeWeaponTypeProjectileStatus)value; }

    //public override async UniTask InAttack(CancellationToken token)
    //{
    //    try
    //    {
    //        while (!token.IsCancellationRequested)
    //        {
    //            if (isAttacking)
    //            {
    //                // 바라보고 있는 경로 상에 타겟이 있을 경우에만 공격 실행
    //                if (Physics2D.Raycast(owner.position, owner.GetDirection(), Status.AttackRange, targetLayer))
    //                {
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

    public override void AttackImpact()
    {
        RaycastHit2D hit = Physics2D.Raycast(owner.transform.position,owner.GetDirection(),Status.AttackRange,targetLayer);

        if(hit)
        {
            CastNoise(status.NoiseStrength);

            DamageMessage newDmg = Status.DamageMessage;
            newDmg.dealerPos = owner.position;
            Muzzle.ShowMuzzle().Forget();

            GameObject projectileObj = Managers.Resource.Instantiate("Projectile", WeaponTip.position);
            Managers.CompCache.GetOrAddComponentCache<Projectile>(projectileObj, out var projectile);
            projectile.Init(owner,owner.GetDirection(), status, targetLayer);
        }
    }

    public override void Init()
    {
        base.Init();
        WeaponTip = gameObject.FindChild<Transform>("WeaponTip");
    }
}

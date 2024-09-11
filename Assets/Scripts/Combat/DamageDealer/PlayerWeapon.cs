using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public abstract class PlayerWeapon : DamageDealer
{
    private NoiseCaster noiseCaster;

    protected override void Start()
    {
        base.Start();
        noiseCaster = GetComponent<NoiseCaster>();
    }

    public override void SetTargetLayer()
    {
        targetLayer = LayerMask.GetMask("Enemy");
    }

    protected void CastNoise(Vector3 pos, float strength)
    {
        noiseCaster.CastNoise(pos, strength);
    }

    protected void CastNoise(float strength)
    {
        noiseCaster.CastNoise(strength);
    }
}

public abstract class RangeWeapon : PlayerWeapon
{
    protected Transform Aim;
    protected Muzzle Muzzle;

    public override void Init()
    {
        owner = transform.parent;
        Aim = owner.gameObject.FindChild<Transform>("Aim", true);
        Muzzle = GetComponentInChildren<Muzzle>();
    }

    protected void SetAimPos(float distance)
    {
        Aim.transform.localPosition = new Vector3(0,distance, 0);
    }

    protected override void Start()
    {
        base.Start();
        owner.GetComponentInParent<PlayerController>().OnAimObject += HandleOnAimObject;
    }

    private void HandleOnAimObject(bool value)
    {
        Aim.gameObject.SetActive(value);

        SetAimPos(Status.AttackRange);
    }

    protected override async UniTask InAttack(CancellationToken token)
    {
        try
        {
            while (!token.IsCancellationRequested)
            {
                if (isAttacking)
                {
                    // 바라보고 있는 경로 상에 타겟이 있을 경우에만 공격 실행
                    RaycastHit2D hit = Physics2D.Raycast(owner.position, owner.GetDirection(), Status.AttackRange, targetLayer);
                    if (hit)
                    {
                        SetAimPos(Vector3.Distance(owner.transform.position, hit.transform.position));
                        AttackImpact();
                    }
                    else
                        SetAimPos(Status.AttackRange);
                    // 공격 범위 안에 타겟이 있을 경우에만 공격 실행
                    // if (Physics2D.OverlapCircle(owner.position, status.AttackRange * 0.5f, targetLayer))
                    // {
                    //     AttackImpact();
                    // }
                    // else
                    // SetAimPos(Status.AttackRange);
                    // 인풋이 들어온 경우에는 공격 실행
                    // AttackImpact();
                    await UniTask.Delay(System.TimeSpan.FromSeconds(Status.AttackDelay), cancellationToken: token);
                }
                else
                {
                    await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: token);
                }
            }
        }
        catch (OperationCanceledException) { };
    }
}

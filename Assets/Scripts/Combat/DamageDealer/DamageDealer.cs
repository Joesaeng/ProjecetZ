using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;

public abstract class DamageDealer : MonoBehaviour
{
    public bool isAttacking;

    public abstract BaseDamageDealerStatus Status { get; set; }

    protected Transform owner;

    protected LayerMask targetLayer;

    private CancellationTokenSource cancelToken;

    protected virtual async UniTask InAttack(CancellationToken token)
    {
        try
        {
            while (!token.IsCancellationRequested)
            {
                if (Physics2D.Raycast(owner.position, owner.GetDirection(), Status.AttackRange, targetLayer))
                {
                    AttackImpact();
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

    public abstract void AttackImpact();

    public void SetDamageDealerStatus<T>(T status) where T : BaseDamageDealerStatus
    {
        Status = (T)status;
    }


    public abstract void SetTargetLayer();

    public abstract void Init();

    protected virtual void Start()
    {
        SetTargetLayer();
        Init();

        InAttack(cancelToken.Token).Forget();
    }

    private void OnEnable()
    {
        cancelToken = new();
    }

    private void OnDisable()
    {
        cancelToken?.Cancel();
    }
}

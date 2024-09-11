using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class InteractionChecker : MonoBehaviour
{
    public float InteractionCheckDelay = 0.1f;
    public float InteractionRadius;

    private LayerMask InteractionableLayer;
    private Interactionable curInteractionable;
    private PlayerController playerController;

    public Interactionable CurInteractionable => curInteractionable;

    public Action<bool> OnInteractionable;

    private CancellationTokenSource cancelToken;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        InteractionableLayer = LayerMask.GetMask("Interactionable");
    }

    private void OnEnable()
    {
        cancelToken = new();
        CheckInteraction(cancelToken.Token).Forget();
    }

    private void OnDisable()
    {
        cancelToken?.Cancel();
    }

    private async UniTask CheckInteraction(CancellationToken token)
    {
        try
        {
            while (!token.IsCancellationRequested)
            {
                Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position,InteractionRadius,InteractionableLayer);

                Collider2D closestCollider = cols.OrderBy(col => Vector3.Distance(transform.position,col.transform.position))
                    .FirstOrDefault();

                if (closestCollider != null && closestCollider.TryGetComponent(out Interactionable interactionable))
                {
                    // 현재 상호작용 중인 오브젝트와 새로운 상호작용 가능한 오브젝트가 다를 때만 변경
                    if (curInteractionable != interactionable)
                    {
                        curInteractionable?.SetCanInteration(0f); // 이전 오브젝트 비활성화
                        curInteractionable = interactionable;
                        curInteractionable.SetCanInteration(0.007f); // 새 오브젝트 활성화
                        OnInteractionable?.Invoke(true);
                    }
                }
                else if (curInteractionable != null)
                {
                    // 상호작용 가능한 오브젝트가 없을 경우
                    curInteractionable.SetCanInteration(0f);
                    curInteractionable = null;
                    OnInteractionable?.Invoke(false);
                }

                await UniTask.Delay(System.TimeSpan.FromSeconds(InteractionCheckDelay));
            }
        }
        catch (OperationCanceledException) { };

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = curInteractionable != null ? new Color(0, 1, 0, 0.33f) : new Color(1, 0, 0, 0.33f);

        Gizmos.DrawWireSphere(transform.position, InteractionRadius);
    }
}

using Cysharp.Threading.Tasks;
using UnityEngine;

public class NoiseCaster : MonoBehaviour
{
    private float noiseStrength;
    private float noiseRadius;
    private LayerMask targetLayer;
    public float LogConst;

    private void Start()
    {
        // 타겟 레이어 설정
        targetLayer = LayerMask.GetMask("NoiseListener");
    }

    // 소음 캐스트
    public void CastNoise(float noiseStrength)
    {
        SetNoise(noiseStrength);
        CastNoiseTask(transform.position).Forget();
    }

    public void CastNoise(Vector3 pos, float noiseStrength)
    {
        SetNoise(noiseStrength);
        CastNoiseTask(pos).Forget();
    }

    private void SetNoise(float noiseStrength)
    {
        // 인자로 받아온 소음의 세기에 따라 로그를 적용하여 소음의 반경을 정한다.
        this.noiseStrength = noiseStrength;
        noiseRadius = Mathf.Log(noiseStrength + 1) * LogConst;
    }

    private float AdjustStrength(float noiseStrength, float distance)
    {
        // 인자로 받은 소음의 세기와 Listener와의 거리를 통해 Listener가 들을 소리의 세기를 정한다.
        return noiseStrength / Mathf.Max(distance,1);
    }

    // 한개의 NoiseCaster를 통해 비동기적으로 소음을 내주기 위해 UniTask를 사용했다.
    private async UniTask CastNoiseTask(Vector3 noisePos)
    {
        // OverlapCircleAll 을 통해 계산된 반경 내에 있는 Listener에게 전달한다.
        Collider2D[] cols = Physics2D.OverlapCircleAll(noisePos,noiseRadius,targetLayer);
        foreach(var col in cols)
        {
            NoiseListener listener = Managers.Noise.GetListener(col); // Listener를 캐시해둔 매니저
            if(listener != null)
            {
                float distance = Vector3.Distance(noisePos, col.transform.position);
                float adjustStrength = AdjustStrength(noiseStrength, distance);
                listener.ListenNoise(noisePos, adjustStrength);
            }
        }
        await UniTask.Delay(System.TimeSpan.FromSeconds(0.1f));
        // TEMP
        // Managers.Resource.Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, noiseRadius);
    }
}

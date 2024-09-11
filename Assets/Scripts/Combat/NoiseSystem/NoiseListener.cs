using System;
using UnityEngine;

// 소음을 듣는 컴포넌트
[RequireComponent(typeof(Collider2D))]
public class NoiseListener : MonoBehaviour
{
    private float sensitivity;

    public Action<Vector3> OnListenNoise;

    private void Start()
    {
        EnemyStatus status = (EnemyStatus)GetComponentInParent<EnemyController>().Status;
        sensitivity = status.NoiseListenSensitivity;
        Managers.Noise.AddListener(GetComponent<Collider2D>(), this);
    }
    public void ListenNoise(Vector3 noisePos, float noiseStrength)
    {
        if(noiseStrength > 1 / sensitivity)
        {
            RespondToNoise(noisePos);
        }
    }

    private void RespondToNoise(Vector3 noisePos)
    {
        OnListenNoise?.Invoke(noisePos);
    }
}

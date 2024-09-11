using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatus", menuName = "ScriptableObject/EnemyStatus")]
public class EnemyStatus : BaseStatus
{
    [SerializeField]private float damage;
    [SerializeField]private float moveSpeed;
    [SerializeField]private float noiseListenSensitivity;

    public float Damage => damage;
    public float MoveSpeed => moveSpeed;
    public float NoiseListenSensitivity => noiseListenSensitivity;
}

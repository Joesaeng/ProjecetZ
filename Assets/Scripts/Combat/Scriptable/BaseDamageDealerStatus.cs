using System.Collections;
using System.Collections.Generic;
using UnityEditor.Purchasing;
using UnityEngine;

public abstract class BaseDamageDealerStatus : ScriptableObject
{
    [SerializeField] private float damage;
    [SerializeField] private float knockbackPower;
    [SerializeField] private float attackDelay;
    [SerializeField] private float attackRange;

    public DamageMessage DamageMessage => new DamageMessage { damage = damage, knockbackPower = knockbackPower };
    public float AttackRange => attackRange;
    public float AttackDelay => attackDelay;

    /// <summary>
    /// Status에 맞는 DamageDealer 컴포넌트를 owner에 부착합니다.
    /// </summary>
    public abstract void AddDamageDealerComponentByType(Transform owner);
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RangeWeaponTypeProjectileStatus", menuName = "ScriptableObject/RangeWeaponTypeProjectileStatus")]
public class RangeWeaponTypeProjectileStatus : RangeWeaponStatus
{
    [SerializeField] private float projectileSpeed = 3f;
    [SerializeField] private float projectileRadius;
    [SerializeField] private float projectileCount;

    public float ProjectileSpeed => projectileSpeed;
    public float ProjectileRadius => projectileRadius;
    public float ProjectileCount => projectileCount;

    public override void AddDamageDealerComponentByType(Transform owner)
    {
        owner.GetOrAddComponent<RangeWeaponTypeProjectile>();
    }
}

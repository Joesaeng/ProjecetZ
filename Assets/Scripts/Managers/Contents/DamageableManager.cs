using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableManager
{
    private Dictionary<Collider2D, Damageable> damageableDict = new();

    public void AddDamageable(Collider2D collider, Damageable damageable) => 
        damageableDict.Add(collider, damageable);

    public Damageable GetDamageable(Collider2D collider) => damageableDict[collider];

    public void DeadDamageable(Damageable damageable)
    {
        Managers.Resource.Destroy(damageable.gameObject);

        // TODO
        // 사망 애니메이션, 이펙트 등
        // float deadDelay = 1f;
        // Managers.Resource.DestroyDelay(damageable.gameObject,deadDelay);
    }
}

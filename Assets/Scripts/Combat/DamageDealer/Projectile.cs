using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Transform RendererTF;

    Vector3 startPos;
    Vector2 direction;
    DamageMessage damageMessage;
    float projectileSpeed;
    float projectileRadius;
    int curPierceCount;

    LayerMask targetLayer;
    HashSet<Collider2D> piercedTargets;

    RangeWeaponTypeProjectileStatus Status;

    public void Init(Transform owner, Vector2 direction, RangeWeaponTypeProjectileStatus Status, LayerMask targetLayer)
    {
        this.Status = Status;
        this.startPos = owner.position;
        RendererTF.rotation = owner.rotation;
        this.direction = direction;
        this.projectileSpeed = Status.ProjectileSpeed;
        this.damageMessage = Status.DamageMessage;
        this.targetLayer = targetLayer;
        this.curPierceCount = Status.PierceCount;

        damageMessage.dealerPos = owner.position;
        piercedTargets = new HashSet<Collider2D>();

    }

    void FixedUpdate()
    {
        MoveProjectile();
        Collider2D hit = Physics2D.OverlapCircle(transform.position, projectileRadius, targetLayer);
        if(hit != null)
        {
            if(piercedTargets.Contains(hit))
            {
                return;
            }

            piercedTargets.Add(hit);

            var damageable = Managers.Damageable.GetDamageable(hit);
            if (damageable != null)
            {
                damageable.TakeDamage(damageMessage);
            }

            curPierceCount--;
            damageMessage.damage *= 1 - Status.DamageReduceByPierce;

            if (curPierceCount <= 0)
                Managers.Resource.Destroy(gameObject);
        }
    }

    private void MoveProjectile()
    {
        if(Vector3.Distance(transform.position, startPos) > Status.AttackRange)
            Managers.Resource.Destroy(gameObject);

        transform.Translate(direction * projectileSpeed * Time.fixedDeltaTime);
    }
}

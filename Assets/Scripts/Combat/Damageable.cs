using System;
using UnityEngine;

public struct DamageMessage
{
    public float damage;
    public Vector3 dealerPos;
    public float knockbackPower;
}

public interface IDamageable
{
    public BaseStatus Status { get; }
    public void HandleDead();
    public void HandleKnockback(Vector3 dealerPos,float knockbackPower);
}

[RequireComponent(typeof(Collider2D))]
public class Damageable : MonoBehaviour
{
    BaseStatus status;
    Collider2D hitBox;

    int curHp;

    public bool IsDead;
    public Action<Vector3,float> OnKnockback;
    public Action OnDead;

    private void Start()
    {
        status = transform.GetComponent<IDamageable>().Status;
        hitBox = transform.GetComponent<Collider2D>();
        Managers.Damageable.AddDamageable(hitBox, this);
        curHp = status.MaxHp;
    }

    public void TakeDamage(DamageMessage msg)
    {
        GameObject damageTextObj = Managers.Resource.Instantiate("DamageText");
        Managers.CompCache.GetOrAddComponentCache<DamageText>(damageTextObj, out var effectComp);

        if (!effectComp.isInit)
            effectComp.Init();

        msg.damage = Mathf.RoundToInt(msg.damage);

        effectComp.ShowDamageEffect(transform.position + Vector3.up, msg, 1f);

        curHp -= (int)msg.damage;
        if(curHp < 0)
        {
            hitBox.enabled = false;
            OnDead?.Invoke();
        }

        OnKnockback?.Invoke(msg.dealerPos, msg.knockbackPower);
    }


}

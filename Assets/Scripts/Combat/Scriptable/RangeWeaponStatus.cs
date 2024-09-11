using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangeWeaponStatus : PlayerWeaponStatus
{
    [SerializeField] private int pierceCount = 1;
    [SerializeField] private float damageReduceByPierce = 0.2f;

    public int PierceCount => pierceCount;
    public float DamageReduceByPierce => damageReduceByPierce;
}

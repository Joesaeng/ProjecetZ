using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerWeaponStatus : BaseDamageDealerStatus
{
    [SerializeField] float noiseStrength;

    public float NoiseStrength => noiseStrength;
}

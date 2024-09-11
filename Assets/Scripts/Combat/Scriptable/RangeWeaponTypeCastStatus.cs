using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RangeWeaponTypeCastStatus", menuName = "ScriptableObject/RangeWeaponTypeCastStatus")]
public class RangeWeaponTypeCastStatus : RangeWeaponStatus
{
    public override void AddDamageDealerComponentByType(Transform owner)
    {
        owner.GetOrAddComponent<RangeWeaponTypeCast>();
    }
}

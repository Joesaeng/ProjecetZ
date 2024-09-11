using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStatus : ScriptableObject
{
    [SerializeField]private int maxHp;

    public int MaxHp { get => maxHp; }
}



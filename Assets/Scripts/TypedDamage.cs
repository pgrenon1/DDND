using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
    Impact,
    Fire,
    Ice,
    Wind,
    Earth
}

public class TypedDamage
{
    [HorizontalGroup("Damage")]
    public DamageType damageType;
    [HorizontalGroup("Damage")]
    public float value;

    public TypedDamage() { }

    public TypedDamage(DamageType damageType, float value)
    {
        this.damageType = damageType;
        this.value = value;
    }
}
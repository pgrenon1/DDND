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
    public ScalableFloat scalableValue;

    public TypedDamage() { }

    public TypedDamage(TypedDamage typedDamage)
    {
        damageType = typedDamage.damageType;
        scalableValue = new ScalableFloat(typedDamage.scalableValue);
    }

    public TypedDamage(DamageType damageType, ScalableFloat scalableValue)
    {
        this.damageType = damageType;
        this.scalableValue = scalableValue;
    }
}
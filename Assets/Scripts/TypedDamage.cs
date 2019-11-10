using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
    Fire,
    Ice,
    Wind,
    Earth
}

public class TypedDamage
{
    public DamageType damageType;
    public float value;
}
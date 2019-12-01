using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage
{
    public DamageData DamageData { get; set; }
    public Targetable Source { get; set; }
    public float ScalingFactor { get; set; }

    public Damage() { }

    public Damage(DamageData damageData)
    {
        DamageData = damageData;
    }

    public Damage(DamageData damageData, Targetable source, float scalingFactor)
    {
        DamageData = damageData;
        Source = source;
        ScalingFactor = scalingFactor;
    }
}

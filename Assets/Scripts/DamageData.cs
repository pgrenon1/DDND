using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DamageData
{
    public List<TypedDamage> typedDamages = new List<TypedDamage>();

    public DamageData() { }

    public DamageData(DamageData damageData)
    {
        var copy = new DamageData();

        foreach (var typedDamage in typedDamages)
        {
            copy.typedDamages.Add(new TypedDamage(typedDamage.damageType, typedDamage.value));
        }
    }

    public void Scale(float scalingFactor)
    {
        foreach (var typedDamage in typedDamages)
        {
            typedDamage.value *= scalingFactor;
        }
    }
}

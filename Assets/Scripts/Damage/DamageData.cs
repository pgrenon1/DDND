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
        foreach (var typedDamage in damageData.typedDamages)
        {
            typedDamages.Add(new TypedDamage(typedDamage));
        }
    }

    //public void Scale(float scalingFactor)
    //{
    //    foreach (var typedDamage in typedDamages)
    //    {
    //        typedDamage.value *= scalingFactor;
    //    }
    //}
}

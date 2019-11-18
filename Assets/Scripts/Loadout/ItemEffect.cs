using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEffect
{
    public Item Item { get; set; }
    public bool IsActive { get; set; }

    public virtual void Apply(float noteScore) { }
}

[System.Serializable]
public class DamageItemEffect : ItemEffect
{
    public DamageData damageData;

    public override void Apply(float noteScore)
    {
        base.Apply(noteScore);

        damageData.Scale(noteScore);

        Item.Owner.PlayerMenu.CurrentTarget.Damageable.TryDamage(damageData);
    }
}

[System.Serializable]
public class SlowItemEffect : ItemEffect
{

}

[System.Serializable]
public class EnergyGainItemEffect : ItemEffect
{
    public float energyGainPerNote = 1f;

    public override void Apply(float noteScore)
    {
        base.Apply(noteScore);

        var energyGain = energyGainPerNote * noteScore;

        Item.Owner.Energy += energyGainPerNote;
    }
}

public class HealItemEffect : ItemEffect
{
    public float healPerNote = 1f;

    public override void Apply(float noteScore)
    {
        base.Apply(noteScore);

        var healAmount = healPerNote * noteScore;

        Item.Owner.PlayerMenu.CurrentTarget.Damageable.TryHeal(healAmount);
    }
}
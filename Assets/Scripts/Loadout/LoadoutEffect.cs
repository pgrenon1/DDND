using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LoadoutEffect
{
    public LoadoutSlotElement LoadoutObject { get; set; }
    public bool IsActive { get; set; }

    public virtual void Apply(float scale) { }
}

[System.Serializable]
public class DamageLoadoutEffect : LoadoutEffect
{
    public DamageData damageData;

    public override void Apply(float scalingFactor)
    {
        base.Apply(scalingFactor);

        var target = LoadoutObject.Owner.CurrentTarget;
        var damageable = target.Damageable;
        damageable.TryDamage(new Damage(damageData, LoadoutObject.Owner, scalingFactor));
    }
}

[System.Serializable]
public class SlowLoadoutEffect : LoadoutEffect
{

}

[System.Serializable]
public class EnergyGainLoadoutEffect : LoadoutEffect
{
    public float energyGainPerNote = 1f;

    public override void Apply(float scalingFactor)
    {
        base.Apply(scalingFactor);

        var energyGain = energyGainPerNote * scalingFactor;

        LoadoutObject.Owner.Energy += energyGainPerNote;
    }
}

[System.Serializable]
public class HealLoadoutEffect : LoadoutEffect
{
    public float healAmount = 1f;

    public override void Apply(float scalingFactor)
    {
        base.Apply(scalingFactor);

        var amount = this.healAmount * scalingFactor;

        LoadoutObject.Owner.CurrentTarget.Damageable.TryHeal(amount);
    }
}
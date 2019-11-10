using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEffect
{
    public virtual void Apply() { }
}

[System.Serializable]
public class TypedDamageItemEffect : ItemEffect
{

}

[System.Serializable]
public class SlowItemEffect : ItemEffect
{

}

[System.Serializable]
public class EnergyMultiplierItemEffect : ItemEffect
{

}
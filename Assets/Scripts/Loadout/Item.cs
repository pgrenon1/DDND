using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : LoadoutObject
{
    [System.NonSerialized, OdinSerialize]
    public List<ItemEffect> itemEffects = new List<ItemEffect>();

    public Item(ItemData itemData)
    {
        objectName = itemData.objectName;
        description = itemData.description;
        sprite = itemData.sprite;
        foreach (var itemEffect in itemData.itemEffects)
        {
            itemEffects.Add(itemEffect);
            itemEffect.Item = this;
        }
    }

    public override void Trigger(float noteScore)
    {
        foreach (var itemEffect in itemEffects)
        {
            itemEffect.Apply(noteScore);
        }
    }

    public List<T> GetActiveItemEffectsOfType<T>() where T : ItemEffect
    {
        var itemEffectsOfType = new List<T>();

        foreach (var itemEffect in itemEffects)
        {
            if (itemEffect is T && itemEffect.IsActive)
            {
                itemEffectsOfType.Add(itemEffect as T);
            }
        }

        return itemEffectsOfType;
    }
}
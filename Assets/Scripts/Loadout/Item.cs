using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : LoadoutSlotElement
{
    [System.NonSerialized, OdinSerialize]
    public List<LoadoutEffect> itemEffects = new List<LoadoutEffect>();

    public Item(ItemData itemData)
    {
        elementName = itemData.objectName;
        description = itemData.description;
        sprite = itemData.sprite;
        foreach (var itemEffect in itemData.itemEffects)
        {
            itemEffects.Add(itemEffect);
            itemEffect.LoadoutObject = this;
        }
    }

    public override void ScoreNote(float noteScore)
    {
        // Apply Item Effects
        foreach (var itemEffect in itemEffects)
        {
            itemEffect.Apply(noteScore);
        }
    }

    public List<T> GetActiveItemEffectsOfType<T>() where T : LoadoutEffect
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
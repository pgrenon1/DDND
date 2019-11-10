using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ShowOdinSerializedPropertiesInInspector, CreateAssetMenu(fileName = "Item_", menuName = "Item")]
public class Item : LoadoutObject
{
    [System.NonSerialized, OdinSerialize]
    public List<ItemEffect> itemEffects = new List<ItemEffect>();
}
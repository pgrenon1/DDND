using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ShowOdinSerializedPropertiesInInspector, CreateAssetMenu(fileName = "Item_", menuName = "Item")]
public class ItemData : LoadoutObjectData
{
    [System.NonSerialized, OdinSerialize]
    public List<LoadoutEffect> itemEffects = new List<LoadoutEffect>();
}

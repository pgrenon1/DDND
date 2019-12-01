using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ShowOdinSerializedPropertiesInInspector, CreateAssetMenu(fileName = "Skill_", menuName = "Skill")]
public class SkillData : LoadoutObjectData
{
    public int noteCount = 10;
    [System.NonSerialized, OdinSerialize]
    public List<LoadoutEffect> effects = new List<LoadoutEffect>();
}

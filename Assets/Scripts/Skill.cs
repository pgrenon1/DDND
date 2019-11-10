using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ShowOdinSerializedPropertiesInInspector, CreateAssetMenu(fileName = "Skill_", menuName = "Skill")]
public class Skill : LoadoutObject
{
    public int noteCount = 10;
    public List<SkillEffect> skillEffects = new List<SkillEffect>();
}

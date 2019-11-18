using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : LoadoutObject
{
    public int noteCount = 10;
    public List<SkillEffect> skillEffects = new List<SkillEffect>();

    public bool IsActive { get; set; }

    public Skill(SkillData skillData)
    {
        objectName = skillData.name;
        description = skillData.description;
        sprite = skillData.sprite;
        foreach (var skillEffect in skillData.skillEffects)
        {
            skillEffects.Add(skillEffect);
        }
    }

    public override void Trigger(float noteScore)
    {
        if (!IsActive)
            return;

        base.Trigger(noteScore);

        Debug.Log("Applying Skill");
    }
}

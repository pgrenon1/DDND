using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : LoadoutSlotElement
{
    public int noteCount = 10;
    public List<LoadoutEffect> effects = new List<LoadoutEffect>();

    public bool IsActive { get; private set; }
    public int NotesLeft { get; private set; }
    public float Score { get; private set; }

    public Skill(SkillData skillData)
    {
        elementName = skillData.name;
        description = skillData.description;
        sprite = skillData.sprite;
        foreach (var effect in skillData.effects)
        {
            effects.Add(effect);
            effect.LoadoutObject = this;
        }
    }

    public void Activate()
    {
        if (IsActive)
            return;

        IsActive = true;

        NotesLeft = noteCount;
    }

    public override void ScoreNote(float noteScore)
    {
        if (!IsActive)
            return;

        base.ScoreNote(noteScore);

        Score += noteScore;
    }

    public void Resolve()
    {
        foreach (var effect in effects)
        {
            effect.Apply(Score);
        }
    }
}

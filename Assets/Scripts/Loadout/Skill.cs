using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : LoadoutSlotElement
{
    public GameObject noteVFX;
    public int noteCount = 10;
    public List<LoadoutEffect> effects = new List<LoadoutEffect>();

    public bool IsActive { get; set; }
    public float Score { get; private set; }

    public List<Note> ActiveNotes = new List<Note>();

    public Skill(SkillData skillData, Player player)
    {
        Owner = player;
        noteVFX = skillData.noteVFX;
        slotElementName = skillData.objectName;
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

        foreach (var note in Owner.Conductor.ActiveNotes)
        {
            ActiveNotes.Add(note);
        }

        Debug.Log("Activating " + this.slotElementName);
    }

    public override void ScoreNote(Note note, float noteScore)
    {
        base.ScoreNote(note, noteScore);

        Score += noteScore;

        UpdateSkillProgress(note);
    }

    BAD: notes are removed and added infinetly.
        need to check that notes are only added until 10 are added, Not until the count of activenote reach the notecount!
    public void UpdateSkillProgress(Note note)
    {
        if (ActiveNotes.Contains(note))
            ActiveNotes.Remove(note);

        if (ActiveNotes.Count == 0)
            Resolve();
    }

    public void Resolve()
    {
        Debug.Log("Resolving " + this.slotElementName);

        foreach (var effect in effects)
        {
            effect.Apply(Score);
        }

        IsActive = false;
        Owner.ActiveSkill = null;
    }
}

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
    public float MaxTimingPoint
    {
        get
        {
            return noteCount * GameManager.Instance.timingValues[Timing.Perfect];
        }
    }

    public List<Note> ActiveNotes = new List<Note>();
    public List<Timing> Timings = new List<Timing>();

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
            RegisterNote(note);
        }

        Debug.Log("Activating " + this.slotElementName);
    }

    public void RegisterNote(Note note)
    {
        ActiveNotes.Add(note);
        UnityEngine.Object.Instantiate(noteVFX, note.transform);
    }

    public override void ScoreNote(Note note, float noteScore)
    {
        if (!IsActive)
            return;

        base.ScoreNote(note, noteScore);

        Score += noteScore;

        UpdateSkillProgress(note);
    }

    //    BAD: notes are removed and added infinetly.
    //    need to check that notes are only added until 10 are added, Not until the count of activenote reach the notecount!
    public void UpdateSkillProgress(Note note)
    {
        ActiveNotes[ActiveNotes.IndexOf(note)] = null;

        Timings.Add(note.EvaluateNote());

        if (ActiveNotes.Count == noteCount && ActiveNotes.TrueForAll(n => n == null))
            Resolve();
    }

    public void Resolve()
    {
        Debug.Log("Resolving " + this.slotElementName);

        var totalTimingPoints = 0f;
        foreach (var timing in Timings)
        {
            totalTimingPoints += GameManager.Instance.GetTimingValue(timing);
        }

        foreach (var effect in effects)
        {
            effect.Apply(totalTimingPoints / MaxTimingPoint);
        }

        ActiveNotes.Clear();
        IsActive = false;
        Owner.ActiveSkill = null;
    }
}

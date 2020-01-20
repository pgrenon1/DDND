using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Timing
{
    Bad,
    Ok,
    Good,
    Perfect
}

public class Note : MonoBehaviour
{
    public float TimeStamp { get; set; }
    public Conductor Conductor { get; set; }
    public Direction Direction { get; set; }
    public Image Image { get; set; }

    private void Start()
    {
        Image = GetComponent<Image>();
    }

    public Timing EvaluateNote()
    {
        if (GameManager.Instance.SongPositionInSeconds >= TimeStamp - GameManager.Instance.timingWindows[Timing.Perfect] / 2
            && Conductor.SongPositionInSeconds <= TimeStamp + GameManager.Instance.timingWindows[Timing.Perfect] / 2)
        {
            return Timing.Perfect;
        }
        else if (GameManager.Instance.SongPositionInSeconds >= TimeStamp - GameManager.Instance.timingWindows[Timing.Good] / 2
            && Conductor.SongPositionInSeconds <= TimeStamp + GameManager.Instance.timingWindows[Timing.Good] / 2)
        {
            return Timing.Good;
        }
        else if (GameManager.Instance.SongPositionInSeconds >= TimeStamp - GameManager.Instance.timingWindows[Timing.Ok] / 2
            && Conductor.SongPositionInSeconds <= TimeStamp + GameManager.Instance.timingWindows[Timing.Ok] / 2)
        {
            return Timing.Ok;
        }
        else
        {
            return Timing.Bad;
        }
    }

    public void Travel(Vector3 targetPosition, float timeInAdvance)
    {
        transform.DOMove(targetPosition + (targetPosition - transform.position) * 0.25f, timeInAdvance * 1.25f)
            .SetEase(Ease.Linear)
            .OnComplete(() => Remove());
    }

    private void Remove()
    {
        if (Conductor.ActiveNotes.Contains(this))
        {
            Conductor.ActiveNotes.Remove(this);
        }

        if (Conductor.Player.ActiveSkill != null && Conductor.Player.ActiveSkill.ActiveNotes.Contains(this))
        {
            Conductor.Player.ActiveSkill.UpdateSkillProgress(this);
        }

        Destroy(gameObject);
    }

    public float Score(int currentComboCount)
    {
        return GameManager.Instance.GetTimingValue(EvaluateNote()) + (1 + currentComboCount * GameManager.Instance.comboValue);
    }

    public void ApplyEffects(float noteScore)
    {
        foreach (var loadoutSlot in Conductor.Player.LoadoutPanel.LoadoutSlots)
        {
            var slotElement = loadoutSlot.Key.GetPickedSlotElement<LoadoutSlotElement>();
            slotElement.ScoreNote(this, noteScore);
        }
    }
}

using DG.Tweening;
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

    public Timing Timing
    {
        get
        {
            if (Conductor.SongPositionInSeconds >= TimeStamp - Conductor.perfectWindow / 2
                && Conductor.SongPositionInSeconds <= TimeStamp + Conductor.perfectWindow / 2)
            {
                return Timing.Perfect;
            }
            else if (Conductor.SongPositionInSeconds >= TimeStamp - Conductor.goodWindow / 2
                && Conductor.SongPositionInSeconds <= TimeStamp + Conductor.goodWindow / 2)
            {
                return Timing.Good;
            }
            else if (Conductor.SongPositionInSeconds >= TimeStamp - Conductor.okWindow / 2
                && Conductor.SongPositionInSeconds <= TimeStamp + Conductor.okWindow / 2)
            {
                return Timing.Ok;
            }
            else
            {
                return Timing.Bad;
            }
        }
    }

    //public bool IsOpen
    //{
    //    get
    //    {
    //        return Conductor.SongPositionInSeconds >= TimeStamp - Conductor.okWindow / 2
    //            && Conductor.SongPositionInSeconds <= TimeStamp + Conductor.okWindow / 2;
    //    }
    //}

    //public bool IsGood
    //{
    //    get
    //    {
    //        return Conductor.SongPositionInSeconds >= TimeStamp - Conductor.goodWindow / 2
    //            && Conductor.SongPositionInSeconds <= TimeStamp + Conductor.goodWindow / 2;
    //    }
    //}

    //public bool IsPerfect
    //{
    //    get
    //    {
    //        return Conductor.SongPositionInSeconds >= TimeStamp - Conductor.perfectWindow / 2
    //            && Conductor.SongPositionInSeconds <= TimeStamp + Conductor.perfectWindow / 2;
    //    }
    //}

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

        Destroy(gameObject);
    }
}

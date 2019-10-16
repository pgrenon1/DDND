using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public float TimeStamp { get; set; }
    public Conductor Conductor { get; set; }
    public bool IsOpen
    {
        get
        {
            return Conductor.SongPositionInSeconds >= TimeStamp - Conductor.window / 2
                && Conductor.SongPositionInSeconds <= TimeStamp + Conductor.window / 2;
        }
    }

    public void Travel(Transform target, float timeInAdvance)
    {
        var travelSequence = DOTween.Sequence();
        travelSequence.Append(transform.DOMove(target.position + (target.position - transform.position) * 0.25f, timeInAdvance * 1.25f)
            .SetEase(Ease.Linear))
            .AppendCallback(() => Destroy(transform.gameObject));
        travelSequence.Play();
    }

    private void Update()
    {
        if (Conductor.SongPositionInSeconds >= TimeStamp - Conductor.window / 2)
        {

        }
    }

}

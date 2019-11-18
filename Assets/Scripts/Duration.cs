using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duration
{
    public float timeDuration;

    public float TimeLeft { get; private set; }
    public bool IsElapsed { get; private set; }

    public void StartDuration()
    {
        IsElapsed = false;
        TimeLeft = timeDuration;
    }

    public void Update()
    {
        if (IsElapsed)
        {
            Debug.LogWarning("Updating an elapsed Duration");
            return;
        }

        TimeLeft -= Time.deltaTime;

        if (TimeLeft <= 0f)
        {
            IsElapsed = true;
        }
    }
}

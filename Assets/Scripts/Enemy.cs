using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Targetable
{
    public string enemyName;
    public SongData songData;

    public Song Song
    {
        get
        {
            return songData.song;
        }
    }
}

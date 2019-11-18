using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Targetable
{
    public List<SongData> songDatas = new List<SongData>();

    protected override void Start()
    {
        base.Start();

        GameManager.Instance.SetupSong(songDatas.RandomElement().song);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public List<SongData> songDatas = new List<SongData>();

    private void Start()
    {
        GameManager.Instance.SetupSong(songDatas.RandomElement().song);
    }
}

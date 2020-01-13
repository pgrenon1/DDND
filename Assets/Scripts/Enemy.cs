using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Targetable
{
    public string enemyName;
    public SongData songData;

    protected override void Start()
    {
        base.Start();

        Damageable.OnDamageableDeath += Damageable_OnDamageableDeath;
    }

    private void Damageable_OnDamageableDeath(Damageable damageable, Damage damage)
    {
        GameManager.Instance.EndBattle();

        Destroy(gameObject);
    }

    public Song Song
    {
        get
        {
            return songData.song;
        }
    }
}

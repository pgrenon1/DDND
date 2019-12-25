using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyPanel : UIBaseBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI artistText;
    //public TextMeshProUGUI albumText;
    //public TextMeshProUGUI yearText;

    private void Update()
    {
        UpdateVisibility();

        UpdateContent();
    }

    private void UpdateContent()
    {
        var enemy = GameManager.Instance.CurrentEnemy;

        if (!enemy)
            return;

        var song = enemy.Song;

        nameText.SetText(enemy.enemyName);
        titleText.SetText(song.title);
        //albumText.SetText(song.album);
        artistText.SetText(song.artist);
        //yearText.SetText(song.year);
    }

    private void UpdateVisibility()
    {
        if (GameManager.Instance.State is GameStateLoadout
            || GameManager.Instance.State is GameStateBattle)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
}

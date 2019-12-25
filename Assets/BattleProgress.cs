using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleProgress : MonoBehaviour
{
    public Image progressBar;
    public RectTransform scrubber;

    private void Update()
    {
        if (!GameManager.Instance.MusicSource.isPlaying)
            return;

        var song = GameManager.Instance.CurrentSong;

        var songProgressInSeconds = GameManager.Instance.SongPositionInSeconds - song.offset;
        var songProgressRatio = songProgressInSeconds / song.audioClip.length;
        progressBar.fillAmount = songProgressRatio;

        var progressBarHalfWidth = progressBar.rectTransform.rect.width / 2;
        var scrubberX = songProgressRatio.Remap(0f, 1f, -progressBarHalfWidth, progressBarHalfWidth);

        scrubber.localPosition = new Vector2(scrubberX, 0f);
    }
}

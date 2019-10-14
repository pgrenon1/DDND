using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum Button
{
    Left,
    Right,
    Up,
    Down
}

public class Conductor : SingletonMonoBehaviour<Conductor>
{
    [Header("Prefabs")]
    public GameObject leftPrefab;
    public GameObject rightPrefab;
    public GameObject upPrefab;
    public GameObject downPrefab;

    [Header("Settings")]
    public AudioSource musicSource;
    public int beatsInAdvance = 4;
    [Space]
    public RectTransform leftTarget;
    public RectTransform rightTarget;
    public RectTransform upTarget;
    public RectTransform downTarget;
    [Space]
    public Transform leftOrigin;
    public Transform rightOrigin;
    public Transform upOrigin;
    public Transform downOrigin;

    public SongData CurrentSong { get; set; }

    private float _songPositionInSeconds;
    private float _startTime;
    private int _nextIndex;
    private float _timeInAdvance;
    private Difficulty _currentDifficulty;
    private int _currentNoteIndex = 0;
    private int _currentBPMIndex = 0;
    private float _travelTime;

    private void Start()
    {
        CurrentSong = SongLoader.Instance.LoadSongData(SongLoader.Instance.songPath);

        StartSong();
    }

    private void Update()
    {
        if (CurrentSong == null)
            return;

        if (CurrentSong.audioClip == null)
        {
            Debug.LogWarning("AudioClip is null");
            return;
        }

        UpdateSong();
    }

    private void StartSong()
    {
        musicSource.clip = CurrentSong.audioClip;

        _startTime = (float)AudioSettings.dspTime;

        _timeInAdvance = beatsInAdvance * 60f / CurrentSong.bpms[_currentBPMIndex].Value;

        _currentDifficulty = Difficulty.Hard;

        musicSource.Play();
    }

    private void UpdateSong()
    {
        _songPositionInSeconds = (float)(AudioSettings.dspTime - _startTime + CurrentSong.offset);

        if (CurrentSong.bpms.Count > _currentBPMIndex + 1)
        {
            if (_songPositionInSeconds > CurrentSong.bpms[_currentBPMIndex + 1].Key)
            {
                _currentBPMIndex++;
                _timeInAdvance = beatsInAdvance * 60f / CurrentSong.bpms[_currentBPMIndex].Value;
            }
        }

        var nextNote = CurrentSong.notes[_currentDifficulty][_currentNoteIndex];

        if (_songPositionInSeconds + _timeInAdvance >= nextNote.Timestamp)
        {
            //targetTransform.DOScale(1.5f, 0.1f).OnComplete(() => targetTransform.DOScale(1f, 0.1f));

            if (!nextNote.IsEmpty)
                SpawnNote(nextNote);

            //if (nextNote.Left != '0')
            //{
            //    _left.DOScale(1.5f, 0.1f).OnComplete(() => _left.DOScale(1f, 0.1f));
            //}

            //if (nextNote.Right != '0')
            //{
            //    _right.DOScale(1.5f, 0.1f).OnComplete(() => _right.DOScale(1f, 0.1f));
            //}

            //if (nextNote.Up != '0')
            //{
            //    _up.DOScale(1.5f, 0.1f).OnComplete(() => _up.DOScale(1f, 0.1f));
            //}

            //if (nextNote.Down != '0')
            //{
            //    _down.DOScale(1.5f, 0.1f).OnComplete(() => _down.DOScale(1f, 0.1f));
            //}

            _currentNoteIndex++;
        }
    }

    private void SpawnNote(Note note)
    {
        if (note.Left != '0')
        {
            var button = Instantiate(leftPrefab, leftOrigin.position, Quaternion.identity, transform);
            Travel(button, leftTarget);
        }

        if (note.Right != '0')
        {
            var button = Instantiate(rightPrefab, rightOrigin.position, Quaternion.identity, transform);
            Travel(button, rightTarget);
        }

        if (note.Up != '0')
        {
            var button = Instantiate(upPrefab, upOrigin.position, Quaternion.identity, transform);
            Travel(button, upTarget);
        }

        if (note.Down != '0')
        {
            var button = Instantiate(downPrefab, downOrigin.position, Quaternion.identity, transform);
            Travel(button, downTarget);
        }
    }

    private void Travel(GameObject button, Transform target)
    {
        button.transform.DOMoveY(target.position.y, _timeInAdvance).SetEase(Ease.Linear).OnComplete(() => PulseAndDie(button.transform, 0.25f, 0.2f));
    }

    public void Pulse(Transform transform, float endValue, float duration)
    {
        var originalScale = transform.localScale;
        transform.DOScale(endValue, duration / 2).OnComplete(() => transform.DOScale(originalScale, duration / 2));
    }

    public void PulseAndDie(Transform transform, float endValue, float duration)
    {
        var originalScale = transform.localScale;
        transform.DOScale(endValue, duration / 2).OnComplete(() => transform.DOScale(originalScale, duration / 2)).OnComplete(() => Destroy(transform.gameObject));
    }
}

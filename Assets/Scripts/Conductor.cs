using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public enum Button
{
    Left,
    Right,
    Up,
    Down
}

public class Conductor : MonoBehaviour
{
    [Header("Prefabs")]
    public Note leftNote;
    public Note rightNote;
    public Note upNote;
    public Note downNote;

    [Header("Scene")]
    public RectTransform leftOrigin;
    public RectTransform rightOrigin;
    public RectTransform upOrigin;
    public RectTransform downOrigin;
    [Space]
    public RectTransform leftJudgment;
    public RectTransform rightJudgment;
    public RectTransform upJudgment;
    public RectTransform downJudgment;

    [Header("Settings")]
    public AudioSource musicSource;
    public int beatsInAdvance = 4;
    public Difficulty currentDifficulty;
    public float window = 0.5f;

    public SongData CurrentSong { get; private set; }
    public bool IsActive { get; private set; }

    public float SongPositionInSeconds { get; private set; }
    private float _startTime;
    private int _nextIndex;
    private float _timeInAdvance;
    private int _currentBPMIndex = 0;
    private int _nextNoteIndex = 0;
    private NoteData _nextNote;
    private int _currentNoteIndex = 0;
    private NoteData _currentNote;

    //private void Start()
    //{
    //    CurrentSong = SongLoader.Instance.LoadSongData(SongLoader.Instance.songPath);

    //    StartSong();
    //}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CurrentSong = SongLoader.Instance.Songs[0];
            StartSong();
        }

        if (CurrentSong == null || !CurrentSong.IsValid)
            return;

        UpdateSong();
    }

    private void StartSong()
    {
        if (!musicSource.isPlaying)
        {
            musicSource.clip = CurrentSong.audioClip;
            musicSource.Play();
        }

        _startTime = (float)AudioSettings.dspTime;

        _timeInAdvance = beatsInAdvance * 60f / CurrentSong.bpms[_currentBPMIndex].Value;

        _nextNote = CurrentSong.notes[currentDifficulty][_nextNoteIndex];
        _currentNote = CurrentSong.notes[currentDifficulty][_currentNoteIndex];

    }

    private void UpdateSong()
    {
        SongPositionInSeconds = (float)(AudioSettings.dspTime - _startTime + CurrentSong.offset);

        if (CurrentSong.bpms.Count > _currentBPMIndex + 1)
        {
            if (SongPositionInSeconds > CurrentSong.bpms[_currentBPMIndex + 1].Key)
            {
                _currentBPMIndex++;
                _timeInAdvance = beatsInAdvance * 60f / CurrentSong.bpms[_currentBPMIndex].Value;
            }
        }

        //if (_songPositionInSeconds >= _currentNote.Timestamp)
        //{
        //    if (_currentNote.Left != '0')
        //        PulseAndDie(leftJudgment, 1.25f, 0.5f);
        //    if (_currentNote.Right != '0')
        //        PulseAndDie(rightJudgment, 1.25f, 0.5f);
        //    if (_currentNote.Up != '0')
        //        PulseAndDie(upJudgment, 1.25f, 0.5f);
        //    if (_currentNote.Down != '0')
        //        PulseAndDie(downJudgment, 1.25f, 0.5f);

        //    _currentNoteIndex++;
        //    _currentNote = CurrentSong.notes[currentDifficulty][_currentNoteIndex];
        //}

        if (SongPositionInSeconds + _timeInAdvance >= _nextNote.Timestamp)
        {
            if (!_nextNote.IsEmpty)
            {
                SpawnNote(_nextNote);
            }

            _nextNoteIndex++;

            _nextNote = CurrentSong.notes[currentDifficulty][_nextNoteIndex];
        }
    }

    private void SpawnNote(NoteData noteData)
    {
        if (noteData.IsEmpty)
            return;

        if (noteData.Left != '0')
        {
            var note = Instantiate(leftNote, leftOrigin.position, Quaternion.identity, transform);
            note.TimeStamp = noteData.Timestamp;
            note.Conductor = this;
            note.Travel(leftJudgment, _timeInAdvance);
        }

        if (noteData.Right != '0')
        {
            var note = Instantiate(rightNote, rightOrigin.position, Quaternion.identity, transform);
            note.TimeStamp = noteData.Timestamp;
            note.Conductor = this;
            note.Travel(rightJudgment, _timeInAdvance);
        }

        if (noteData.Up != '0')
        {
            var note = Instantiate(upNote, upOrigin.position, Quaternion.identity, transform);
            note.TimeStamp = noteData.Timestamp;
            note.Conductor = this;
            note.Travel(upJudgment, _timeInAdvance);
        }

        if (noteData.Down != '0')
        {
            var note = Instantiate(downNote, downOrigin.position, Quaternion.identity, transform);
            note.TimeStamp = noteData.Timestamp;
            note.Conductor = this;
            note.Travel(downJudgment, _timeInAdvance);
        }
    }

    public void Pulse(Transform transform, float endValue, float duration)
    {
        transform.DOPunchScale(Vector3.one * endValue, duration).From();
    }

    public void PulseAndDie(Transform transform, float endValue, float duration)
    {
        var copy = Instantiate(transform.gameObject, transform.position, Quaternion.identity, transform);
        copy.GetComponent<Image>().DOFade(0, duration).OnComplete(() => Destroy(copy));
        copy.transform.DOScale(Vector3.one * endValue, duration);
    }
}

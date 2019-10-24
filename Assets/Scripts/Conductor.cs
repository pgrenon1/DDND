using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public enum Direction
{
    Left = 0,
    Right = 1,
    Up = 2,
    Down = 3
}

public class Conductor : MonoBehaviour
{
    [Header("Prefabs")]
    public Note leftNote;
    public Note upNote;
    public Note downNote;
    public Note rightNote;

    [Header("Scene")]

    public RectTransform leftOrigin;
    public RectTransform upOrigin;
    public RectTransform downOrigin;
    public RectTransform rightOrigin;
    [Space]
    public RectTransform leftJudgment;
    public RectTransform upJudgment;
    public RectTransform downJudgment;
    public RectTransform rightJudgment;

    [Header("Settings")]
    public int beatsInAdvance = 4;
    public Difficulty currentDifficulty;
    public float okWindow = 0.5f;
    public float goodWindow = 0.3f;
    public float perfectWindow = 0.15f;

    public SongData CurrentSong
    {
        get
        {
            return GameManager.Instance.CurrentSong;
        }
    }
    public PlayerController PlayerController { get; set; }
    public float SongPositionInSeconds { get; private set; }
    public List<Note> ActiveNotes { get; private set; } = new List<Note>();

    private float _startTime;
    private int _nextIndex;
    private float _timeInAdvance;
    private int _currentBPMIndex = 0;
    private int _nextNoteIndex = 0;
    private NoteData _nextNote;
    private bool _songIsPlaying = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerController.IsPlaying = true;
        }

        if (!_songIsPlaying)
            return;

        UpdateSong();
    }

    public void Play()
    {
        _startTime = (float)AudioSettings.dspTime;

        _timeInAdvance = beatsInAdvance * 60f / CurrentSong.bpms[_currentBPMIndex].Value;

        _nextNote = CurrentSong.notes[currentDifficulty][_nextNoteIndex];

        _songIsPlaying = true;
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

        if (SongPositionInSeconds + _timeInAdvance >= _nextNote.Timestamp)
        {
            if (!_nextNote.IsEmpty)
            {
                SpawnNote(_nextNote);
            }

            _nextNoteIndex++;

            _nextNote = CurrentSong.notes[currentDifficulty][_nextNoteIndex];
        }

        foreach (var note in ActiveNotes)
        {
            if (note != null && note.Image != null)
            {
                if (note.Timing == Timing.Perfect)
                {
                    note.Image.color = Color.blue;
                }
                else if (note.Timing == Timing.Good)
                {
                    note.Image.color = Color.green;
                }
                else if (note.Timing == Timing.Ok)
                {
                    note.Image.color = Color.yellow;
                }
                else
                    note.Image.color = Color.red;
            }
        }
    }

    public void JudgeHit(Direction direction)
    {
        Note noteToDelete = null;

        foreach (var note in ActiveNotes)
        {
            if (note.Direction != direction)
                continue;

            if (note.IsPerfect)
            {
                noteToDelete = note;
                break;
            }
            else if (note.IsGood)
            {
                noteToDelete = note;
                break;
            }
            else if (note.IsOpen)
            {
                noteToDelete = note;
                break;
            }
        }

        if (noteToDelete)
        {
            noteToDelete.transform.DOKill();
            ActiveNotes.Remove(noteToDelete);
            Destroy(noteToDelete.gameObject);
        }
    }

    private void SpawnNote(NoteData noteData)
    {
        if (noteData.IsEmpty || PlayerController.IsPlaying)
            return;

        for (int i = 0; i < noteData.Chars.Count; i++)
        {
            if (noteData.Chars[i] != '0' && noteData.Chars[i] != '3')
            {
                var spawnPosition = GetOriginPosition((Direction)i);
                var judgmentPosition = GetJudgmentPosition((Direction)i);
                var notePrefab = GetNotePrefab((Direction)i);

                var note = Instantiate(notePrefab, spawnPosition, Quaternion.identity, transform);
                note.TimeStamp = noteData.Timestamp;
                note.Conductor = this;
                note.Direction = (Direction)i;
                note.Travel(judgmentPosition, _timeInAdvance);
                ActiveNotes.Add(note);
            }
        }
    }

    private Note GetNotePrefab(Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
                return leftNote;
            case Direction.Right:
                return rightNote;
            case Direction.Up:
                return upNote;
            case Direction.Down:
                return downNote;
            default:
                return null;
        }
    }

    private Vector3 GetJudgmentPosition(Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
                return leftJudgment.position;
            case Direction.Right:
                return rightJudgment.position;
            case Direction.Up:
                return upJudgment.position;
            case Direction.Down:
                return downJudgment.position;
            default:
                return Vector3.zero;
        }
    }

    private Vector3 GetOriginPosition(Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
                return leftOrigin.position;
            case Direction.Right:
                return rightOrigin.position;
            case Direction.Up:
                return upOrigin.position;
            case Direction.Down:
                return downOrigin.position;
            default:
                return Vector3.zero;
        }
    }

    //public void Pulse(Transform transform, float endValue, float duration)
    //{
    //    transform.DOPunchScale(Vector3.one * endValue, duration).From();
    //}

    //public void PulseAndDie(Transform transform, float endValue, float duration)
    //{
    //    var copy = Instantiate(transform.gameObject, transform.position, Quaternion.identity, transform);
    //    copy.GetComponent<Image>().DOFade(0, duration).OnComplete(() => Destroy(copy));
    //    copy.transform.DOScale(Vector3.one * endValue, duration);
    //}
}

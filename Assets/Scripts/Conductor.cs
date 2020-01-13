using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Conductor : MonoBehaviour
{
    [Header("Prefabs")]
    public Note notePrefab;
    [Space]
    public float rotationLeft = -90f;
    public float rotationUp = 180f;
    public float rotationDown = 0f;
    public float rotationRight = 90f;

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
    public bool canChangeDifficulty = false;

    public List<Note> ActiveNotes { get; private set; } = new List<Note>();
    public Player Player { get; set; }

    private Difficulty _previousDifficulty;
    public Difficulty _currentDifficulty;
    public Difficulty CurrentDifficulty
    {
        get
        {
            return _currentDifficulty;
        }
        set
        {
            if (value != _currentDifficulty)
            {
                _previousDifficulty = _currentDifficulty;
                _currentDifficulty = value;

                ChangeDifficulty();
            }
        }
    }

    public float SongPositionInSeconds
    {
        get
        {
            return GameManager.Instance.SongPositionInSeconds;
        }
    }

    private float _currentComboScore;
    private int _currentComboCount;
    private float _score;
    private int _nextNoteIndex = 0;
    private NoteData _nextNote;

    private void UpdateCheats()
    {
        if (!canChangeDifficulty)
            return;

        var currentSong = GameManager.Instance.CurrentSong;

        if (Input.GetKeyDown(KeyCode.Alpha1) && currentSong.HasDifficulty(Difficulty.Beginner))
            CurrentDifficulty = Difficulty.Beginner;
        if (Input.GetKeyDown(KeyCode.Alpha2) && currentSong.HasDifficulty(Difficulty.Easy))
            CurrentDifficulty = Difficulty.Easy;
        if (Input.GetKeyDown(KeyCode.Alpha3) && currentSong.HasDifficulty(Difficulty.Medium))
            CurrentDifficulty = Difficulty.Medium;
        if (Input.GetKeyDown(KeyCode.Alpha4) && currentSong.HasDifficulty(Difficulty.Hard))
            CurrentDifficulty = Difficulty.Hard;
        if (Input.GetKeyDown(KeyCode.Alpha5) && currentSong.HasDifficulty(Difficulty.Challenge))
            CurrentDifficulty = Difficulty.Hard;
    }

    public void ResetConductor()
    {
        _nextNoteIndex = 0;
        _currentComboScore = 0;
        _score = 0;
        _nextNote = null;

        foreach (var note in ActiveNotes)
        {
            Destroy(note.gameObject);
        }

        ActiveNotes.Clear();
    }

    private void ChangeDifficulty()
    {
        var currentSong = GameManager.Instance.CurrentSong;

        foreach (var noteData in currentSong.notes[CurrentDifficulty])
        {
            if (noteData.timestamp > SongPositionInSeconds + GameManager.Instance.TimeInAdvance)
            {
                _nextNoteIndex = currentSong.notes[CurrentDifficulty].IndexOf(noteData);
                _nextNote = noteData;
                break;
            }
        }
    }

    public void UpdateNotes()
    {
        _nextNote = GameManager.Instance.CurrentSong.notes[CurrentDifficulty][_nextNoteIndex];

        if (SongPositionInSeconds + GameManager.Instance.TimeInAdvance >= _nextNote.timestamp)
        {
            if (!_nextNote.IsEmpty)
            {
                SpawnNote(_nextNote);
            }

            _nextNoteIndex++;

            var noteData = GameManager.Instance.CurrentSong.GetNote(CurrentDifficulty, _nextNoteIndex);

            _nextNote = noteData;
        }
    }

    private void UpdateNoteColors()
    {
        foreach (var note in ActiveNotes)
        {
            if (note != null && note.Image != null)
            {
                if (note.GetTiming() == Timing.Perfect)
                {
                    note.Image.color = Color.blue;
                }
                else if (note.GetTiming() == Timing.Good)
                {
                    note.Image.color = Color.green;
                }
                else if (note.GetTiming() == Timing.Ok)
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

            var noteScore = note.Score(_currentComboCount);

            if (noteScore > 0f)
            {
                note.ApplyEffects(noteScore);
                _currentComboCount++;
                noteToDelete = note;
                break;
            }
            else
            {
                //var comboMultiplier = _currentComboCount * comboValue;
                //_score += _currentComboScore * comboMultiplier;
                _currentComboCount = 0;
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
        if (noteData == null || noteData.IsEmpty)
            return;

        for (int i = 0; i < noteData.Chars.Count; i++)
        {
            if (noteData.Chars[i] != '0' && noteData.Chars[i] != '3')
            {
                var direction = (Direction)i;
                var spawnPosition = GetOriginPosition(direction);
                var judgmentPosition = GetJudgmentPosition(direction);
                var noteRotation = GetNoteRotation(direction);

                var note = Instantiate(notePrefab, spawnPosition, Quaternion.identity, transform);
                note.transform.Rotate(0f, 0f, noteRotation);
                note.TimeStamp = noteData.timestamp;
                note.Conductor = this;
                note.Direction = direction;
                note.Travel(judgmentPosition, GameManager.Instance.TimeInAdvance);
                ActiveNotes.Add(note);

                if (Player.ActiveSkill != null && Player.ActiveSkill.ActiveNotes.Count < Player.ActiveSkill.noteCount)
                {
                    note.Skill = Player.ActiveSkill;
                    Player.ActiveSkill.ActiveNotes.Add(note);
                }
            }
        }
    }

    private float GetNoteRotation(Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
                return rotationLeft;
            case Direction.Right:
                return rotationRight;
            case Direction.Up:
                return rotationUp;
            default:
                return rotationDown;
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
}
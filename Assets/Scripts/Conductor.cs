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
    public bool canChangeDifficulty = false;

    public List<Note> ActiveNotes { get; private set; } = new List<Note>();

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

    public Song CurrentSong
    {
        get
        {
            return GameManager.Instance.CurrentSong;
        }
    }

    public Player Player { get; set; }
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

    //private void Update()
    //{
    //    UpdateCheats();

    //    UpdateNoteSpawning();

    //    UpdateNoteColors();
    //}

    private void UpdateCheats()
    {
        if (!canChangeDifficulty)
            return;

        if (Input.GetKeyDown(KeyCode.Alpha1) && CurrentSong.HasDifficulty(Difficulty.Beginner))
            CurrentDifficulty = Difficulty.Beginner;
        if (Input.GetKeyDown(KeyCode.Alpha2) && CurrentSong.HasDifficulty(Difficulty.Easy))
            CurrentDifficulty = Difficulty.Easy;
        if (Input.GetKeyDown(KeyCode.Alpha3) && CurrentSong.HasDifficulty(Difficulty.Medium))
            CurrentDifficulty = Difficulty.Medium;
        if (Input.GetKeyDown(KeyCode.Alpha4) && CurrentSong.HasDifficulty(Difficulty.Hard))
            CurrentDifficulty = Difficulty.Hard;
        if (Input.GetKeyDown(KeyCode.Alpha5) && CurrentSong.HasDifficulty(Difficulty.Challenge))
            CurrentDifficulty = Difficulty.Hard;
    }

    private void ChangeDifficulty()
    {
        foreach (var noteData in CurrentSong.notes[CurrentDifficulty])
        {
            if (noteData.timestamp > SongPositionInSeconds + GameManager.Instance.TimeInAdvance)
            {
                _nextNoteIndex = CurrentSong.notes[CurrentDifficulty].IndexOf(noteData);
                _nextNote = noteData;
                break;
            }
        }
    }

    public void UpdateNotes()
    {
        _nextNote = CurrentSong.notes[CurrentDifficulty][_nextNoteIndex];

        if (SongPositionInSeconds + GameManager.Instance.TimeInAdvance >= _nextNote.timestamp)
        {
            if (!_nextNote.IsEmpty)
            {
                SpawnNote(_nextNote);
            }

            _nextNoteIndex++;

            var noteData = CurrentSong.GetNote(CurrentDifficulty, _nextNoteIndex);

            _nextNote = noteData;
        }
    }

    private void UpdateNoteColors()
    {
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

            if (TryScoreNote(note))
            {
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

    private bool TryScoreNote(Note note)
    {
        bool noteScored = false;

        var noteScore = GameManager.Instance.GetNoteValue(note.Timing) * (1 + _currentComboCount * GameManager.Instance.comboValue);

        if (noteScore > 0f)
        {
            noteScored = true;

            ApplyNoteEffects(noteScore);
        }

        return noteScored;
    }

    private void ApplyNoteEffects(float noteScore)
    {
        var loadoutSlots = Player.LoadoutPanel.LoadoutSlots;
        foreach (var kvp in loadoutSlots)
        {
            kvp.Key.GetPickedSlotElement<LoadoutSlotElement>();
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
                var spawnPosition = GetOriginPosition((Direction)i);
                var judgmentPosition = GetJudgmentPosition((Direction)i);
                var notePrefab = GetNotePrefab((Direction)i);

                var note = Instantiate(notePrefab, spawnPosition, Quaternion.identity, transform);
                note.TimeStamp = noteData.timestamp;
                note.Conductor = this;
                note.Direction = (Direction)i;
                note.Travel(judgmentPosition, GameManager.Instance.TimeInAdvance);
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
}
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
    public int beatsInAdvance = 4;
    public float okWindow = 0.5f;
    public float goodWindow = 0.3f;
    public float perfectWindow = 0.15f;
    public float comboValue = 0.1f;
    public float noteValueOk = 1f;
    public float noteValueGood = 1f;
    public float noteValuePerfect = 1f;
    public bool canChangeDifficulty = false;

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
    public float SongPositionInSeconds { get; private set; }
    public List<Note> ActiveNotes { get; private set; } = new List<Note>();
    public LoadoutPanel PlayerMenu { get; private set; }

    private float _currentComboScore;
    private int _currentComboCount;
    private float _score;
    private float _startTime;
    private float _timeInAdvance;
    private int _currentBPMIndex = 0;
    private int _nextNoteIndex = 0;
    private NoteData _nextNote;
    private bool _songIsPlaying = false;

    private void Init()
    {
        PlayerMenu = Player.PlayerMenu;
    }

    private void Update()
    {
        UpdateCheats();

        if (!_songIsPlaying)
            return;

        UpdateSong();

        UpdateNoteColors();
    }

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
        var tentativeTimestamp = SongPositionInSeconds + _timeInAdvance;

        foreach (var noteData in CurrentSong.notes[CurrentDifficulty])
        {
            if (noteData.timestamp > SongPositionInSeconds + _timeInAdvance)
            {
                _nextNoteIndex = CurrentSong.notes[CurrentDifficulty].IndexOf(noteData);
                _nextNote = noteData;
                break;
            }
        }
    }

    public void Play()
    {
        _startTime = (float)AudioSettings.dspTime;

        _timeInAdvance = beatsInAdvance * 60f / CurrentSong.bpms[_currentBPMIndex].Value;

        _nextNote = CurrentSong.notes[CurrentDifficulty][_nextNoteIndex];

        _songIsPlaying = true;
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

        if (SongPositionInSeconds + _timeInAdvance >= _nextNote.timestamp)
        {
            if (!_nextNote.IsEmpty)
            {
                SpawnNote(_nextNote);
            }

            _nextNoteIndex++;

            _nextNote = CurrentSong.notes[CurrentDifficulty][_nextNoteIndex];
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

        var noteScore = GetNoteValue(note.Timing) * (1 + _currentComboCount * comboValue);

        if (noteScore > 0f)
        {
            noteScored = true;

            ApplyNoteEffects(noteScore);
        }

        return noteScored;
    }

    private void ApplyNoteEffects(float noteScore)
    {
        var loadoutSlots = PlayerMenu.LoadoutSlots;
        foreach (var kvp in loadoutSlots)
        {
            kvp.Key.GetPickedSlotElement<LoadoutSlotElement>();
        }
    }

    private float GetNoteValue(Timing timing)
    {
        switch (timing)
        {
            case Timing.Perfect:
                return noteValuePerfect;
            case Timing.Good:
                return noteValueGood;
            case Timing.Ok:
                return noteValueOk;
            default:
                return 0f;
        }
    }

    private void SpawnNote(NoteData noteData)
    {
        if (noteData.IsEmpty/* || !Player.IsDancing*/)
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
}
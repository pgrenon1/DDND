using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Song
{
    public string title;
    public string artist;
    public float offset;
    [System.NonSerialized, OdinSerialize]
    public List<KeyValuePair<float, float>> bpms;
    public float displayBpm;
    public string musicPath;
    public AudioClip audioClip;
    public float duration;
    public float lastTimestamp;
    [OdinSerialize]
    public Dictionary<Difficulty, List<NoteData>> notes;

    public bool HasDifficulty(Difficulty difficulty)
    {
        return notes.ContainsKey(difficulty);
    }

    public bool IsValid
    {
        get
        {
            return !string.IsNullOrEmpty(title)
                && !string.IsNullOrEmpty(artist)
                && bpms.Count > 0f
                && displayBpm != 0f
                && audioClip != null
                && notes.Count > 0;
        }
    }

    public Song(string title, string artist, float offset, List<KeyValuePair<float, float>> bpms, string musicPath, float displayBpm, Dictionary<Difficulty, List<NoteData>> notes)
    {
        this.title = title;
        this.artist = artist;
        this.offset = offset;
        this.bpms = bpms;
        this.musicPath = musicPath;
        this.audioClip = (AudioClip)Resources.Load(musicPath);
        this.displayBpm = displayBpm;
        this.notes = notes;

        GetLastTimeStamp(notes);
    }

    private void GetLastTimeStamp(Dictionary<Difficulty, List<NoteData>> notes)
    {
        foreach (var difficulty in notes)
        {
            var timestamp = difficulty.Value[difficulty.Value.Count - 1].timestamp;
            if (timestamp > lastTimestamp)
            {
                lastTimestamp = timestamp;
            }
        }
    }

    public NoteData GetNote(Difficulty currentDifficulty, int noteIndex)
    {
        var noteList = notes[currentDifficulty];
        if (noteList != null && noteIndex < noteList.Count - 1)
        {
            var noteData = noteList[noteIndex];
            if (noteData != null)
                return noteData;
        }

        return null;
    }
}

public enum Difficulty
{
    Beginner,
    Easy,
    Medium,
    Hard,
    Challenge
}

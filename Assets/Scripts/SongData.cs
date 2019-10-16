using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SongData
{
    public string title;
    public string artist;
    public float offset;
    public List<KeyValuePair<float, float>> bpms;
    public float displayBpm;
    public string musicPath;
    public AudioClip audioClip;
    public Dictionary<Difficulty, List<NoteData>> notes;

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

    public SongData(string title, string artist, float offset, List<KeyValuePair<float, float>> bpms, string musicPath, float displayBpm, Dictionary<Difficulty, List<NoteData>> notes)
    {
        this.title = title;
        this.artist = artist;
        this.offset = offset;
        this.bpms = bpms;
        this.musicPath = musicPath;
        this.audioClip = (AudioClip)Resources.Load(musicPath);
        this.displayBpm = displayBpm;
        this.notes = notes;
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

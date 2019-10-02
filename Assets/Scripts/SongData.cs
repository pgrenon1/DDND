using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SongData
{
    public bool isValid;
    public string title;
    public string artist;
    public float offset;
    public Dictionary<float, float> bpms;
    public AudioClip music;
    public Dictionary<Difficulty, Bar[]> bars;
}

public enum Difficulty
{
    Beginner,
    Easy,
    Medium,
    Hard,
    Challenge
}

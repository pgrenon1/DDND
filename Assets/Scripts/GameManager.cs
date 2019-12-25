using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ShowOdinSerializedPropertiesInInspector]
public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public AudioSource musicSourcePrefab;
    [AssetList(Path = "Resources/PlayerClasses")]
    public List<PlayerClass> playerClasses = new List<PlayerClass>();
    public List<Enemy> enemyPrefabs = new List<Enemy>();

    [Header("Settings")]
    public int beatsInAdvance = 4;
    public float okWindow = 0.5f;
    public float goodWindow = 0.3f;
    public float perfectWindow = 0.15f;
    public float comboValue = 0.1f;
    public float noteValueOk = 1f;
    public float noteValueGood = 1.5f;
    public float noteValuePerfect = 2f;

    public GameStateBase State { get; set; }
    public float SongPositionInSeconds { get; private set; }
    public Enemy CurrentEnemy { get; private set; }
    public AudioSource MusicSource { get; private set; }
    public float StartTime { get; private set; }

    private float _audioTimescale = 1f;
    public float AudioTimescale
    {
        set
        {
            _audioTimescale = value;

            if (MusicSource != null)
                MusicSource.pitch = _audioTimescale;

            Time.timeScale = _audioTimescale;
        }
    }

    public float TimeInAdvance
    {
        get
        {
            return beatsInAdvance * 60f / CurrentSong.bpms[_currentBPMIndex].Value;
        }
    }

    public List<Player> Players
    {
        get
        {
            return PlayerManager.Instance.Players;
        }
    }

    [System.NonSerialized, OdinSerialize, ReadOnly]
    public Song currentSong;
    public Song CurrentSong
    {
        get
        {
            return currentSong;
        }
        set
        {
            currentSong = value;
            MusicSource.clip = currentSong.audioClip;
            MusicSource.time = 0f;
        }
    }

    private SongLoader _songLoader;
    private int _currentBPMIndex = 0;
    private double _lastDspTime;

    protected override void Awake()
    {
        base.Awake();

        _songLoader = new SongLoader();

        MusicSource = Instantiate(musicSourcePrefab, transform);

        //SpawnEnemy(enemyPrefabs[0]);
    }

    private void Start()
    {
        State = GameStateBase.gameStateBase;
        State.ToState(this, GameStateBase.gameStateRegistration);
    }

    public void SpawnEnemy()
    {
        CurrentEnemy = Instantiate(enemyPrefabs.RandomElement()); ;
        CurrentSong = CurrentEnemy.Song;
    }

    public void PlaySong()
    {
        StartTime = (float)AudioSettings.dspTime;
        _lastDspTime = StartTime;
        SongPositionInSeconds = 0 + currentSong.offset;

        MusicSource.Play();
    }

    public void UpdateSong()
    {
        var toAdd = AudioSettings.dspTime - _lastDspTime;
        SongPositionInSeconds += (float)(toAdd * _audioTimescale);
        //SongPositionInSeconds = (float)(AudioSettings.dspTime - StartTime + CurrentSong.offset);
        _lastDspTime = AudioSettings.dspTime;

        if (CurrentSong.bpms.Count > _currentBPMIndex + 1)
        {
            if (SongPositionInSeconds > CurrentSong.bpms[_currentBPMIndex + 1].Key)
            {
                _currentBPMIndex++;
            }
        }
    }

    private void Update()
    {
        State.Update(this);

        if (Input.GetKeyDown(KeyCode.F1))
            AudioTimescale = 1f;
        else if (Input.GetKeyDown(KeyCode.F2))
            AudioTimescale = 3f;
    }

    public float GetNoteValue(Timing timing)
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

    public void DestroyEnemy()
    {
        Destroy(CurrentEnemy);
    }
}

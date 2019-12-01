using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    MainMenu,
    Registeration,
    Map,
    Encounter
}

public enum EncounterState
{
    Loadout,
    Combat,
    Review
}

[ShowOdinSerializedPropertiesInInspector]
public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public AudioSource musicSourcePrefab;
    public List<Player> players = new List<Player>();
    [AssetList(Path = "Resources/PlayerClasses")]
    public List<PlayerClass> playerClasses = new List<PlayerClass>();
    public List<Enemy> enemyPrefabs = new List<Enemy>();
    public Transform enemyParent;
    public Transform playerStage;

    public Player ActivePlayer { get { return players[_activePlayerIndex]; } }
    public bool SongIsPlaying { get; set; }
    public Enemy CurrentEnemy { get; set; }

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
            _musicSource.clip = currentSong.audioClip;
            _musicSource.time = 0f;
        }
    }

    private int _activePlayerIndex = 0;
    private AudioSource _musicSource;
    private SongLoader _songLoader;
    private Enemy _currentEnemy;

    protected override void Awake()
    {
        base.Awake();

        _songLoader = new SongLoader();

        _musicSource = Instantiate(musicSourcePrefab, transform);

        SpawnEnemy(enemyPrefabs[0]);
    }

    private void Start()
    {
        foreach (var player in players)
        {
            player.Init();
            player.PickLoadout();
        }
    }

    private void SpawnEnemy(Enemy enemyPrefab)
    {
        var enemy = Instantiate(enemyPrefab, enemyParent.position, Quaternion.identity, enemyParent);
        enemy.transform.LookAt(playerStage);

        _currentEnemy = enemy;
    }

    public void StartSong()
    {
        _musicSource.Play();

        foreach (var player in players)
        {
            player.Conductor.Play();
            player.StartDancing();
        }

        SongIsPlaying = true;
    }

    private void Update()
    {
        if (!SongIsPlaying && players.TrueForAll(player => player.IsReady))
        {
            StartSong();
        }
    }

    public void SetupSong(Song songToPlay)
    {
        CurrentSong = songToPlay;
    }
}

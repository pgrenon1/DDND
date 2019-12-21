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
    public Transform enemyParent;
    public Transform playerStage;

    public bool SongIsPlaying { get; set; }
    public Enemy CurrentEnemy { get; set; }
    public List<Player> Players { get; set; } = new List<Player>();
    public GameStateBase State;

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

    private AudioSource _musicSource;
    private SongLoader _songLoader;
    private Enemy _currentEnemy;

    protected override void Awake()
    {
        base.Awake();

        _songLoader = new SongLoader();

        _musicSource = Instantiate(musicSourcePrefab, transform);

        //SpawnEnemy(enemyPrefabs[0]);
    }

    private void Start()
    {
        State = GameStateBase.gameStateBase;
        State.ToState(this, GameStateBase.gameStateRegistration);
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

        foreach (var player in Players)
        {
            player.Conductor.Play();
            player.StartDancing();
        }

        SongIsPlaying = true;
    }

    private void Update()
    {
        State.Update(this);
    }

    public void SetupSong(Song songToPlay)
    {
        CurrentSong = songToPlay;
    }
}

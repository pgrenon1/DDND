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
    public List<Player> players = new List<Player>();
    public List<Enemy> enemyPrefabs = new List<Enemy>();
    public Transform enemyParent;
    public Transform playerStage;

    public Player ActivePlayer { get { return players[_activePlayerIndex]; } }
    public bool SongIsPlaying { get; set; }
    public Enemy CurrentEnemy { get; set; }

    [System.NonSerialized, OdinSerialize]
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

    //private void StartPlaying()
    //{
    //    _activePlayerIndex = _firstIndex;
    //    FirstPlayer = players[_firstIndex];
    //}

    //public void ChooseSong(Song songData)
    //{
    //    CurrentSong = songData;
    //}

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

    //public void NextTurn()
    //{
    //    _activePlayerIndex++;

    //    if (_activePlayerIndex > playerControllers.Count - 1)
    //    {
    //        _firstIndex++;

    //        if (_firstIndex >= playerControllers.Count - 1)
    //        {
    //            _firstIndex = 0;
    //        }

    //        _activePlayerIndex = _firstIndex;

    //        FirstPlayer = playerControllers[_firstIndex];
    //    }

    //    playerControllers[_activePlayerIndex].StartTurn();
    //}

    //public void StartActionMenus()
    //{
    //    Inventory.Instance.Hide();

    //    foreach (var playerController in players)
    //    {
    //        playerController.PlayerMenu.InitActionMenu();
    //    }
    //}

    private void Update()
    {
        if (players.TrueForAll(player => player.IsReady))
        {
            StartSong();
        }
    }

    public void SetupSong(Song songToPlay)
    {
        _musicSource.clip = songToPlay.audioClip;
    }
}

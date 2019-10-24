using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{

    public AudioSource musicSourcePrefab;
    public List<PlayerController> playerControllers = new List<PlayerController>();
    [Title("Settings")]
    public float delay = 10f;

    public int ActiveIndex { get; private set; }
    public PlayerController ActivePlayer { get { return playerControllers[ActiveIndex]; } }
    public bool SongIsPlaying { get; set; }

    private SongData _currentSong;
    public SongData CurrentSong
    {
        get
        {
            return _currentSong;
        }
        set
        {
            _currentSong = value;
            _musicSource.clip = _currentSong.audioClip;
            _musicSource.time = 0f;
        }
    }

    private int previousFirstIndex = 0;
    private AudioSource _musicSource;
    private SongLoader _songLoader;

    private void Start()
    {
        _musicSource = Instantiate(musicSourcePrefab, transform);

        _songLoader = GetComponent<SongLoader>();
        _songLoader.LoadSongs();

        // TEMP
        CurrentSong = SongLoader.Instance.Songs[0];

        previousFirstIndex = ActiveIndex;
    }

    public void StartSong()
    {
        _musicSource.Play();

        foreach (var playerController in playerControllers)
        {
            playerController.Conductor.Play();
        }

        SongIsPlaying = true;
    }

    public void NextTurn()
    {
        ActiveIndex++;

        if (ActiveIndex > playerControllers.Count - 1)
        {
            var newFirstIndex = previousFirstIndex + 1;

            if (newFirstIndex >= playerControllers.Count - 1)
            {
                newFirstIndex = 0;
            }

            ActiveIndex = newFirstIndex;
            previousFirstIndex = newFirstIndex;
        }
    }

    public IEnumerator DelayAfterLastTurn()
    {
        yield return new WaitForSeconds(delay);

        NextTurn();
    }
}

using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Conductor : SingletonMonoBehaviour<Conductor>
{
    public SongData CurrentSong { get; set; }

    public bool IsReady { get; set; } = false;

    //The number of seconds for each song beat
    public float secPerBeat { get; set; }

    //Current song position, in seconds
    public float songPositionInSeconds { get; set; }

    //Current song position, in beats
    public float songPositionInBeats { get; set; }

    //How many seconds have passed since the song started
    public float startTime { get; set; }

    public string songPath = "";

    //an AudioSource attached to this GameObject that will play the music.
    public AudioSource musicSource;

    private void Start()
    {
        LoadSongData(songPath);
    }

    private void Update()
    {
        if (CurrentSong != null || !IsReady)
            return;

        UpdateSong();
    }

    public void LoadSongData(string path)
    {
        var lines = File.ReadAllLines(path);

        //Get the file directory, and make sure it ends with either forward or backslash
        var fileDir = Path.GetDirectoryName(path);
        if (!fileDir.EndsWith("\\") && !fileDir.EndsWith("/"))
        {
            fileDir += "\\";
        }

        bool inNotes = false;

        var title = "";
        var musicPath = "";
        var artist = "";
        bool isValid = false;
        float offset;
        float displayBpm;
        var bpms = new List<KeyValuePair<float, float>>();
        var bars = new Dictionary<Difficulty, Bar[]>();

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();

            if (line.StartsWith("//"))
                continue;

            if (line.StartsWith("#"))
            {
                string key = line.Substring(0, line.IndexOf(':')).Trim('#').Trim(':');

                switch (key.ToUpper())
                {
                    case "TITLE":
                        title = line.Substring(line.IndexOf(':')).Trim(':').Trim(';');
                        break;
                    //case "SUBTITLE":
                    //    songData.subtitle = line.Substring(line.IndexOf(':')).Trim(':').Trim(';');
                    //    break;
                    case "ARTIST":
                        artist = line.Substring(line.IndexOf(':')).Trim(':').Trim(';');
                        break;
                    //case "BANNER":
                    //    songData.bannerPath = fileDir + line.Substring(line.IndexOf(':')).Trim(':').Trim(';');
                    //    break;
                    //case "BACKGROUND":
                    //    songData.backgroundPath = fileDir + line.Substring(line.IndexOf(':')).Trim(':').Trim(';');
                    //    break;
                    case "MUSIC":
                        musicPath = path + line.Substring(line.IndexOf(':')).Trim(':').Trim(';');
                        if (!File.Exists(musicPath))
                        {
                            musicPath = null;
                            isValid = false;
                        }
                        break;
                    case "OFFSET":
                        if (!float.TryParse(line.Substring(line.IndexOf(':')).Trim(':').Trim(';'), out offset))
                        {
                            offset = 0.0f;
                        }
                        break;
                    //case "SAMPLESTART":
                    //    if (!float.TryParse(line.Substring(line.IndexOf(':')).Trim(':').Trim(';'), out songData.sampleStart))
                    //    {
                    //        //Error Parsing
                    //        songData.sampleStart = 0.0f;
                    //    }
                    //    break;
                    //case "SAMPLELENGTH":
                    //    if (!float.TryParse(line.Substring(line.IndexOf(':')).Trim(':').Trim(';'), out songData.sampleLength))
                    //    {
                    //        //Error Parsing
                    //        songData.sampleLength = sampleLengthDefault;
                    //    }
                    //    break;
                    case "DISPLAYBPM":
                        if (!float.TryParse(line.Substring(line.IndexOf(':')).Trim(':').Trim(';'), out displayBpm) || displayBpm <= 0)
                        {
                            isValid = false;
                            displayBpm = 0.0f;
                        }
                        break;
                    case "BPMS":
                        var beatsPerMinutes = line.Substring(line.IndexOf(':')).Trim(':').Trim(';').Split(',');
                        for (int j = 0; j < beatsPerMinutes.Length; j++)
                        {
                            var newBpm = beatsPerMinutes[j].Split('=');
                            float bpmKey;
                            float bpmValue;
                            if (!float.TryParse(newBpm[0], out bpmKey) || !float.TryParse(newBpm[1], out bpmValue))
                            {
                                isValid = false;
                            }
                            else
                            {
                                bpms.Add(new KeyValuePair<float, float>(bpmKey, bpmValue));
                            }
                        }
                        break;
                    case "NOTES":
                        inNotes = true;

                        // Not handled
                        if (!lines[i + 1].ToLower().Contains("dance-single"))
                        {
                            // Update the for loop we're in to adequately skip this section
                            for (int j = i; j < lines.Length; j++)
                            {
                                if (lines[j].Contains(";"))
                                {
                                    i = j - 1;
                                    break;
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            }


            if (inNotes)
            {
                if (line.ToLower().Contains("beginner") ||
                line.ToLower().Contains("easy") ||
                line.ToLower().Contains("medium") ||
                line.ToLower().Contains("hard") ||
                line.ToLower().Contains("challenge"))
                {
                    var difficultyString = line.Trim().Trim(':');

                    var noteLines = new List<string>();

                    for (int j = i; j < lines.Length; j++)
                    {
                        string noteLine = lines[j].Trim();
                        if (noteLine.EndsWith(";"))
                        {
                            i = j - 1;
                            break;
                        }
                        else if (!noteLine.Contains(":"))
                        {
                            noteLines.Add(noteLine);
                        }
                    }

                    RecordBars(difficultyString, noteLines, ref bars);
                }

                if (line.EndsWith(";"))
                {
                    inNotes = false;
                }
            }
        }
    }

    private void RecordBars(string difficultyString, List<string> noteLines, ref Dictionary<Difficulty, Bar[]> barsData)
    {
        Difficulty difficulty;
        if (System.Enum.TryParse(difficultyString, ignoreCase: true, out difficulty))
        {
            var bars = new Bar[noteLines.Count];
            for (int i = 0; i < bars.Length; i++)
            {
                bars[i] = ParseNoteLine(noteLines[i]);
            }

            barsData.Add(difficulty, bars);
        }
    }

    private Bar ParseNoteLine(string noteLine)
    {

    }

    private void StartSong()
    {
        //Calculate the number of seconds in each beat
        secPerBeat = 60f / CurrentSong.bpms[0];

        //Record the time when the music starts
        startTime = (float)AudioSettings.dspTime;

        //Start the music
        musicSource.Play();
    }

    private void UpdateSong()
    {
        //determine how many seconds since the song started
        var songPositionInSeconds = (float)(AudioSettings.dspTime - startTime - CurrentSong.offset);

        //determine how many beats since the song started
        songPositionInBeats = songPositionInSeconds / secPerBeat;
    }
}

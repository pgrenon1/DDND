using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SongLoader
{
    //public List<SongData> Songs { get; private set; } = new List<SongData>();

    public Song LoadSong(string smPath)
    {
        return LoadSongData(smPath);
    }

    private Song LoadSongData(string path)
    {
        var lines = File.ReadAllLines(path);

        //Get the file's directory, and make sure it ends with either forward or backslash
        var fileDir = Path.GetDirectoryName(path);
        if (!fileDir.EndsWith("\\") && !fileDir.EndsWith("/"))
        {
            fileDir += "\\";
        }

        bool inNotes = false;

        var title = "";
        var musicPath = "";
        var artist = "";
        bool isValid = true;
        float offset = 0f;
        float displayBpm = 1f;
        var bpms = new List<KeyValuePair<float, float>>();
        var notes = new Dictionary<Difficulty, List<NoteData>>();

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
                        var extensionIndex = line.LastIndexOf('.');
                        var songsDir = fileDir.TrimStart("Assets\\Resources\\".ToCharArray());
                        musicPath = songsDir + line.Substring(line.IndexOf(':') + 1).Trim(line.Substring(extensionIndex).ToCharArray());
                        var music = Resources.Load<AudioClip>(musicPath);
                        if (!music)
                        {
                            Debug.LogWarning("Could not load file from Resources at path: " + musicPath);
                            isValid = false;
                        }
                        break;
                    case "OFFSET":
                        if (!float.TryParse(line.Substring(line.IndexOf(':')).Trim(':').Trim(';'), out offset))
                        {
                            offset = 0f;
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
                            Debug.LogWarning("Could not find a Display BPM");
                            isValid = false;
                            displayBpm = 0f;
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
                                Debug.LogWarning("Could not parse bpms");
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

                    RecordNotes(difficultyString, noteLines, bpms, ref notes);
                }

                if (line.EndsWith(";"))
                {
                    inNotes = false;
                }
            }
        }

        if (isValid)
        {
            var musicPathRelative = musicPath;
            var songData = new Song(title, artist, offset, bpms, musicPathRelative, displayBpm, notes);
            return songData;
        }
        else
        {
            return null;
        }
    }

    private void RecordNotes(string difficultyString, List<string> noteLines, List<KeyValuePair<float, float>> bpms, ref Dictionary<Difficulty, List<NoteData>> notes)
    {
        int barCount = 0;
        Difficulty difficulty;
        if (System.Enum.TryParse(difficultyString, ignoreCase: true, out difficulty))
        {
            //Debug.Log(difficultyString.ToUpper());
            var masterList = new List<NoteData>();
            var barNotes = new List<NoteData>();
            int bpmIndex = 0;

            for (int i = 0; i < noteLines.Count; i++)
            {
                // end of notes for this difficulty
                if (noteLines[i].StartsWith(";"))
                {
                    break;
                }

                // end of bar
                if (noteLines[i].StartsWith(","))
                {
                    // record time stamps
                    //Debug.Log(barNotes.Count);
                    var numberOfNotesInBar = barNotes.Count;
                    for (int j = 0; j < barNotes.Count; j++)
                    {
                        var currentBPM = bpms[bpmIndex].Value;
                        var secPerBeat = 60f / currentBPM;
                        var secPerBar = secPerBeat * 4f;
                        var timeStampInBar = j * secPerBar / numberOfNotesInBar;
                        var timeStamp = timeStampInBar + barCount * secPerBar;

                        barNotes[j].timestamp = timeStamp;

                        if (bpmIndex < bpms.Count - 1)
                        {
                            if (timeStamp > bpms[bpmIndex + 1].Key)
                            {
                                bpmIndex++;
                            }
                        }

                        //Debug.Log(barNotes[j].ToString());
                    }

                    masterList.AddRange(barNotes);

                    barCount++;
                    barNotes = new List<NoteData>();
                    continue;
                }

                barNotes.Add(new NoteData(noteLines[i]));
                //noteList.Add(new Note(noteLines[i], ));
                //bar.notes.Add(new Note(noteLines[i], ));
            }

            notes.Add(difficulty, masterList);
        }
    }
}

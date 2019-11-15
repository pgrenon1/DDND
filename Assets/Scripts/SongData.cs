using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Song_", menuName = "Song"), ShowOdinSerializedPropertiesInInspector]
public class SongData : OdinSerializedScriptableObject
{
    [InlineButton("LoadSongData")]
    [FilePath(Extensions = "sm")]
    public string smPath;
    [System.NonSerialized, OdinSerialize]
    public Song song;

    private void LoadSongData()
    {
        var songLoader = new SongLoader();
        var songDataTemp = songLoader.LoadSong(smPath);

        if (songDataTemp == null || !songDataTemp.IsValid)
            return;

        Debug.Log("Loaded : " + songDataTemp.title);
        EditorUtility.SetDirty(this);

        song = songDataTemp;
    }
}
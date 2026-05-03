using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Linq;

public static class SaveSystem
{
    //Filename for storing best time, best rank, etc
    public const string FILENAME_SAVEDATA = "/savedata.json";
    public const string FILENAME_SETTINGSDATA = "/settingsdata.json";

    public static void SaveGameState()
    {
        string filePathLevelSaveData = Application.persistentDataPath + FILENAME_SAVEDATA;
        levelData leveldata = new levelData(DataManager.Instance);
        SaveData saveData = new SaveData(leveldata);
        string txt = JsonUtility.ToJson(saveData);
        using(StreamWriter stream = File.CreateText(filePathLevelSaveData))
        {
            stream.WriteLine(txt);
        }
        Debug.Log("Success");
    }

    public static void SaveSettings()
    {
        string filePathSettingsSaveData = Application.persistentDataPath + FILENAME_SETTINGSDATA;
        SettingsData SD = new SettingsData(SettingsManager.Instance);
        SettingsSaveData SSD = new SettingsSaveData(SD);
        string txt = JsonUtility.ToJson(SSD);
        using (StreamWriter stream = File.CreateText(filePathSettingsSaveData))
        {
            stream.WriteLine(txt);
        }
    }
}

[System.Serializable]
public class SaveData
{
    [SerializeField] public levelData leveldata;

    public SaveData(levelData leveldata)
    {
        this.leveldata = leveldata;
    }
}

[System.Serializable]
public class SettingsSaveData
{
    [SerializeField] public SettingsData settingsdata;

    public SettingsSaveData(SettingsData SD)
    {
        this.settingsdata = SD;
    }
}


[System.Serializable]
public class levelData
{
    //Levels
    [SerializeField] public float[] Level1Stats;


    public levelData(DataManager datamanager)
    {
        //Levels
        Level1Stats = datamanager.level1Results;
    }
}

[System.Serializable]
public class SettingsData
{
    [SerializeField] public float[] SoundSettings;
    [SerializeField] public bool FullScreen;
    [SerializeField] public float Brightness;
    [SerializeField] public float Sensitivity;

    public SettingsData(SettingsManager settingsmanager)
    {
        SoundSettings = settingsmanager.VolumeSettings;
        FullScreen = settingsmanager.fullscreen;
        Brightness = settingsmanager.brightness;
        Sensitivity = settingsmanager.sensitivity;
    }
}
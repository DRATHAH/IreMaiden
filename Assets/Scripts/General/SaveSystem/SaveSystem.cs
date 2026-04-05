using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public static class SaveSystem
{
    //Filename for storing best time, best rank, etc
    public const string FILENAME_SAVEDATA = "/savedata.json";

    public static void SaveGameState()
    {
        string filePathLevelSaveData = Application.persistentDataPath + FILENAME_SAVEDATA;
        levelData leveldata = new levelData(DataManager.Instance);
        SaveData saveData = new SaveData(leveldata);
        string txt = JsonUtility.ToJson(saveData);
        File.WriteAllText(filePathLevelSaveData, contents: txt);
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
public class levelData
{
    //Levels
    [SerializeField] public float[] masterLevelList;
    [SerializeField] public float[] Level1Stats;


    public levelData(DataManager datamanager)
    {
        //Levels
        Level1Stats = datamanager.level1Results;
    }
}


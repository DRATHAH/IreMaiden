using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public static class LoadSystem
{
    public static SaveData LoadGameData()
    {
        string filePathLevel = Application.persistentDataPath + SaveSystem.FILENAME_SAVEDATA;

        if (File.Exists(filePathLevel))
        {
            using (StreamReader stream = new StreamReader(filePathLevel))
            {
                string jsonString = stream.ReadToEnd();
                SaveData CurrentSave = JsonUtility.FromJson<SaveData>(jsonString);
                return CurrentSave;
            }
        }
        else
        {
            return null;
        }

    }

    public static SettingsSaveData LoadSettingsData()
    {
        string filePathSettings = Application.persistentDataPath + SaveSystem.FILENAME_SETTINGSDATA;
        if (File.Exists(filePathSettings))
        {
            using (StreamReader stream = new StreamReader(filePathSettings))
            {
                string jsonString = stream.ReadToEnd();
                SettingsSaveData CurrentSave = JsonUtility.FromJson<SettingsSaveData>(jsonString);
                return CurrentSave;
            }
        }
        else
        {
            return null;
        }
    }
}

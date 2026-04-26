using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance; //One universal datamanager

    public GameObject SaveWarning;

    //LevelSOs
    public FloatArraySO level1;

    //stuff to send to saveData
    [HideInInspector] public float[] level1Results;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        SaveData savedata = LoadSystem.LoadGameData();
        if (savedata != null)
        {
            if(savedata.leveldata.Level1Stats.Length > 0)
            {
                level1.Value[0] = savedata.leveldata.Level1Stats[0];
                level1.Value[1] = savedata.leveldata.Level1Stats[1];
                level1.Value[2] = savedata.leveldata.Level1Stats[2];
            }
            else
            {
                Debug.LogWarning("Save Data Corrupted! Refreshing File!");
                level1.Value[0] = 0;
                level1.Value[1] = 0;
                level1.Value[2] = 0;
                SaveTheGame();
            }
            //Debug.Log(savedata.leveldata.Level1Stats[0]);
            //Debug.Log(savedata.leveldata.Level1Stats[1]);
            //Debug.Log(savedata.leveldata.Level1Stats[2]);
        }
    }

    public void UpdateVars()
    {
        level1Results = level1.Value;
    }

    public void SaveTheGame()
    {
        UpdateVars();
        SaveWarning.SetActive(true);
        SaveSystem.SaveGameState();
        SaveWarning.SetActive(false);
    }

    public void ResetData()
    {
        level1.Value = new float[3] { 0, 0, 0 };
        level1Results = level1.Value;
        UpdateVars();
        SaveTheGame();
    }
}

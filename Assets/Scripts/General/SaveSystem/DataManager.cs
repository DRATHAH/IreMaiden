using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance; //One universal datamanager

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
            level1.Value = savedata.leveldata.Level1Stats;
        }
    }

    public void UpdateVars()
    {
        level1Results = level1.Value;
    }

    public void SaveTheGame()
    {
        UpdateVars();
        SaveSystem.SaveGameState();
    }

    public void ResetData()
    {
        level1.Value = new float[3] { 0, 0, 0 };
        UpdateVars();
        SaveTheGame();
    }
}

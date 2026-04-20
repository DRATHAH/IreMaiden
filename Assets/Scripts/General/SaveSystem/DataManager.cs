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
            level1.Value[0] = savedata.leveldata.Level1Stats[0];
            level1.Value[1] = savedata.leveldata.Level1Stats[1];
            level1.Value[2] = savedata.leveldata.Level1Stats[2];
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
        SaveSystem.SaveGameState();
    }

    public void ResetData()
    {
        level1.Value = new float[3] { 0, 0, 0 };
        UpdateVars();
        SaveTheGame();
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndOfLevel : MonoBehaviour
{
    private GameManager GM;
    private DataManager DM;

    public FloatArraySO levelData;
    float[] pastValues;
    private float FinalTime;

    //Text to update on levelScreen
    public GameObject EOLCanvas;
    
    public Image RankImage;
    public Sprite[] Ranks;

    public TextMeshProUGUI[] TimerDisplay;
    public TextMeshProUGUI[] BestTimeDisplay;

    public TextMeshProUGUI KillCountText;

    //Stuff for rankCalc
    public int FinalRank;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GM = this.gameObject.GetComponent<GameManager>();
        DM = this.gameObject.GetComponent<DataManager>();
        pastValues = levelData.Value;
    }

    public void FinishLevel()
    {
        GM.PauseGame();
        MusicManager.StopMusic();

        CalculateRank();
        CompareValues();
        DM.SaveTheGame();
        UpdateEndScreen();
    }

    //Func for determining player's rank on the level
    void CalculateRank()
    {
        //Timer Rank Calc
        FinalTime = GM.LevelMinutes + (GM.LevelSeconds * 0.01f);

        float TimerRank = 0;
        if(FinalTime <= GM.RankTimeRequirements[0])
        {
            TimerRank = 4;
        }
        else if(GM.RankTimeRequirements.Length > 1 && FinalTime <= GM.RankTimeRequirements[1])
        {
            TimerRank = 3;
        }
        else if(GM.RankTimeRequirements.Length > 2 && FinalTime <= GM.RankTimeRequirements[2])
        {
            TimerRank = 2;
        }
        else
        {
            TimerRank = 1;
        }

        //Kills Rank Calc
        float KillRank = 0;
        if(GM.KillCount >= GM.RankKillRequirements[0])
        {
            KillRank = 4;
        }
        else if(GM.RankKillRequirements.Length > 1 && GM.KillCount >= GM.RankKillRequirements[1])
        {
            KillRank = 3;
        }
        else if(GM.RankKillRequirements.Length > 2 && GM.KillCount >= GM.RankKillRequirements[2])
        {
            KillRank = 2;
        }
        else
        {
            KillRank = 1;
        }

        //Average the two ranks
        float AverageRank = Mathf.Ceil((TimerRank + KillRank) / 2);

        Debug.Log(TimerRank);
        Debug.Log(KillRank);
        Debug.Log(AverageRank);

        //Assign Final Rank
        if (TimerRank == 4 && KillRank  == 4 && GM.PlayerDeaths == 0)
        {
            FinalRank = 5;
        }
        else if(AverageRank == 4 && GM.PlayerDeaths > 0)
        {
            FinalRank = 4;
        }
        else if(AverageRank == 3)
        {
            FinalRank = 3;
        }
        else if(AverageRank == 2)
        {
            FinalRank = 2;
        }
        else
        {
            FinalRank = 1;
        }

    }
    
    //Compare this rank and current rank
    void CompareValues()
    {

        if (FinalRank > pastValues[0])
        {
            pastValues[0] = FinalRank;
        }

        if ((FinalTime < pastValues[1]) || (pastValues[1] == 0))
        {
            pastValues[1] = FinalTime;
        }

        if((FinalTime < pastValues[2] && FinalRank == 5) || (pastValues[2] == 0 && FinalRank ==5))
        {
            pastValues[2] = FinalTime;
        }

        levelData.Value = new float[3] { pastValues[0], pastValues[1], pastValues[2] };
    }

    //Stuff to Update Text on Level Menu
    void UpdateEndScreen()
    {
        EOLCanvas.SetActive(true);

        switch (FinalRank)
        {
            case 5:
                RankImage.sprite = Ranks[5];
                break;
            case 4:
                RankImage.sprite = Ranks[4];
                break;
            case 3:
                RankImage.sprite = Ranks[3];
                break;
            case 2:
                RankImage.sprite = Ranks[2];
                break;
            case 1:
                RankImage.sprite = Ranks[1];
                break;
            default:
                RankImage.sprite = Ranks[0];
                break;
        }

        float LevelMinutes = Mathf.Floor(FinalTime);
        FinalTime -= LevelMinutes;
        float LevelSeconds = Mathf.Floor(FinalTime * 100);

        TimerDisplay[0].text = LevelMinutes.ToString();
        TimerDisplay[1].text = LevelSeconds.ToString("00");

        LevelMinutes = Mathf.Floor(pastValues[1]);
        pastValues[1] -= LevelMinutes;
        LevelSeconds = Mathf.Floor(pastValues[1] * 100);

        BestTimeDisplay[0].text = LevelMinutes.ToString();
        BestTimeDisplay[1].text = LevelSeconds.ToString("00");

        KillCountText.text = GM.KillCount.ToString();
    }
}

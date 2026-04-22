using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class MainMenuManager : MonoBehaviour
{
    public FloatArraySO level1;
    private float[] levelData;

    public TextMeshProUGUI[] BestTimeDisplay;
    public TextMeshProUGUI[] BestPTimeDisplay;

    public GameObject BestTimeContainer;
    public GameObject BestPTimeContainer;

    public GameObject RankContainer;
    public Image levelRank;
    public Sprite[] Ranks;


    void Start()
    {
        Time.timeScale = 1f;
        levelData = level1.Value;
        PrepareMenu();
    }

    public void PrepareMenu()
    {
        levelData = level1.Value;
        ShowRank();
        if (levelData[1] == 0)
        {
            UpdateTimerDisplay(0, BestTimeDisplay);
        }
        else
        {
            UpdateTimerDisplay(levelData[1], BestTimeDisplay);
        }

        if (levelData[2] == 0)
        {
            UpdateTimerDisplay(0, BestPTimeDisplay);
        }
        else
        {
            UpdateTimerDisplay(levelData[2], BestPTimeDisplay);
        }
    }

    private void UpdateTimerDisplay(float time, TextMeshProUGUI[] timerBox)
    {
        if(time == 0)
        {
            BestTimeContainer.SetActive(false);
            BestPTimeContainer.SetActive(false);
        }
        else
        {
            BestTimeContainer.SetActive(true);
            BestPTimeContainer.SetActive(true);

            float LevelMinutes = Mathf.Floor(time);
            time -= LevelMinutes;
            float LevelSeconds = Mathf.Floor(time * 100);

            timerBox[0].text = LevelMinutes.ToString();
            timerBox[1].text = LevelSeconds.ToString();
        }
    }

    private void ShowRank()
    {
        RankContainer.SetActive(true);
        switch (levelData[0])
        {
            case 0:
                RankContainer.SetActive(false);
                break;
            case 1:
                levelRank.sprite = Ranks[1];
                break;
            case 2:
                levelRank.sprite = Ranks[2];
                break;
            case 3:
                levelRank.sprite = Ranks[3];
                break;
            case 4:
                levelRank.sprite = Ranks[4];
                break;
            case 5:
                levelRank.sprite = Ranks[5];
                break;
        }
    }

    public void LoadScene(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void QuitGame()
    {
        SaveSystem.SaveGameState();
        Application.Quit();
    }
}

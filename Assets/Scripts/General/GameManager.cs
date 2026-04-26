using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using IM;

public class GameManager : MonoBehaviour
{    
    #region Singleton
    public static GameManager instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    public bool TimerActive = false; //Is the time in level incrementing?
     public float LevelMinutes = 0; //Time spent in the level
     public float LevelSeconds = 0;
    public float ParTime; //Time for player to beat (Minutes.Seconds)

    [HideInInspector] public int KillCount = 0; //Number of enemies player has killed
    public int SavedKillCount = 0; //Number of enemies player has killed after going through a checkpoint
    public int RequiredEnemies; //Number of enemies player needs to kill for best rank

    [HideInInspector] public int PlayerDeaths = 0; //Number of times player has died

    public GameObject[] EnemySpawns; //Spawners in level
    [HideInInspector] public List<bool> FinishedArenas; //Spawners player has cleared
    [HideInInspector] public List<bool> TempFinishedArenas;

    public GameObject PlayerContainer; //Player reference
    private PlayerHealth PH;
    private PlayerLocomotionManager PLM; //Locomotion manager reference
    private PlayerWeaponry PW; //Player weapon reference
    private Vector3 PlayerSpawnOriginal; //Player's starting position
    public Vector3 PlayerSpawnActive; //Player's current spawn point

    public GameObject DeathScreenCanvas; //UI canvas for player death

    //Key Manager
    public bool[] LevelKeys; //Array to store keys
    public GameObject[] RespawnableObjects; //Objects to be reset upon player restarting level (Keys, Doors)

    //PauseMenu
    public GameObject pauseMenu;
    private bool paused;
    public Button[] BackButton;

    void Start()
    {
        PlayerContainer = GameObject.Find("PlayerContainer"); //Find the player
        if(PlayerContainer != null)
        {
            PH = PlayerContainer.GetComponentInChildren<PlayerHealth>();
            PLM = PlayerContainer.GetComponentInChildren<PlayerLocomotionManager>(); //Define PLM
            PW = PlayerContainer.GetComponentInChildren<PlayerWeaponry>(); //Define PW
            PH.gamemanager = this; //Define player health manager's gamemanager

            PlayerSpawnOriginal = PlayerContainer.transform.position;
            PlayerSpawnActive = PlayerSpawnOriginal; //Set checkpoint spawn point
        }

        if (EnemySpawns.Length > 0)
        {
            for (int i = 0; i < EnemySpawns.Length; i++)
            {
                WaveSpawner currentWaveSpawner = EnemySpawns[i].GetComponent<WaveSpawner>(); //Get ref to wave spawner
                currentWaveSpawner.PosInArray = i; //Tell it its array pos
                currentWaveSpawner.gamemanager = this; //Tell it what a gamemanager is

                FinishedArenas.Add(false); //Add a placeholder in finished arenas
                TempFinishedArenas.Add(false);
            }
        }
    }

    public void Update()
    {
        if(TimerActive == true && LevelSeconds < 9999.99f)
        {
            LevelSeconds += Time.deltaTime;
            LevelSeconds = Mathf.Clamp(LevelSeconds, 0, 60);
        }

        if(LevelSeconds > 60)
        {
            LevelSeconds -= 60;
            LevelMinutes += 1;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && paused == false)
        {
            PauseGame();
            paused = true;
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && paused == true)
        {
            for (int i = 0; i < BackButton.Length; i++)
            {
                if (BackButton[i].gameObject.activeInHierarchy == true)
                {
                    BackButton[i].onClick.Invoke();
                }
            }
        }
    }


    public void PlayerDeath() //When player dies
    {
        Debug.Log("Die");
        MusicManager.SetVolume(0);
        PLM.CanMove = false; //disable player movement
        for(int i = 0; i < EnemySpawns.Length; i++)
        {
            //Despawn enemies
            WaveSpawner currentWaveSpawner = EnemySpawns[i].GetComponent<WaveSpawner>();
            currentWaveSpawner.ResetProgress();
        }
        DeathScreenCanvas.SetActive(true); //enable death screen
        //Delete All Projectiles, or Reset Object Pool (To be implemented)
    }

    public void RestartFromCheckpoint() //Restarting from checkpoint
    {
        if(PlayerSpawnActive == PlayerSpawnOriginal)
        {
            UnpauseGame();
            RestartLevel();
        }
        else
        {
            PlayerContainer.GetComponentInChildren<Rigidbody>().MovePosition(PlayerSpawnActive); //Set player pos to checkpoint pos
            PH.ResetHealth();
            DeathScreenCanvas.SetActive(false); //Disable death screen
            PLM.CanMove = true; //player can move again
            KillCount = SavedKillCount; //Reset kill count to what it was when checkpoint was hit
            PlayerDeaths++;
            if (EnemySpawns.Length > 0)
            {
                for (int i = 0; i < EnemySpawns.Length; i++)
                {
                    if (FinishedArenas[i] == false)
                    {
                        //Reset enemy arenas
                        EnemySpawns[i].SetActive(true);
                        TempFinishedArenas[i] = false;
                    }
                }
            }
            if(RespawnableObjects.Length > 0)
            {
                for (int j = 0; j < RespawnableObjects.Length; j++)
                {
                    RespawnableObjects[j].SetActive(true);
                }
            }
            MusicManager.UnpauseMusic();
            UnpauseGame();
        }
    }

    public void RestartLevel()
    {
        UnpauseGame();
        Scene sceneToReload = SceneManager.GetActiveScene();
        SceneManager.LoadScene(sceneToReload.name);
    }

    public void QuitLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void SetCheckpoint(Vector3 CheckpointSpawn)
    {
        PlayerSpawnActive = CheckpointSpawn;
        SavedKillCount = KillCount;
        for (int i = 0; i < EnemySpawns.Length; i++)
        {
            FinishedArenas[i] = TempFinishedArenas[i];
        }
    }

    public void UpdateArenas(int posInArray)
    {
        TempFinishedArenas[posInArray] = true;
    }

    public void PauseGame()
    {
        TimerActive = false;
        MusicManager.PauseMusic();
        Time.timeScale = 0;
        PLM.CanMove = false;
        PW.CanShoot = false;
        pauseMenu.SetActive(true);
    }

    public void UnpauseGame()
    {
        MusicManager.UnpauseMusic();
        paused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        PLM.CanMove = true;
        PW.CanShoot = true;
        TimerActive = true;
    }

}

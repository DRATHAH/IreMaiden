using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  IM;

public class GameManager : MonoBehaviour
{
    //private bool TimerActive = false; //Is the time in level incrementing?
   // private float LevelTime = 0; //Time spent in the level
   // public float ParTime; //Time for player to beat

    private int KillCount = 0; //Number of enemies player has killed
    public int SavedKillCount = 0; //Number of enemies player has killed after going through a checkpoint
    public int RequiredEnemies; //Number of enemies player needs to kill for best rank

    private int PlayerDeaths = 0; //Number of times player has died

    public GameObject[] EnemySpawns; //Spawners in level
    [HideInInspector] public List<bool> FinishedArenas; //Spawners player has cleared

    private GameObject Player; //Player reference
    private PlayerLocomotionManager PLM; //Locomotion manager reference
    private Vector3 PlayerSpawnOriginal; //Player's starting position
    public Vector3 PlayerSpawnActive; //Player's current spawn point

    public GameObject DeathScreenCanvas; //UI canvas for player death

    private bool DoneFromMenu = true; //If player is resetting from the pause menu or death menu
    void Start()
    {
        Player = GameObject.Find("Player"); //Find the player
        PLM = Player.GetComponent<PlayerLocomotionManager>(); //Define PLM
        Player.GetComponent<PlayerHealth>().gamemanager = this; //Define player health manager's gamemanager

        PlayerSpawnOriginal = Player.transform.position; //Set player origin
        PlayerSpawnActive = PlayerSpawnOriginal; //Set checkpoint spawn point

        if (EnemySpawns.Length > 0)
        {
            for (int i = 0; i < EnemySpawns.Length; i++)
            {
                WaveSpawner currentWaveSpawner = EnemySpawns[i].GetComponent<WaveSpawner>(); //Get ref to wave spawner
                currentWaveSpawner.PosInArray = i; //Tell it its array pos
                currentWaveSpawner.gamemanager = this; //Tell it what a gamemanager is

                FinishedArenas.Add(false); //Add a placeholder in finished arenas
            }
        }
    }

    public void Update()
    {
        //Timer shit to be fully implemented later
        /*if(TimerActive == true && LevelTime < 9999.99f)
        {
            LevelTime += Time.deltaTime;
            LevelTime = Mathf.Clamp(LevelTime, 0, 9999.99f);
        } */

    }


    public void PlayerDeath() //When player dies
    {
        PlayerDeaths++;
        DoneFromMenu = false;
        DeathScreenCanvas.SetActive(true); //enable death screen
        PLM.CanMove = false; //disable player movement
        for(int i = 0; i < EnemySpawns.Length; i++)
        {
            //Despawn enemies
            WaveSpawner currentWaveSpawner = EnemySpawns[i].GetComponent<WaveSpawner>();
            currentWaveSpawner.ResetProgress();
        }
        //Delete All Projectiles, or Reset Object Pool (To be implemented)
    }


    public void RestartFromCheckpoint() //Restarting from checkpoint
    {
        if (EnemySpawns.Length > 0)
        {
            for (int i = 0; i < EnemySpawns.Length; i++)
            {
                if (FinishedArenas[i] != true)
                {
                    //Reset enemy arenas
                    EnemySpawns[i].SetActive(true);
                }
            }
        }
        if(DoneFromMenu == true)
        {
            PlayerDeaths++; //If resetting from pause menu, add a death for rank consideration
        }
        KillCount = SavedKillCount; //Reset kill count to what it was when checkpoint was hit
        Player.transform.position = PlayerSpawnActive; //Set player pos to checkpoint pos
        DeathScreenCanvas.SetActive(false); //Disable death screen
        PLM.CanMove = true; //player can move again
        DoneFromMenu = true; //Reset donefrommenu
    }

    public void RestartLevel()
    {
        if (EnemySpawns.Length > 0)
        {
            for (int i = 0; i < EnemySpawns.Length; i++)
            {
                //Reset arenas back to their original values
                EnemySpawns[i].SetActive(true);
                FinishedArenas[i] = false;
            }
        }
       // TimerActive = false; //Timer isn't active for spawn room purposes
        //LevelTime = 0; //Level time is reset
        KillCount = 0; //Kill count is reset
        SavedKillCount = KillCount; //Saved kill count is reset to prevent exploits
        Player.transform.position = PlayerSpawnOriginal; //Set player back to their original spawn point
        PlayerSpawnActive = PlayerSpawnOriginal; //Reset checkpoint spawn
        DeathScreenCanvas.SetActive(false); //Turn off deathscreen canvas
        PLM.CanMove = true; //Player can no longer move
        DoneFromMenu = true; //DoneFromMenu is reset
    }

    public void QuitLevel()
    {
        //Temp Quit level function, to be implemented later
        Debug.Log("Not Implemented Yet");
        RestartLevel();
    }
}

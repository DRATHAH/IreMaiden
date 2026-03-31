using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public GameObject[] EnemiesToSpawn; // Array containing the enemies spawned by the spawner
    public int[] EnemiesPerWave; //Array containing the number of enemies per wave
    private List<GameObject> FreshMeat = new List<GameObject>(); //List containing the active enemies

    public GameObject[] Gates; //Array containing the gates that trap a player in a room (If there are any)

    private int CurrentWave; //The current enemy wave number
    private int TotalWaves; //Total number of enemy waves
    private int EnemiesToSkip = 0; //Array cells to skip while spawning enemies

    private int DeadEnemies; //Number of dead enemies in an active wave

    private BoxCollider SpawnTrigger; //Ref to trigger that spawns enemies

    [HideInInspector]public GameManager gamemanager;
    [HideInInspector]public int PosInArray;

    void Start()
    {
        SpawnTrigger = this.GetComponent<BoxCollider>(); //Define the trigger
        SpawnTrigger.enabled = true; //Enable the trigger (For resets)
        TotalWaves = EnemiesPerWave.Length; //Determine the number of spawns
        DeadEnemies = 0;
        CurrentWave = 0; //Current wave (For resets)
        EnemiesToSkip = 0; //Set Enemies to skip (For resets)
        for (int i = 0; i < EnemiesToSpawn.Length; i++)
        {
            EnemiesToSpawn[i].gameObject.SetActive(false);
        }
    }

    private void SpawnWave()
    {
        for (int i = 0; i < EnemiesPerWave[CurrentWave]; i++)
        {
            GameObject NewBlood = EnemiesToSpawn[i + EnemiesToSkip]; //Grab the reference to the enemy
            NewBlood.GetComponent<EnemyHealth>().WaveSource = this; //Define the enemy health's var for this
            NewBlood.SetActive(true); //Activate the object
            FreshMeat.Add(NewBlood); //Add the object to the active enemy list
        }

    }

    //Have this run if the player dies during a wave (Or at all)
    public void ResetProgress()
    {
        if(FreshMeat != null)
        {
            for (int i = 0; i < EnemiesToSpawn.Length; i++)
            {
                EnemiesToSpawn[i].GetComponent<EnemyHealth>().ResetEnemy();
            }
            DespawnGates();
            Start(); //Reset Values
            FreshMeat.Clear(); //Clear the active enemy list
        }
    }

    public void NextUp()
    {
        DeadEnemies = 0; //Reset Number of dead enemies
        EnemiesToSkip += EnemiesPerWave[CurrentWave]; //Add defeated enemies to skip counter
        CurrentWave++; //Increment wave
        FreshMeat.Clear(); //Clear active enemies list
        SpawnWave(); //Spawn the new wave
    }

    public void EnemyDied() //To be called by an enemy when it dies
    {
        DeadEnemies++; //+1 to number of dead enemies
        if (FreshMeat != null && DeadEnemies == FreshMeat.Count && FreshMeat.Count > 0) //Detemine if the enemies can spawn
        {
            if (CurrentWave + 1 < TotalWaves) //Are we at the end of the total waves
            {
                NextUp(); //Spawn the enemies
            }
            else
            {
                DespawnGates(); //Disable gates
                if(gamemanager != null)
                {
                    gamemanager.UpdateArenas(PosInArray);
                }
                DeactivateSelf(); //Turn off the spawner if all enemies are dead
            }
        }
    }


    //Trigger Stuff
    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player") //Is it the player?
        {
            SpawnGates(); //Lock the player in if necessary
            SpawnWave(); //Spawn first wave
            SpawnTrigger.enabled = false; //Turn off trigger
        }
    }

    public void DeactivateSelf()
    {
        SpawnTrigger.enabled = true; //Turn on the trigger
        this.gameObject.SetActive(false); //Turn off the game object. To be re-enabled if necessary
    }


    private void SpawnGates()
    {
        if(Gates != null && Gates.Length > 0)//Check if gates exist
        {
            for (int i = 0; i < Gates.Length; i++)
            {
                Gates[i].SetActive(true); //Set gates to be active
            }
        }
    }

    private void DespawnGates()
    {
        if (Gates != null && Gates.Length > 0) //Check if gates exist
        {
            for (int i = 0; i < Gates.Length; i++)
            {
                Gates[i].SetActive(false); //Set gates to be inactive
            }
        }
    }


}

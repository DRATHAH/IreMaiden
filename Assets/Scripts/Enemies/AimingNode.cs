using UnityEngine;

public class AimingNode : MonoBehaviour
{
    //All of this could be better optimized, this is just the ramshackle way I did it
    //Identify Player
    private GameObject player;

    void Start()
    {
        //define player
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //Aim at the player for projectile rotation purposes
        transform.LookAt(player.transform);
    }
}

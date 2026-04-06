using UnityEngine;

public class KeyCheck : MonoBehaviour
{
    private GameManager GM; //Game manager storage

    public bool RedDoor; //Is the red door?
    public bool BlueDoor; //Is the blue door?
    public bool YellowDoor; //Is the yellow door?


    void Start()
    {
        GM = GameObject.Find("Game Manager").GetComponent<GameManager>(); //Find Gamemanager component
    }


    void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            if (YellowDoor == true && GM.LevelKeys[0] == true)
            {
                this.gameObject.SetActive(false); //If has yellow key and is yellow door, deactivate
            }
            else if (BlueDoor == true && GM.LevelKeys.Length >= 2 && GM.LevelKeys[1] == true)
            {
                this.gameObject.SetActive(false); //If has blue key and is blue door, deactivate
            }
            else if (RedDoor == true && GM.LevelKeys.Length >= 3 && GM.LevelKeys[2] == true)
            {
                this.gameObject.SetActive(false); //If has red key and is red door, deactivate
            }
        }
    }
}

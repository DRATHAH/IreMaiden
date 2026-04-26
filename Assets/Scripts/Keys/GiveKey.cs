using UnityEngine;

public class GiveKey : MonoBehaviour
{
    public bool RedKey; //Is red key?
    public bool BlueKey; //Is blue key?
    public bool YellowKey; //Is yellow key?

    private GameManager GM; //Game manager storage

    void Start()
    {
        GM = GameObject.Find("Game Manager").GetComponent<GameManager>(); //Grab gamemanager
    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            if(RedKey == true && GM.LevelKeys.Length >= 3)
            {
                GM.LevelKeys[2] = true; //Player now has red key
                this.gameObject.SetActive(false); //Toggle key off to show it has been grabbed
            }
            else if(BlueKey == true && GM.LevelKeys.Length >= 2)
            {
                GM.LevelKeys[1] = true; //Player now has blue key
                this.gameObject.SetActive(false);
            }
            else
            {
                GM.LevelKeys[0] = true; //Player now has yellow key, feel free to change the order for any reason
                this.gameObject.SetActive(false);
            }
        }
    }
}

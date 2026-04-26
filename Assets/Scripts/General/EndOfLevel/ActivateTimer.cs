using UnityEngine;

public class ActivateTimer : MonoBehaviour
{
    private GameManager GM;
    void Start()
    {
        GM = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            GM.TimerActive = true;
        }
    }
}

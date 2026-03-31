using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private GameManager GM;
    public bool reusable = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GM = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GM.SetCheckpoint(transform.position);
            this.gameObject.SetActive(false);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && reusable == true) //Allows for reusable checkpoints so we don't need to place multiple
        {
            this.gameObject.SetActive(true);
        }
    }
}

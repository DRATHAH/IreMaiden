using UnityEngine;

public class EoLTrigger : MonoBehaviour
{
    private EndOfLevel EOL;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EOL = GameObject.Find("Game Manager").GetComponent<EndOfLevel>();
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            EOL.FinishLevel();
        }
    }
}

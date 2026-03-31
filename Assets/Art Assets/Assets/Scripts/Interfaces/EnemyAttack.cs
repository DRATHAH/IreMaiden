using UnityEngine;

public class EnemyAttack : MonoBehaviour, IAttack
{
    public virtual void StopAttacking()
    {
        /*
         * This is to create a uniform "Stop Attacking" function that can be called
         * across different enemy scripts. This should be overrided, if not
         * an error will be called by the script
         */
        Debug.LogError("Default Program Ran! You Forgot to Override!");
    }
}

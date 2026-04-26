using UnityEngine;

namespace IM
{
    public class PlayerCameraManager : MonoBehaviour
    {
        public Transform player;

        void Update()
        {
            transform.position = player.transform.position;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    private static SFXManager instance;

    [SerializeField]private GameObject sfxPlayer;

    private void Awake()
    {
        instance = this;
    }

    public static void PlaySound(AudioClip sound, Vector3 spawnPos)
    {
        GameObject currentsfx = Instantiate(instance.sfxPlayer, spawnPos, Quaternion.identity) as GameObject;
        currentsfx.GetComponent<PlaySFX>().AudioToPlay = sound;
        currentsfx.GetComponent<PlaySFX>().PlaySound();
    }
}
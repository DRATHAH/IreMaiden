using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;

    private AudioSource MusicPlayer;
    public float HighIntensityVolume;
    public float LowIntensityVolume;
    public float PauseVolume;
    private float LastVolume;
    private float finalVol;
    private bool fadeIn;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        MusicPlayer = GetComponent<AudioSource>();
        MusicPlayer.volume = LowIntensityVolume;
        instance.LastVolume = instance.LowIntensityVolume;
    }

    private void Update()
    {
        if(fadeIn == true)
        {
            float currentVol = instance.MusicPlayer.volume;
            instance.MusicPlayer.volume = Mathf.Lerp(currentVol, instance.finalVol, 3 * Time.deltaTime);
            if (currentVol == finalVol)
            {
                instance.fadeIn = false;
            }
        }
    }

    public static void SetVolume(float vol) //Volume is a float from 0 to 1
    {
        instance.finalVol = vol;
        instance.fadeIn = true;
    }


    public static void IncreaseIntensity()
    {
        instance.finalVol = instance.HighIntensityVolume;
        instance.LastVolume = instance.HighIntensityVolume;
        instance.fadeIn = true;
    }

    public static void DecreaseIntensity()
    {
        instance.finalVol = instance.LowIntensityVolume;
        instance.LastVolume = instance.LowIntensityVolume;
        instance.fadeIn = true;
    }

    public static void PauseMusic()
    {
        instance.MusicPlayer.volume = instance.PauseVolume;
    }

    public static void UnpauseMusic()
    {
        instance.finalVol = instance.LastVolume;
        instance.fadeIn = true;
    }

    public static void StopMusic()
    {
        instance.MusicPlayer.Stop();
    }

    public static void RestartMusic()
    {
        instance.MusicPlayer.Play();
    }
}

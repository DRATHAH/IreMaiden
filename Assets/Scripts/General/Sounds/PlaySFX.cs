using UnityEngine;
using System.Collections;


public class PlaySFX : MonoBehaviour
{
    [SerializeField] private AudioSource AS;

    public AudioClip AudioToPlay;

    public void PlaySound()
    {
        AS.PlayOneShot(AudioToPlay, 1);
        StartCoroutine(DestroySource(AudioToPlay.length));
    }

    private IEnumerator DestroySource(float AudioLength)
    {
        yield return new WaitForSeconds(AudioLength);
        Destroy(this.gameObject);
    }
}

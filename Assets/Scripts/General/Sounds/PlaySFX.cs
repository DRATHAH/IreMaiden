using UnityEngine;

public class PlaySFX : MonoBehaviour
{

    public void PlaySound3D(AudioClip AudioToPlay, Vector3 ListeningPosition, Vector3 SourcePosition, float Volume)
    {
        SFXManager.PlaySound(AudioToPlay, DistanceVolume(ListeningPosition, SourcePosition, Volume));
    }

    public void PlaySound2D(AudioClip AudioToPlay, float Volume)
    {
        float FinalVolume = (Volume - -80) / (0 - -80);
        SFXManager.PlaySound(AudioToPlay, FinalVolume);
    }

    private float DistanceVolume(Vector3 StartingPoint, Vector3 EndingPoint, float InitialVolume)
    {
        float distance = Vector3.Distance(StartingPoint, EndingPoint);

        float FinalVolume = InitialVolume - 20 * Mathf.Log10(distance);

        return (FinalVolume - -78.62261f) / (-1.777344f - -78.62261f);
    }
}

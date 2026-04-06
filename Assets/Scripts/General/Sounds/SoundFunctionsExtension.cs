using UnityEngine;

namespace SoundFunctionsExtension
{
    public static class VolumeCalc
    {
        public static float DistanceVolume(this float VolumeVar, Vector3 StartingPoint, Vector3 EndingPoint, float InitialVolume)
        {
            float distance = Vector3.Distance(EndingPoint, StartingPoint);

            float FinalVolume = InitialVolume - 20 * Mathf.Log10(distance);

            return (FinalVolume - -78.62261f) / (-1.777344f - -78.62261f);
        }
    }
}

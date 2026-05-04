using UnityEngine;

public static class AudioHelper
{
    public static void PlayRandomPitch(AudioSource audioSource)
    {
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.Play();
    }
}

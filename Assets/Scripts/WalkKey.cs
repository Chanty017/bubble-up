using UnityEngine;

public class WalkKey : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip;

    public void PlaySound()
    {
        audioSource.PlayOneShot(audioClip);
    }
}

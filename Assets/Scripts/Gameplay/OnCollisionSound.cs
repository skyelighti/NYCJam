using UnityEngine;

public class OnCollisionSound : MonoBehaviour
{
    [SerializeField] AudioClip soundVFX;
    AudioSource audioSource;
    private void OnTriggerEnter(Collider other)
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null) {
            audioSource.PlayOneShot(soundVFX);
        }
    }
}

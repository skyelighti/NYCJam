using UnityEngine;

public class OnCollisionSound : MonoBehaviour
{
    [SerializeField] AudioClip soundVFX;
    [SerializeField] float soundVolume = 1f;
    private void OnTriggerEnter(Collider other)
    {
        AudioManager.Instance.PlaySFX(soundVFX, soundVolume);
    }
}

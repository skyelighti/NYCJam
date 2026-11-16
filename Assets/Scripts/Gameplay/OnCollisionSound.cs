using UnityEngine;

public class OnCollisionSound : MonoBehaviour
{
    [SerializeField] AudioClip soundVFX;
    private void OnTriggerEnter(Collider other)
    {
        AudioManager.Instance.PlaySFX(soundVFX);
    }
}

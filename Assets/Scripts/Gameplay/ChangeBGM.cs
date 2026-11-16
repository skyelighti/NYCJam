using UnityEngine;

public class ChangeBGM : MonoBehaviour
{
    [SerializeField]AudioClip newBGM;
    private void OnTriggerEnter(Collider other)
    {
        AudioManager.Instance.ChangeBGM(newBGM);
    }
}

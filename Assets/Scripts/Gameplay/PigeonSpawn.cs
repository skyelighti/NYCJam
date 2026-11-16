using UnityEngine;

public class PigeonSpawn : MonoBehaviour
{
    GameObject pigeon;
    private void Start()
    {
        pigeon.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            pigeon.SetActive(true);
        }
    }
}

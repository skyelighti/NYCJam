using UnityEngine;

public class IncreaseSpeed: MonoBehaviour
{
    [SerializeField] float increaseAmt;

    public void Start()
    {
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) { return; }
        Events.IncreaseSplineSpeed(increaseAmt);
        gameObject.SetActive(false);
    }

}

using System;
using UnityEngine;

public class CrowdLevelTrigger : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] ActionMap nextMap;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            GameManager.Instance.SwapMap(nextMap);
            gameObject.SetActive(false);
        }
    }
}

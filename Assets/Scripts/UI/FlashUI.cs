using System;
using System.Collections;
using System.Threading;
using UnityEngine;

public class FlashUI : MonoBehaviour
{
    [SerializeField] GameObject toShow;
    [SerializeField] float time;
    private void Start()
    {
        toShow.SetActive(false);
    }

    IEnumerator ShowthenHide(float time) {
        toShow.SetActive(true);
        yield return new WaitForSeconds(time);
        toShow.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.gameObject.layer == LayerMask.NameToLayer("Player")) {
            StartCoroutine(ShowthenHide(time));
        }
    }
}

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingUI : MonoBehaviour
{
    [SerializeField] Image UImage;
    IEnumerator Blink(int amounnt, float interval)
    {
        for (int i = 0; i < amounnt; i++) {
            UImage.enabled = true;
            yield return new WaitForSeconds(interval);
            UImage.enabled = false;
            yield return new WaitForSeconds(interval);
            UImage.enabled = true;
        }

    }
    public void StartBlink(int amt, float interval) {
        StopAllCoroutines();
        StartCoroutine(Blink(amt, interval));
    }
}

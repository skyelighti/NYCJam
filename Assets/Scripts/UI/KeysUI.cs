using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeysUI : MonoBehaviour
{
    [SerializeField] GameObject[] D3;
    [SerializeField] GameObject[] D2;
    [SerializeField] GameObject[] D1;
    Dictionary<ActionMap, GameObject[]> UImapping;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UImapping = new Dictionary<ActionMap, GameObject[]> { { ActionMap.D1Gameplay, D1 }, { ActionMap.D2Gameplay, D2 }, { ActionMap.D3Gameplay, D3 } };
        GameManager.Instance.ActionMapChanged += UpdateKeyUI;
        UpdateKeyUI(GameManager.Instance.currentActionMap);

    }

    public void UpdateKeyUI(ActionMap map)
    {
        if (map == ActionMap.D1Gameplay || map == ActionMap.D2Gameplay || map == ActionMap.D3Gameplay) {
            foreach (GameObject x in D1.Concat(D2).Concat(D3))
            {
                x.SetActive(false);
            }
            foreach (GameObject y in UImapping[map]) {
                y.SetActive(true);
            }

        }
        
    }
}

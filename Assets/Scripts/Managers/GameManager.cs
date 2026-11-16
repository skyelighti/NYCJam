using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public InputSystem_Actions inputActions;
    public event Action<ActionMap> ActionMapChanged;
    public ActionMap currentActionMap = ActionMap.D1Gameplay;
    public DayPhase currentTime = DayPhase.Morning;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        inputActions = new InputSystem_Actions();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SwapMap(ActionMap map)
    {
        foreach (InputActionMap m in inputActions.asset.actionMaps)
        {
            m.Disable();
        }
        inputActions.asset.FindActionMap(map.ToString()).Enable();
        currentActionMap = map;
        ActionMapChanged?.Invoke(map);
    }


    public void Pause()
    {
        Time.timeScale = 0;
        // show pause UI
    }
    public void Unpause()
    {
        Time.timeScale = 1f;
        //hide pause UI
    }
    
}
public enum ActionMap { D1Gameplay, D2Gameplay, D3Gameplay, UI, Dialouge, CustomUI }

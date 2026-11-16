using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class DayManager : MonoBehaviour
{
    public static DayManager Instance { get; set; }
    [SerializeField] Material skyBlendMaterial; 
    [SerializeField] Volume volA, volB;
    Material skyMat;
    Volume outgoingVol, incomingVol;
    Cubemap shownSky;
    float blend = 1f;
    StateMachine dayState = new();
    [SerializeField]DayInfo morning;
    [SerializeField] DayInfo noon;
    [SerializeField] DayInfo sunset;
    [SerializeField] DayInfo night;
    DayPhase currentTime;
    DayInfo currInfo;
    [SerializeField] Slider dayCycle;

    [SerializeField] float volumeBlendSpeed = 0.5f;

    private int timeCount = 0;

    Dictionary<DayPhase, DayInfo> daylookUp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        Instance = this;
    }
    void Start()
    {
        daylookUp = new Dictionary<DayPhase, DayInfo>() {
            {DayPhase.Morning, morning },
            {DayPhase.Noon, noon},
            {DayPhase.Sunset, sunset},
            {DayPhase.Night, night }
        };

        dayState.SetNumOfStates(4);
        dayState.SetStateFunctions((int)DayPhase.Morning, EnterMorning, UpdatePhase, ExitMorning);
        dayState.SetStateFunctions((int)DayPhase.Noon, EnterNoon, UpdatePhase, ExitNoon);
        dayState.SetStateFunctions((int)DayPhase.Sunset, EnterSunset, UpdatePhase, ExitSunset);
        dayState.SetStateFunctions((int)DayPhase.Night, EnterNight, UpdatePhase, ExitNight);
        dayState.SetState((int)DayPhase.Morning);

        skyMat = new Material(skyBlendMaterial);
        RenderSettings.skybox = skyMat;
        outgoingVol = volA; incomingVol = volB;
        shownSky = morning.sky;
        skyMat.SetTexture("_Tex", shownSky);
        skyMat.SetFloat("_CubemapTransition", 1f);
        dayCycle.value = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        dayState.UpdateState();    
    }

    public DayInfo LookUp() {
        return daylookUp[currentTime];
        //redundant but lazy
    }
    void EnterPhase(DayPhase p)   
    {
        currentTime = p;
        currInfo = LookUp();
        skyMat.SetTexture("_Tex", shownSky);         
        skyMat.SetTexture("_Tex_Blend", currInfo.sky);
        shownSky = currInfo.sky;

        (outgoingVol, incomingVol) = (incomingVol, outgoingVol);
        incomingVol.sharedProfile = currInfo.volume;

        blend = 0f;
        DynamicGI.UpdateEnvironment();   
    }
    public void EnterMorning() {
        EnterPhase(DayPhase.Morning);
    }
    void UpdatePhase()
    {
        blend = Mathf.MoveTowards(blend, 1f, volumeBlendSpeed * Time.deltaTime);
        incomingVol.weight = blend;
        outgoingVol.weight = 1f - blend;
        skyMat.SetFloat("_CubemapTransition", blend);
    }
    public void ExitMorning() { }

    public void EnterNoon() {
        EnterPhase(DayPhase.Noon);
    }

    public void ExitNoon() { }

    public void EnterSunset() {
        EnterPhase(DayPhase.Sunset);
    }
    public void ExitSunset() { }

    public void EnterNight() {
        EnterPhase(DayPhase.Night);
    }

    public void ExitNight() { }

    public void UpdateTimeCount(int add)
    {
        if (add < 0) { return; }
        timeCount += add;
        if (dayCycle != null || dayCycle.value == 1f)
        {
            dayCycle.value = (float)(timeCount) / 100;
        }
        DayPhase? nP = null;
        float best = float.MinValue;
        foreach (var dayp in daylookUp) {
            if (dayp.Value.startsAt <= timeCount && dayp.Value.startsAt >= best) {
                nP = dayp.Key;
                best = dayp.Value.startsAt;
            }
        }
        if (nP == null) { return; }
        if ((DayPhase)nP != (DayPhase)dayState.CurrentState) {
            dayState.SetState((int)nP);
        }
    }
}

public enum DayPhase { 
    Morning,
    Noon, 
    Sunset,
    Night
}

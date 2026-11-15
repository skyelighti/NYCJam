using System;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
public struct DayInfo
{
    //period of time
    public int startsAt;
    public Cubemap sky;
    public VolumeProfile volume;
}
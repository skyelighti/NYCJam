using System;
using UnityEngine;

public static class Events
{
    public static event Action<float> increaseSpeed;

    public static void IncreaseSplineSpeed(float amount) {
        increaseSpeed?.Invoke(amount);
    }
}

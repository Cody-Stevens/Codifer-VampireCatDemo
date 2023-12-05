using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    public static int Day;

    private void Awake()
    {
        GameEvents.GameStarted += GameStarted;
        GameEvents.WaveFinished += WaveFinished;
    }

    private void OnDestroy()
    {
        GameEvents.GameStarted -= GameStarted;
        GameEvents.WaveFinished -= WaveFinished;
    }

    private void GameStarted()
    {
        Day = 0;
    }

    private void WaveFinished(int day)
    {
        Day++;
    }
}

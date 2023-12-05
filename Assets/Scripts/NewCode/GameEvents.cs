using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameEvents
{
    public static Action<int> WaveStarted;
    public static void SendWaveStarted(int waveNumber)
    {
        WaveStarted?.Invoke(waveNumber);
    }

    public static Action<int> WaveFinished;
    public static void SendWaveFinished(int waveNumber)
    {
        WaveFinished?.Invoke(waveNumber);
    }

    public static Action GameStarted;
    public static void SendGameStarted()
    {
        GameStarted?.Invoke();
    }

    public static Action<int> GameOver;
    public static void SendGameOver(int day)
    {
        GameOver?.Invoke(day);
    }
}

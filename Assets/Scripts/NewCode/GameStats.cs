using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStats : MonoBehaviour
{
    public float SelectionStartRadius = 6f;
    public float SelectionRadiusSizeIncrease = 2f;
    public float AttackIncrease = 0f;
    public float HealthIncrease = 0f;
    public float AppleHealthIncrease = 0f;

    public static GameStats Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        GameEvents.GameStarted += OnGameStarted;
    }
    
    private void OnDestroy()
    {
        GameEvents.GameStarted -= OnGameStarted;
    }

    private void OnGameStarted()
    {
        AttackIncrease = 0f;
        HealthIncrease = 0f;
        AppleHealthIncrease = 0f;
    }
}

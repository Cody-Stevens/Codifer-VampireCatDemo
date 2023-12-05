using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBuilding : MonoBehaviour
{
    public int Level = 1;
    public int MaxLevel = 5;

    public int BaseCost = 10;
    public int CostIncrease = 10;
    public int CostToUpgrade;

    public bool FullyUpgraded => Level >= MaxLevel;

    protected virtual void Awake()
    {
        GameEvents.GameStarted += OnGameStarted;
    }

    protected virtual void OnDestroy()
    {
        GameEvents.GameStarted -= OnGameStarted;
    }

    public virtual void Upgrade()
    {
        Level++;
        CostToUpgrade = BaseCost + (Level-1)*CostIncrease;
    }

    public virtual void OnGameStarted()
    {
        Level = 1;
        CostToUpgrade = BaseCost;
    }
}

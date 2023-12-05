using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceBuilding : BaseBuilding
{
    protected override void Awake()
    {
        base.Awake();
        GameEvents.WaveFinished += GenerateFromEndOfRound;
    }

    protected override void OnDestroy()
    {
        GameEvents.WaveFinished -= GenerateFromEndOfRound;
        base.OnDestroy();
    }

    private void GenerateFromEndOfRound(int day)
    {
        Generate();
    }

    public virtual void Generate()
    {
    }
}

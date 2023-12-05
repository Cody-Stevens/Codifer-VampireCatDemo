using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TownHouse : ServiceBuilding
{
    public PoolableGameObject Pig;
    public Transform PlacePosition;

    private Damageable m_damageable;
    private int m_pigsSpawnedAtEndOfRound = 1;

    protected override void Awake()
    {
        base.Awake();
        m_damageable = GetComponent<Damageable>();
        m_damageable.OnDeath += OnDestroyed;
    }

    protected override void OnDestroy()
    {
        m_damageable.OnDeath -= OnDestroyed;
        base.OnDestroy();
    }

    private void OnDestroyed(Damageable damageable)
    {
        GameEvents.GameOver(DayManager.Day);
    }

    public override void Generate()
    {
        base.Generate();
        PoolableGameObject newPig = GameObjectPooler.Instance.FetchFromPool(Pig.ID);
        newPig.transform.position = PlacePosition.position;
    }

    public override void Upgrade()
    {
        base.Upgrade();
        m_pigsSpawnedAtEndOfRound++;
    }

    public override void OnGameStarted()
    {
        base.OnGameStarted();
        m_pigsSpawnedAtEndOfRound = 1;
    }
}

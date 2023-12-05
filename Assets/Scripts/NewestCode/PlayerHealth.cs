using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IPlayerDeath, IDamageable
{
    public float playerMaxHealth;
    public float playerCurrentHealth;
    public float healingRate = .25f;

    public static bool IsAlive { get; set; } = true;
    public static Action<Vector3> OnPlayerDeath { get; set; }
    public static Action<Vector3> OnPlayerDamageTaken { get; set; }
    public PlayerType PlayerType { get; set; }
    
    private void Awake()
    {
        IsAlive = true;
        PlayerType = PlayerType.Player;
    }
    private void Update()
    {
        if (playerCurrentHealth < playerMaxHealth)
        {
            playerCurrentHealth += Time.deltaTime * healingRate;
        }
    }
    public void TakeDamage(float damage)
    {
        if (playerCurrentHealth <= 0f) return;

        playerCurrentHealth -= damage * 2f;
        OnPlayerDamageTaken?.Invoke(transform.position);
        if (playerCurrentHealth <= 0)
        {
            Death();
        }
    }
    public void Death()
    {
        IsAlive = false;
        OnPlayerDeath?.Invoke(transform.position);
    }

    public float GetMaxHealth()
    {
        return playerMaxHealth;
    }

    public float GetCurrentHealth()
    {
        return playerCurrentHealth;
    }
}

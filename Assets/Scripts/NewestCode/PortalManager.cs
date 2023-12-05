using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortalManager : MonoBehaviour, IDamageable
{
    public float MaxHealth = 250;
    public float CurrentHealth { get; set; }
    public float ChargePercentage { get; set; }
    public PlayerType PlayerType { get; set; } = PlayerType.Player;

    [Header("References")]
    [SerializeField] GameObject VFX;

    // Actions
    public Action<GameObject> OnPortalEnter;
    public Action<Vector3> OnPortalDamaged;
    public Action<Vector3> OnPortalDead;

    public static PortalManager Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            CurrentHealth = MaxHealth;
        }
    }
    private void Update()
    {
        if (ChargePercentage >= 1 && CurrentHealth > 0)
        {
            VFX.SetActive(true);
        }
        else
        {
            VFX.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnPortalEnter?.Invoke(collision.gameObject);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        OnPortalEnter?.Invoke(collision.gameObject);
    }
    public void SetChargePercentage(float percentage)
    {
        ChargePercentage = percentage;
        if (ChargePercentage >= 1)
        {
            VFX.SetActive(true);
        }
        else
        {
            VFX.SetActive(false);
        }
    }

    public void TakeDamage(float damage)
    {
        if (CurrentHealth <= 0f) return;

        CurrentHealth -= damage /2f;
        OnPortalDamaged?.Invoke(transform.position);
        if (CurrentHealth <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        OnPortalDead?.Invoke(transform.position);
        //Implement game over
        Debug.Log("Game Over");
    }

    public float GetMaxHealth()
    {
        return MaxHealth;
    }

    public float GetCurrentHealth()
    {
        return CurrentHealth;
    }
}

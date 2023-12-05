using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Damageable : MonoBehaviour
{
    public int ArmyID;
    public AttackerVisuals Visuals;
    public int BaseMaxHealth;

    public Action StatsUpdated;
    public Action OnHit;
    public Action<Damageable> OnDeath;

    private float m_currentHealth;

    public float GetHealth()
    {
        return m_currentHealth;
    }

    public virtual float GetMaxHealth()
    {
        return BaseMaxHealth;
    }

    protected virtual void Awake()
    {
        m_currentHealth = GetMaxHealth();
        StatsUpdated?.Invoke();
    }

    public void GetAttacked(float damage)
    {
        m_currentHealth -= damage;
        if (m_currentHealth <= 0)
        {
            Die();
        }
        else
        {
            OnHit?.Invoke();
        }

        StatsUpdated?.Invoke();
    }

    public void Heal(float health)
    {
        m_currentHealth = Mathf.Min(m_currentHealth + health, GetMaxHealth());
        StatsUpdated?.Invoke();
    }

    public void HealFull()
    {
        m_currentHealth = GetMaxHealth();
        StatsUpdated?.Invoke();
    }

    public virtual void Die()
    {
        OnDeath?.Invoke(this);
        gameObject.SetActive(false);
    }
}

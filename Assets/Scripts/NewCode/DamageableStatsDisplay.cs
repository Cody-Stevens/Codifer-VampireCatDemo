using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamageableStatsDisplay : MonoBehaviour
{
    public Damageable Target;

    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Image healthbar;

    private void Awake()
    {
        Target.StatsUpdated += UpdateStats;
    }

    private void OnDestroy()
    {
        Target.StatsUpdated -= UpdateStats;
    }

    private void UpdateStats()
    {
        SetHealth();
    }

    private void SetHealth()
    {
        if (healthText == null) return;

        float health = Target.GetHealth();
        float maxHealth = Target.GetMaxHealth();

        healthText.text =  health +"/" + maxHealth;

        healthbar.fillAmount = (float)health / maxHealth;
        if (health > maxHealth)
        {
            healthText.color = Color.green;
        }
        else
        {
            if (health <= 3)
            {
                healthText.color = Color.red;
            }
            else
            {
                healthText.color = Color.white;
            }
        }
    }
}

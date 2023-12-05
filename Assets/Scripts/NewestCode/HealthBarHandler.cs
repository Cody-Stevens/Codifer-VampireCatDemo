using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarHandler : MonoBehaviour
{
    [SerializeField] GameObject damageableGO;
    private IDamageable damageable;
    [SerializeField] Image healthBar;

    private void Awake()
    {
        damageable = damageableGO.GetComponent<IDamageable>();
        if (damageable == null)
        {
            Debug.LogError("No IDamageable component found on " + damageableGO.name);
            return;
        }
    }
    private void FixedUpdate()
    {
        if (damageable == null) { return; }
        if (healthBar == null) { return;}
        healthBar.fillAmount = Mathf.Clamp01(damageable.GetCurrentHealth() / damageable.GetMaxHealth());
    }
}

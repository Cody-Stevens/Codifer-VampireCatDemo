using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonDamage: MonoBehaviour
{
    public static Action<Vector3> PoisonDamagetaken;
    [Header("Settings")]
    [SerializeField] CircleCollider2D colliderSize;
    [SerializeField] Transform VFX;
    public PlayerType targetType;
    [SerializeField] float tier1Damage = 1f;
    [SerializeField] float tier2Damage = 3f;
    [SerializeField] float tier3Damage = 5f;
    private float damage = 0f;
    private Weapon weaponInfo;

    void Update()
    {
        if (WeaponUpgradeLootTable.Instance == null) return;
        if (weaponInfo == null)
        {
            weaponInfo = WeaponUpgradeLootTable.Instance.GetWeapon(WeaponUpgradeLootTable.WeaponType.Poison);
            return;
        }

        switch (weaponInfo.Tier)
        {
            case WeaponUpgradeLootTable.WeaponTier.Tier1:
                colliderSize.radius = 6f;
                VFX.localScale = Vector3.one * .3f;
                damage = tier1Damage;
                break;
            case WeaponUpgradeLootTable.WeaponTier.Tier2:
                colliderSize.radius = 8f;
                VFX.localScale = Vector3.one * .4f;
                damage = tier2Damage;
                break;
            case WeaponUpgradeLootTable.WeaponTier.Tier3:
                colliderSize.radius = 10f;
                VFX.localScale = Vector3.one * .55f;
                damage = tier3Damage;
                break;
            default:
                damage = tier1Damage;
                Debug.LogError("Weapon tier not found");
                break;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        DealDamage(collision);
    }

    private void DealDamage(Collider2D collision)
    {
        var damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            if (damageable.PlayerType == targetType)
            {
                damageable.TakeDamage(damage * Time.deltaTime);
                PoisonDamagetaken?.Invoke(transform.position);
            }
        }
    }
}

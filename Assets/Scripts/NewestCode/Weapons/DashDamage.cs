using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashDamage : MonoBehaviour
{
    public static Action<Vector3> DashDamagetaken;
    [Header("Settings")]
    [SerializeField] CircleCollider2D colliderSize;
    [SerializeField] TrailRenderer VFX;
    public PlayerType targetType;
    [SerializeField] float tier1Damage = 5f;
    [SerializeField] float tier2Damage = 10f;
    [SerializeField] float tier3Damage = 20f;
    private float damage = 0f;
    private Weapon weaponInfo;

    // Update is called once per frame
    void Update()
    {
        if (WeaponUpgradeLootTable.Instance == null) return;
        if (weaponInfo == null)
        {
            weaponInfo = WeaponUpgradeLootTable.Instance.GetWeapon(WeaponUpgradeLootTable.WeaponType.Dash);
            return;
        }

        switch (weaponInfo.Tier)
        {
            case WeaponUpgradeLootTable.WeaponTier.Tier1:
                colliderSize.radius = 2f;
                VFX.widthMultiplier = 1f;
                damage = tier1Damage;
                break;
            case WeaponUpgradeLootTable.WeaponTier.Tier2:
                colliderSize.radius = 4f;
                VFX.widthMultiplier = 1.5f;
                damage = tier2Damage;
                break;
            case WeaponUpgradeLootTable.WeaponTier.Tier3:
                colliderSize.radius = 6f;
                VFX.widthMultiplier = 2f;
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
                DashDamagetaken?.Invoke(transform.position);
            }
        }
    }
}

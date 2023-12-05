using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeWeapon : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float tier1FireRate = 1f;
    [SerializeField] float tier2FireRate = .5f;
    [SerializeField] float tier3FireRate = .2f;
    [SerializeField] GameObject rockPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float rockSpeed = 10f;
    [SerializeField] Transform firePointAnchor;
    private Weapon weaponInfo;
    private float fireCooldown = 0f;
    private float fireRate = 0f;
    
    void Update()
    {
        if (WeaponUpgradeLootTable.Instance == null) return;
        if (weaponInfo == null)
        {
            weaponInfo = WeaponUpgradeLootTable.Instance.GetWeapon(WeaponUpgradeLootTable.WeaponType.Knife);
            return;
        } 

        fireCooldown += Time.deltaTime;
        switch (weaponInfo.Tier)
        {
            case WeaponUpgradeLootTable.WeaponTier.Tier1:
                fireRate = tier1FireRate;
                break;
            case WeaponUpgradeLootTable.WeaponTier.Tier2:
                fireRate = tier2FireRate;
                break;
            case WeaponUpgradeLootTable.WeaponTier.Tier3:
                fireRate = tier2FireRate;
                break;
            default:
                fireRate = tier1FireRate;
                Debug.LogError("Weapon tier not found");
                break;
        }

        if (fireCooldown >= fireRate)
        {
            Aim();
            fireCooldown = 0f;
            var rock = Instantiate(rockPrefab, firePoint.position, Quaternion.identity);
            rock.GetComponent<Rigidbody2D>().AddForce(firePointAnchor.forward * rockSpeed);
            if (weaponInfo.Tier == WeaponUpgradeLootTable.WeaponTier.Tier3)
                rock.transform.localScale *= 3f;
            Destroy(rock, 3f);
        }
    }

    private void Aim()
    {
        Vector3 mousePosition = CamController.Instance.GetCursorWorldPosition();

        // Calculate the direction from the object to the mouse position
        Vector2 direction = new Vector3(mousePosition.x, mousePosition.y, firePointAnchor.position.z) - firePointAnchor.position;

        // Apply the rotation around the Z-axis
        firePointAnchor.rotation = Quaternion.LookRotation(direction);
    }
}

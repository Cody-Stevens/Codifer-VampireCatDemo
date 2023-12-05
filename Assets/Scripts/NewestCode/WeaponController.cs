using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] GameObject RockWeapon;
    [SerializeField] GameObject PoisonWeapon;
    [SerializeField] GameObject LightningWeapon;
    [SerializeField] GameObject DashWeapon;
    [SerializeField] GameObject JumpWeapon;

    private PlayerMovement playerMovement;
    private WeaponUpgradeLootTable weaponLootTable;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement is null in weapon controller");
        }
        weaponLootTable = WeaponUpgradeLootTable.Instance;
        if (weaponLootTable == null)
        {
            Debug.LogError("WeaponLootTable is null in weapon controller");
        }
        //playerMovement.OnJumpStarted += OnJumpStarted;
        //playerMovement.OnJumpEnded += OnJumpEnded;
        playerMovement.OnDashStarted += OnDashStarted;
        playerMovement.OnDashEnded += OnDashEnded;
        weaponLootTable.OnWeaponUpgrade += OnWeaponUpgrade;
    }

    private void OnWeaponUpgrade(Weapon weapon)
    {
        switch (weapon.WeaponType)
        {
            case WeaponUpgradeLootTable.WeaponType.Knife:
                RockWeapon.SetActive(true);
                break;
            case WeaponUpgradeLootTable.WeaponType.Poison:
                PoisonWeapon.SetActive(true);
                break;
            default:
                break;
        }
    }

    private void OnDashEnded()
    {
        if (WeaponUpgradeLootTable.Weapons[(int)WeaponUpgradeLootTable.WeaponType.Dash] != null)
        {
            DashWeapon.SetActive(false);
        }
    }

    private void OnDashStarted()
    {
        if (WeaponUpgradeLootTable.Weapons[(int)WeaponUpgradeLootTable.WeaponType.Dash] != null)
        {
            DashWeapon.SetActive(true);
        }
    }

    //private void OnJumpEnded()
    //{
    //    if (WeaponUpgradeLootTable.WeaponTierDictionary.ContainsKey(WeaponUpgradeLootTable.WeaponType.Jump))
    //    {
    //        DashWeapon.SetActive(true);
    //    }
    //}

    //private void OnJumpStarted()
    //{
    //    // nothing for now
    //}

}

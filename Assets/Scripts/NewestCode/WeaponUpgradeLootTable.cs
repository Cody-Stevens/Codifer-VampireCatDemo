using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUpgradeLootTable : MonoBehaviour
{
    [SerializeField] GameObject PickupPrefab;
    [SerializeField] GameObject RockObject;
    [SerializeField] GameObject DashObject;
    [SerializeField] GameObject PoisonObject;
    [SerializeField] float minTimeBetweenUpgrades = 8f;
    [SerializeField] float pickupDelay = 1f;
    [SerializeField] int chanceToSpawnUpgradeOutOf100 = 4;
    [SerializeField] Image[] WeaponImages;
    [SerializeField] Sprite Dash;
    [SerializeField] Sprite Projectile;
    [SerializeField] Sprite Poison;
    [SerializeField] Weapon[] WeaponsDebug;
    [SerializeField] Weapon[] AvailableWeaponsDebug;

    public enum WeaponType { Knife, Poison, Dash }//Lightning, Dash, Jump }
    public enum WeaponTier { Tier1, Tier2, Tier3 }

    public static List<Weapon> Weapons;
    public static List<Weapon> AvailableWeapons;
    
    //public static Dictionary<WeaponType, WeaponTier> WeaponTierDictionary = new Dictionary<WeaponType, WeaponTier>();
    
    public static bool CanUpgrade = false;
    public static bool upgradeAvailable = false;
    public static int WeaponUpgradeCount;
    public int WeaponUpgradeCap;

    public static WeaponUpgradeLootTable Instance;
    public Action<Weapon> OnWeaponUpgrade;
    private float mercyUpgradeTime;
    private float upgradeCooldown;
    private float pickupCooldown;
    private bool upgradeSpawnNext;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        Weapons = new List<Weapon>();
    }

    private void Start()
    {
        AvailableWeapons= new List<Weapon>();
        for (int i = 0; i < 3; i++)
        { 
            AvailableWeapons.Add(new Weapon((WeaponType)i, (WeaponTier)0));
        }
        SpawnUpgrade(transform.position);
    }
    private void Update()
    {
        if (Weapons != null)
        {
            WeaponsDebug = Weapons.ToArray();
        }
        if (AvailableWeapons != null)
        {
            AvailableWeaponsDebug = AvailableWeapons.ToArray();
        }
        if (pickupCooldown > 0) 
        {
            pickupCooldown -= Time.deltaTime;
        }
        CanUpgrade = pickupCooldown <= 0;
        if ( upgradeAvailable) return;
        if (WeaponUpgradeCount >= WeaponUpgradeCap) return;
        if (upgradeCooldown > 0)
        {
            upgradeCooldown -= Time.deltaTime;
            return;
        }
        if (upgradeSpawnNext) return;
        
        if (mercyUpgradeTime > 0)
        {
            mercyUpgradeTime -= Time.deltaTime;
        }
        else
        {
            mercyUpgradeTime = GameManager.Instance.mercyUpgradeTime;
            upgradeSpawnNext = true;
        }
    }

    public bool ShouldSpawnUpgrade()
    {
        if (upgradeSpawnNext)
        {
            upgradeSpawnNext = false;
            return true;
        }
        return UnityEngine.Random.Range(0, 100) < chanceToSpawnUpgradeOutOf100;
    }

    public void SpawnUpgrade(Vector3 position)
    {
        if (upgradeAvailable) return;
        if (WeaponUpgradeCount >= WeaponUpgradeCap) return;
        upgradeAvailable = true;
        pickupCooldown = pickupDelay;
        var weapon = GetRandomWeaponFromLootTable();

        GameObject o = Instantiate(PickupPrefab, position, Quaternion.identity);
        var pickup = o.GetComponent<WeaponPickup>();
        pickup.Initialize(weapon.WeaponType, weapon.Tier);
        Debug.Log($"Spawned upgrade, {weapon.WeaponType.ToString()} {weapon.Tier.ToString()}");
    }

    public Weapon GetWeapon(WeaponType weaponType)
    {
        for (int i = 0; i < Weapons.Count; i++)
        {
            if (Weapons[i] == null) continue;
            if (Weapons[i].WeaponType == weaponType)
            {
                return Weapons[i];
            }
        }
        return null;
    }

    public Weapon GetRandomWeaponFromLootTable()
    {
        var wepIndex = UnityEngine.Random.Range(0, AvailableWeapons.Count);
        var newWeapon = new Weapon(AvailableWeapons[wepIndex].WeaponType, AvailableWeapons[wepIndex].Tier);
        if (AvailableWeapons[wepIndex].Tier == WeaponTier.Tier3)
        {
            AvailableWeapons.RemoveAt(wepIndex);
        }
        else
        {
            AvailableWeapons[wepIndex].Tier++;
        }
        return newWeapon;
    }

    public void AddWeapon(Weapon newWeapon)
    {
        Debug.Log("Adding weapon, " + newWeapon.WeaponType.ToString());
        if (WeaponUpgradeCount >= WeaponUpgradeCap) return;
        
        if (newWeapon == null)
        {
            Debug.LogError("Weapon is null"); 
            return;
        }

        WeaponUpgradeCount++;
        upgradeAvailable = false;
        upgradeCooldown = minTimeBetweenUpgrades;

        // Upgrade tier or add weapon
        bool found = false;
        for (int i = 0; i < Weapons.Count; i++)
        {
            if (Weapons[i].WeaponType == newWeapon.WeaponType)
            {
                Weapons[i].Tier = newWeapon.Tier;
                found = true;
                break;
            }   
        }
        if (!found)
        {
           Weapons.Add(new Weapon(newWeapon.WeaponType, newWeapon.Tier));
        }

        // Enable weapon
        if (newWeapon.WeaponType == WeaponType.Knife)
        {
            RockObject.SetActive(true);
        }
        if (newWeapon.WeaponType == WeaponType.Dash)
        {
            if (newWeapon.Tier == 0)
            {
                transform.root.gameObject.GetComponent<PlayerMovement>().OnDashStarted += () => { DashObject.SetActive(true); };
                transform.root.gameObject.GetComponent<PlayerMovement>().OnDashEnded += () => { DashObject.SetActive(false); };
            }
        }
        if (newWeapon.WeaponType == WeaponType.Poison)
        {
            PoisonObject.SetActive(true);
        }
        OnWeaponUpgrade?.Invoke(newWeapon);

        // Update UI
        WeaponImages[WeaponUpgradeCount - 1].sprite = TextureType(newWeapon.WeaponType);
        WeaponImages[WeaponUpgradeCount - 1].color = Color.white;
    }

    private Sprite TextureType(WeaponType weaponType)
    {
        switch(weaponType)
        {
            case WeaponType.Knife:
                return Projectile;
            case WeaponType.Poison:
                return Poison;
            case WeaponType.Dash:
                return Dash;
            default:
                return null;
        }
    }
}
[Serializable]
public class Weapon
{
    public WeaponUpgradeLootTable.WeaponType WeaponType;
    public WeaponUpgradeLootTable.WeaponTier Tier;

    public Weapon(WeaponUpgradeLootTable.WeaponType weaponType, WeaponUpgradeLootTable.WeaponTier tier)
    {
        WeaponType = weaponType;
        Tier = tier;
    }
}

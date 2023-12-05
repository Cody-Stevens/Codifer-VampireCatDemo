using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public WeaponUpgradeLootTable.WeaponType weaponType;
    public WeaponUpgradeLootTable.WeaponTier weaponTier;

    [SerializeField] GameObject OnPickupVFX;
    [SerializeField] AudioSource OnPickupSFX;
    [SerializeField] Sprite Dash;
    [SerializeField] Sprite Projectile;
    [SerializeField] Sprite Poison;
    private bool initialized = false;

    public void Initialize(WeaponUpgradeLootTable.WeaponType weaponType, WeaponUpgradeLootTable.WeaponTier weaponTier)
    {
        this.weaponType = weaponType;
        this.weaponTier = weaponTier; 
        var mat = transform.GetChild(0).GetComponent<MeshRenderer>().material;
        mat.mainTexture = TextureType().texture;
        initialized = true;
    }

    private Sprite TextureType() 
    {
        switch (weaponType)
        {
            case WeaponUpgradeLootTable.WeaponType.Knife:
                return Projectile;
            case WeaponUpgradeLootTable.WeaponType.Poison:
                return Poison;
            case WeaponUpgradeLootTable.WeaponType.Dash:
                return Dash;
            default:
                break;
        }
        return null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Upgrade(other);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Upgrade(collision);
    }

    void Upgrade(Collider2D other)
    {
        if (!initialized) return;
        if (!WeaponUpgradeLootTable.CanUpgrade) return;

        var WeaponController = other.GetComponent<WeaponUpgradeLootTable>();
        if (WeaponController != null)
        {
            var weapon = new Weapon(weaponType, weaponTier);
            WeaponController.AddWeapon(weapon);
            OnPickupVFX.SetActive(true);
            Destroy(gameObject);
        }
    }
}

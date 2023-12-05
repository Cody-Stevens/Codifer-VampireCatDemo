using UnityEngine;

[CreateAssetMenu(menuName = "WeaponUpgrade", fileName = "default.WeaponUpgrade.asset")]
public class WeaponUpgrade : ScriptableObject
{
    public Sprite Icon;
    public WeaponUpgradeLootTable.WeaponType WeaponType;
    public WeaponUpgradeLootTable.WeaponTier Tier;
}

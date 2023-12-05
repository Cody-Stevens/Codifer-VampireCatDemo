using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PurchaseableItem : MonoBehaviour
{
    public int price;
    public Sprite icon;
    public Food food;
    public GameObject item;
    public Image iconI;
    public TextMeshProUGUI descT, priceT;
    public ItemType type;
    public int value = 2;
    private void Start()
    {
        Initialize();
        
    }


    private void Initialize()
    {
        priceT.text = price.ToString() ;
        iconI.sprite = icon;
        
    }
    public void PurchaseItem()
    {
        if(CityManager.townStats.coins>= price)
        {
            CityManager.ModifyMoney(-price);

            HandleItemPurchase();
        }

    }

    private void HandleItemPurchase()
    {
        switch (type)
        {
            case ItemType.townItem:
                CreateItem();
                TownMenu.Instance.Close();
                break;
            case ItemType.mouseStatIncrease:
                Hero.damageValue += value;
                CityManager.Instance.HandleStats();

                break;
            case ItemType.pigHealthUp:
                UnitController.GetPigs();
                for(int i = 0; i < UnitController.pigs.Count; i++)
                {
                    //UnitController.pigs[i].stats.maxHealth += value;
                    //UnitController.pigs[i].stats.health += value;
                    //UnitController.pigs[i].stats.UpdateStats();
                }

                break;
            case ItemType.pigStrengthUp:
                UnitController.GetPigs();
                for (int i = 0; i < UnitController.pigs.Count; i++)
                {
                    //UnitController.pigs[i].stats.strength += value;
                    //UnitController.pigs[i].stats.UpdateStats();
                }

                break;
            case ItemType.pigSpeedUp:
                UnitController.GetPigs();
                for (int i = 0; i < UnitController.pigs.Count; i++)
                {
                    //UnitController.pigs[i].stats.speed += value;
                    //UnitController.pigs[i].stats.UpdateStats();
                }
                break;
        }

    }
    private void CreateItem()
    {
      Vector3  spawnPos = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);
      GameObject created=  Instantiate(item, CityManager.Instance.items.transform);
        created.transform.localPosition = spawnPos;
    }


}

public enum ItemType
{
    townItem,
    mouseStatIncrease,
    pigHealthUp,
    pigStrengthUp,
    pigSpeedUp

}

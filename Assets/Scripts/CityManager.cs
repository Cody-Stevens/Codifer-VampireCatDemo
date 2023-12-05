using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CityManager : MonoBehaviour
{
    public static CityManager Instance;
    public static TownStats townStats;
    public List<FamilyMember> allMembers;
    public GameObject city, friendly, foodObjects, items,enemies;
    public static float timeMultiplier = 3;
    
    public static float timeToGrow = 3;
    [SerializeField] TextMeshProUGUI stats, coins;
    
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }


    private void Start()
    {
        townStats = new TownStats();
        HandleStats();
    }
    public   void HandleStats()
    {
        townStats.citizens = friendly.transform.childCount;
        stats.text = "Friendly: " + townStats.citizens + 
              "\nMouse Damage: " + Hero.damageValue;


        Instance.coins.text = townStats.coins.ToString();
        if (townStats.citizens <= 0)
        {
            stats.text = "Game Over";
        }
    }


    public void ToggleTimeMult(bool state)
    {
      
            timeMultiplier = state? timeMultiplier + timeMultiplier * 0.1f:
            timeMultiplier - timeMultiplier * 0.1f ;
      

    }


   

   public static void ModifyMoney(int amount)
    {
        townStats.coins += amount;
        Instance.coins.text = townStats.coins.ToString(); 
    }




}

public class TownStats
{
    public int coins = 50;
    public int citizens = 3;
}

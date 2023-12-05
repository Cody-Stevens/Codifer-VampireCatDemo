using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownMenu : MonoBehaviour
{
    public static TownMenu Instance;
    public GameObject content;
    public List<PurchaseableItem> items;
    public GameObject purchasableItem;

    private void Start()
    {
        if (Instance == null) Instance = this;
    }
    public void Open()
    {
        if (Instance == null) Instance = this;
        gameObject.SetActive(true);
    
    }

   
    public void Close()
    {
        gameObject.SetActive(false);
    }
}

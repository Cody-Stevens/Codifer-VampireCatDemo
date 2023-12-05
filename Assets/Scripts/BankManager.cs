using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BankManager : MonoBehaviour
{
    public static BankManager Instance;

    public int Amount;

    public static Action BankUpdated;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        BankUpdated?.Invoke();
    }

    public bool CanAfford(int money)
    {
        return money <= Amount;
    }

    public void SpendMoney(int amount)
    {
        Amount -= amount;
        BankUpdated?.Invoke();
    }

    public void EarnMoney(int amount)
    {
        Amount += amount;
        BankUpdated?.Invoke();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    public GameObject DayUI;
    public GameObject NightUI;
    public TMP_Text DayNumberText;
    public TMP_Text MoneyText;

    private void Awake()
    {
        GameEvents.WaveFinished += ToDay;
    }

    private void Start()
    {
        BankManager.BankUpdated += ChangeMoneyAmount;
    }

    private void OnDestroy()
    {
        GameEvents.WaveFinished -= ToDay;
    }

    public void ToNight()
    {
        DayUI.SetActive(false);
        NightUI.SetActive(true);

        GameEvents.SendWaveStarted(DayManager.Day);
    }

    public void ToDay(int day)
    {
        DayUI.SetActive(true);
        NightUI.SetActive(false);

        DayNumberText.text = "Day " + day.ToString();
    }

    public void ChangeMoneyAmount()
    {
        MoneyText.text = BankManager.Instance.Amount.ToString();
    }
}

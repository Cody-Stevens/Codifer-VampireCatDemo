using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeButton : MonoBehaviour
{
    public TMP_Text CostText;
    public Button Button;

    private BaseBuilding m_building;

    private void Awake()
    {
        m_building = GetComponentInParent<BaseBuilding>();

        GameEvents.WaveFinished += OnWaveFinished;
        GameEvents.WaveStarted += OnWaveStarted;
        BankManager.BankUpdated += OnBankUpdated;
    }

    private void OnDestroy()
    {
        GameEvents.WaveFinished -= OnWaveFinished;
        GameEvents.WaveStarted -= OnWaveStarted;
        BankManager.BankUpdated -= OnBankUpdated;
    }

    private void OnWaveFinished(int day)
    {
        gameObject.SetActive(true);
    }

    private void OnWaveStarted(int day)
    {
        gameObject.SetActive(false);
    }

    private void OnBankUpdated()
    {
        Button.interactable = CanBeUpgraded();
        CostText.text = m_building.CostToUpgrade.ToString();
    }

    private bool CanBeUpgraded()
    {
        return m_building.Level < m_building.MaxLevel && BankManager.Instance.CanAfford(m_building.CostToUpgrade);
    }
}

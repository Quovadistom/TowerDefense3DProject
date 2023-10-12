using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AvailableTileUpgradeButton : MonoBehaviour
{
    [SerializeField] private TMP_Text m_text;
    [SerializeField] private TMP_Text m_boostAmountText;
    [SerializeField] private Button m_button;
    [SerializeField] private GameObject m_upgradeAppliedCheck;

    private BoostContainer m_connectedBoost;
    private int m_boostAmount;
    private TownHousingService m_townHousingService;
    private Guid m_id;
    private int m_connectedLocation;

    [Inject]
    private void Construct(Guid id, TownHousingService townHousingService)
    {
        m_id = id;
        m_townHousingService = townHousingService;
    }

    private void Awake()
    {
        m_townHousingService.TileUpgradeApplied += OnTileUpgradeApplied;
    }

    private void OnDestroy()
    {
        m_button.onClick.RemoveAllListeners();
        m_townHousingService.TileUpgradeApplied -= OnTileUpgradeApplied;
    }

    private void OnTileUpgradeApplied(HousingData housingData, int location)
    {
        m_upgradeAppliedCheck.SetActive(housingData.ActiveUpgrades[location] == m_connectedBoost.ID);
    }

    public void SetButtonInfo(BoostContainer connectedBoost, int boostAmount, int connectedLocation)
    {
        m_connectedBoost = connectedBoost;
        m_boostAmount = boostAmount;
        m_text.text = m_connectedBoost.Name;
        m_connectedLocation = connectedLocation;

        m_button.onClick.AddListener(OnButtonClicked);

        m_boostAmountText.text = boostAmount.ToString();

        m_upgradeAppliedCheck.SetActive(m_townHousingService.GetHousingData(m_id).ActiveUpgrades[m_connectedLocation] == m_connectedBoost.ID);
    }

    private void OnButtonClicked()
    {
        m_townHousingService.UpgradeTile(m_id, m_connectedBoost, m_connectedLocation);
    }

    public class Factory : PlaceholderFactory<Guid, AvailableTileUpgradeButton> { }
}

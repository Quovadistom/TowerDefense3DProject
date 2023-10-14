using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AvailableTileUpgradeButton : MonoBehaviour
{
    [SerializeField] private TMP_Text m_text;
    [SerializeField] private TMP_Text m_enhancementAmountText;
    [SerializeField] private Button m_button;
    [SerializeField] private GameObject m_upgradeAppliedCheck;

    private EnhancementContainer m_connectedEnhancement;
    private int m_enhancementAmount;
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
        m_upgradeAppliedCheck.SetActive(housingData.ActiveUpgrades[location] == m_connectedEnhancement);
    }

    public void SetButtonInfo(EnhancementContainer connectedEnhancement, int enhancementAmount, int connectedLocation)
    {
        m_connectedEnhancement = connectedEnhancement;
        m_enhancementAmount = enhancementAmount;
        m_text.text = m_connectedEnhancement.Name;
        m_connectedLocation = connectedLocation;

        m_button.onClick.AddListener(OnButtonClicked);

        m_enhancementAmountText.text = enhancementAmount.ToString();

        m_upgradeAppliedCheck.SetActive(m_townHousingService.GetHousingData(m_id).ActiveUpgrades[m_connectedLocation] == m_connectedEnhancement);
    }

    private void OnButtonClicked()
    {
        m_townHousingService.UpgradeTile(m_id, m_connectedEnhancement, m_connectedLocation);
    }

    public class Factory : PlaceholderFactory<Guid, AvailableTileUpgradeButton> { }
}

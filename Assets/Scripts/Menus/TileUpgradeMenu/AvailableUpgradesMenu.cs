using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class AvailableUpgradesMenu : MonoBehaviour
{
    [SerializeField] private AvailableTileUpgradeButton m_availableTileUpgradeButton;
    [SerializeField] private RectTransform m_buttonParent;

    private BoostAvailabilityService m_boostAvailabilityService;
    private TurretCollection m_towerCollection;
    private AvailableTileUpgradeButton.Factory m_buttonFactory;

    [Inject]
    private void Construct(BoostAvailabilityService boostAvailabilityService, TurretCollection towerCollection, AvailableTileUpgradeButton.Factory buttonFactory)
    {
        m_boostAvailabilityService = boostAvailabilityService;
        m_towerCollection = towerCollection;
        m_buttonFactory = buttonFactory;
    }

    public void SetUpgrades(Guid m_selectedTileGuid, int buttonIndex)
    {
        m_buttonParent.ClearChildren();

        if (m_towerCollection.TryGetAssets(m_selectedTileGuid, out TowerAssets towerAssets))
        {
            Dictionary<BoostContainer, int> availableBoosts = m_boostAvailabilityService.GetBoostsForComponentParent(towerAssets.TowerPrefab, BoostType.TowerBoost);

            for (int i = 0; i < availableBoosts.Count(); i++)
            {
                AvailableTileUpgradeButton availableTileUpgradeButton = m_buttonFactory.Create(m_selectedTileGuid);
                availableTileUpgradeButton.transform.SetParent(m_buttonParent, false);
                availableTileUpgradeButton.SetButtonInfo(availableBoosts.ElementAt(i).Key, availableBoosts.ElementAt(i).Value, buttonIndex);
            }
        }
    }
}

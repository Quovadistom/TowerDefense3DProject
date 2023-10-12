using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class AvailableUpgradesMenu : MonoBehaviour
{
    [SerializeField] private AvailableTileUpgradeButton m_availableTileUpgradeButton;
    [SerializeField] private RectTransform m_buttonParent;

    private EnhancementAvailabilityService m_enhancementAvailabilityService;
    private TurretCollection m_towerCollection;
    private AvailableTileUpgradeButton.Factory m_buttonFactory;

    [Inject]
    private void Construct(EnhancementAvailabilityService enhancementAvailabilityService, TurretCollection towerCollection, AvailableTileUpgradeButton.Factory buttonFactory)
    {
        m_enhancementAvailabilityService = enhancementAvailabilityService;
        m_towerCollection = towerCollection;
        m_buttonFactory = buttonFactory;
    }

    public void SetUpgrades(Guid m_selectedTileGuid, int buttonIndex)
    {
        m_buttonParent.ClearChildren();

        if (m_towerCollection.TryGetAssets(m_selectedTileGuid, out TowerAssets towerAssets))
        {
            Dictionary<EnhancementContainer, int> availableEnhancements = m_enhancementAvailabilityService.GetEnhancementsForComponentParent(towerAssets.TowerPrefab, EnhancementType.TowerEnhancement);

            for (int i = 0; i < availableEnhancements.Count(); i++)
            {
                AvailableTileUpgradeButton availableTileUpgradeButton = m_buttonFactory.Create(m_selectedTileGuid);
                availableTileUpgradeButton.transform.SetParent(m_buttonParent, false);
                availableTileUpgradeButton.SetButtonInfo(availableEnhancements.ElementAt(i).Key, availableEnhancements.ElementAt(i).Value, buttonIndex);
            }
        }
    }
}

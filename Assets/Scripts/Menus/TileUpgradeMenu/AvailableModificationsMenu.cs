using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class AvailableModificationsMenu : MonoBehaviour
{
    [SerializeField] private AvailableTileModificationButton m_availableTileModificationButton;
    [SerializeField] private RectTransform m_buttonParent;

    private ModificationAvailabilityService m_modificationAvailabilityService;
    private TurretCollection m_towerCollection;
    private AvailableTileModificationButton.Factory m_buttonFactory;

    [Inject]
    private void Construct(ModificationAvailabilityService modificationAvailabilityService, TurretCollection towerCollection, AvailableTileModificationButton.Factory buttonFactory)
    {
        m_modificationAvailabilityService = modificationAvailabilityService;
        m_towerCollection = towerCollection;
        m_buttonFactory = buttonFactory;
    }

    public void SetModifications(Guid m_selectedTileGuid, int buttonIndex)
    {
        m_buttonParent.ClearChildren();

        if (m_towerCollection.TryGetAssets(m_selectedTileGuid, out TowerAssets towerAssets))
        {
            Dictionary<ModificationContainer, int> availableModifications = m_modificationAvailabilityService.GetModificationsForComponentParent(towerAssets.TowerPrefab, ModificationType.TowerModification);

            for (int i = 0; i < availableModifications.Count(); i++)
            {
                AvailableTileModificationButton availableTileModificationButton = m_buttonFactory.Create(m_selectedTileGuid);
                availableTileModificationButton.transform.SetParent(m_buttonParent, false);
                availableTileModificationButton.SetButtonInfo(availableModifications.ElementAt(i).Key, availableModifications.ElementAt(i).Value, buttonIndex);
            }
        }
    }
}

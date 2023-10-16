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
    private TownHousingService m_townHousingService;
    private TurretCollection m_towerCollection;
    private AvailableTileModificationButton.Factory m_buttonFactory;

    [Inject]
    private void Construct(ModificationAvailabilityService modificationAvailabilityService, TownHousingService townHousingService, TurretCollection towerCollection, AvailableTileModificationButton.Factory buttonFactory)
    {
        m_modificationAvailabilityService = modificationAvailabilityService;
        m_townHousingService = townHousingService;
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
                KeyValuePair<ModificationContainer, int> modification = availableModifications.ElementAt(i);
                var currentModification = m_townHousingService.GetHousingData(m_selectedTileGuid).ActiveModifications[buttonIndex];

                if (modification.Value != 0 || (currentModification != null && currentModification.ID == modification.Key.ID))
                {
                    AvailableTileModificationButton availableTileModificationButton = m_buttonFactory.Create(m_selectedTileGuid);
                    availableTileModificationButton.transform.SetParent(m_buttonParent, false);
                    availableTileModificationButton.SetButtonInfo(modification.Key, modification.Value, buttonIndex);
                }
            }
        }
    }
}

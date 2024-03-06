using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class AvailableModificationsMenu : MonoBehaviour
{
    [SerializeField] private AvailableTileModificationButton m_availableTileModificationButton;
    [SerializeField] private RectTransform m_buttonParent;

    private BlueprintService m_blueprintService;
    private TurretCollection m_towerCollection;
    private AvailableTileModificationButton.Factory m_buttonFactory;

    [Inject]
    private void Construct(BlueprintService blueprintService, TurretCollection towerCollection, AvailableTileModificationButton.Factory buttonFactory)
    {
        m_blueprintService = blueprintService;
        m_towerCollection = towerCollection;
        m_buttonFactory = buttonFactory;
    }

    public void SetModifications(Guid m_selectedTileGuid, int buttonIndex)
    {
        m_buttonParent.ClearChildren();

        if (m_towerCollection.TryGetAssets(m_selectedTileGuid, out TowerAssets towerAssets))
        {
            IEnumerable<Blueprint> availableModifications = m_blueprintService.GetSuitableBlueprints(towerAssets.TowerPrefab, BlueprintType.Tower);

            foreach (Blueprint blueprint in availableModifications)
            {
                AvailableTileModificationButton availableTileModificationButton = m_buttonFactory.Create(m_selectedTileGuid);
                availableTileModificationButton.transform.SetParent(m_buttonParent, false);
                availableTileModificationButton.SetButtonInfo(blueprint, buttonIndex);
            }
        }
    }
}

using System;
using UnityEngine;
using Zenject;

public class TileUpgradeMenuItem : MonoBehaviour
{
    [SerializeField] private RectTransform m_content;
    [SerializeField] private Transform m_tile;

    public Guid TileGuid = Guid.Empty;

    private TowerAvailabilityService m_towerAvailabilityService;
    private TowerTileVisual.Factory m_townTileVisualFactory;

    [Inject]
    private void Construct(TowerAvailabilityService towerAvailabilityService, TowerTileVisual.Factory townTileVisualFactory)
    {
        m_towerAvailabilityService = towerAvailabilityService;
        m_townTileVisualFactory = townTileVisualFactory;
    }

    public void SetHousingInfo(HousingData housingData)
    {
        TileGuid = housingData.TowerTypeGuid;

        if (m_towerAvailabilityService.TryGetTowerAssets(housingData.TowerTypeGuid, out TowerAssets towerAssets))
        {
            TowerTileVisual visual = m_townTileVisualFactory.Create(towerAssets.HousingPrefab, towerAssets.ID);
            visual.transform.SetParent(m_tile, false);
        }
    }

    public class Factory : PlaceholderFactory<TileUpgradeMenuItem> { }
}

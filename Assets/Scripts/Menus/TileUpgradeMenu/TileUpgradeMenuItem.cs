using UnityEngine;
using Zenject;

public class TileUpgradeMenuItem : MonoBehaviour
{
    [SerializeField] private RectTransform m_content;
    [SerializeField] private Transform m_tile;

    private HousingData m_housingData;

    private RectTransform m_parent;
    private TowerAvailabilityService m_towerAvailabilityService;
    private TownTileVisual.Factory m_townTileVisualFactory;

    [Inject]
    private void Construct(TowerAvailabilityService towerAvailabilityService, TownTileVisual.Factory townTileVisualFactory)
    {
        m_towerAvailabilityService = towerAvailabilityService;
        m_townTileVisualFactory = townTileVisualFactory;
    }

    public void SetHousingInfo(HousingData housingData)
    {
        m_parent = transform.parent.GetComponent<RectTransform>();
        m_housingData = housingData;

        if (m_towerAvailabilityService.TryGetTowerAssets(housingData.TowerTypeGuid, out TowerAssets towerAssets))
        {
            TownTileVisual visual = m_townTileVisualFactory.Create(towerAssets.HousingPrefab);
            visual.transform.SetParent(m_tile, false);
        }
    }

    public class Factory : PlaceholderFactory<TileUpgradeMenuItem> { }
}

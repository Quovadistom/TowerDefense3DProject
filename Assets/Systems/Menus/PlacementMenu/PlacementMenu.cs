using UnityEngine;
using Zenject;

public class PlacementMenu : MonoBehaviour
{
    [SerializeField] private PlacementItemButton m_buttonPrefab;
    [SerializeField] private Transform m_buttonParent;
    [SerializeField] private MenuController m_menuController;
    [SerializeField] private MenuPage m_menuPage;

    private TowerAvailabilityService m_towerAvailabilityService;
    private TownTileService m_townTileService;
    private PlacementItemButton.Factory m_buttonFactory;

    [Inject]
    private void Construct(TowerAvailabilityService towerAvailabilityService, TownTileService townTileService, PlacementItemButton.Factory factory)
    {
        m_towerAvailabilityService = towerAvailabilityService;
        m_townTileService = townTileService;
        m_buttonFactory = factory;
    }

    private void Awake()
    {
        m_townTileService.TileSelected += OnTileSelected;
    }

    private void OnDestroy()
    {
        m_townTileService.TileSelected -= OnTileSelected;
    }

    private void OnTileSelected(TownTile tile)
    {
        if (tile.IsCaptured && !tile.IsOccupied)
        {
            m_buttonParent.ClearChildren();
            m_menuController.PushMenuPage(m_menuPage);

            foreach (TowerAssets turretAssets in m_towerAvailabilityService.AvailableTowers)
            {
                PlacementItemButton placementItemButton = m_buttonFactory.Create();
                placementItemButton.transform.SetParent(m_buttonParent, false);
                placementItemButton.InitializeButton(tile, turretAssets, m_menuController);
            }
        }
    }
}

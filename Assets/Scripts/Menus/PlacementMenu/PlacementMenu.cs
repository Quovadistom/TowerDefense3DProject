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

    [Inject]
    private void Construct(TowerAvailabilityService towerAvailabilityService, TownTileService townTileService)
    {
        m_towerAvailabilityService = towerAvailabilityService;
        m_townTileService = townTileService;
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
                PlacementItemButton placementItemButton = Instantiate(m_buttonPrefab, m_buttonParent);
                placementItemButton.InitializeButton(turretAssets.TowerPrefab.GetType().ToString(),
                    () =>
                    {
                        tile.UpdateTile(turretAssets);
                        m_menuController.PopMenuPage();
                    });
            }
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayMenu : MonoBehaviour
{
    [SerializeField] private MenuController m_menuController;
    [SerializeField] private MenuPage m_menuPage;
    [SerializeField] private Button m_startButton;

    private TownTileService m_tileService;

    [Inject]
    private void Construct(TownTileService tileService)
    {
        m_tileService = tileService;
    }

    private void Awake()
    {
        m_startButton.onClick.AddListener(StartLevel);
        m_tileService.TileSelected += OnTileSelected;
    }

    private void OnDestroy()
    {
        m_startButton.onClick.RemoveAllListeners();
        m_tileService.TileSelected -= OnTileSelected;
    }

    private void StartLevel()
    {
        m_tileService.ActiveTownTile.TownTileData.IsCaptured = true;
        m_menuController.PopMenuPage();
    }

    private void OnTileSelected(TownTile tile)
    {
        if (!tile.TownTileData.IsCaptured)
        {
            m_menuController.PushMenuPage(m_menuPage);
        }
    }
}

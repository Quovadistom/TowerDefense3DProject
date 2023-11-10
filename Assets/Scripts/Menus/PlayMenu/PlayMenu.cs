using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class PlayMenu : MonoBehaviour
{
    [SerializeField] private MenuController m_menuController;
    [SerializeField] private MenuPage m_menuPage;
    [SerializeField] private Button m_startButton;

    private TownTileService m_tileService;
    private SceneCollection m_sceneCollection;
    private LevelService m_levelService;

    [Inject]
    private void Construct(TownTileService tileService, SceneCollection sceneCollection, LevelService levelService)
    {
        m_tileService = tileService;
        m_sceneCollection = sceneCollection;
        m_levelService = levelService;
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
        m_levelService.Map = m_tileService.ActiveTownTile.ConnectedMap;
        m_menuController.PopMenuPage();
        SceneManager.LoadScene(m_sceneCollection.LevelScene);
    }

    private void OnTileSelected(TownTile tile)
    {
        if (!tile.IsCaptured)
        {
            m_menuController.PushMenuPage(m_menuPage);
        }
    }
}

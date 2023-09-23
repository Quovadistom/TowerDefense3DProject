using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TileContextMenu : MonoBehaviour
{
    [SerializeField] private RectTransform m_uiRectTransform;
    [SerializeField] private Button m_deleteButton;
    [SerializeField] private Button m_moveButton;
    [SerializeField] private Button m_upgradeButton;

    private TownTileService m_tileService;
    private RectTransform m_rectTransform;

    [Inject]
    private void Construct(TownTileService tileService)
    {
        m_tileService = tileService;
    }

    private void Awake()
    {
        m_tileService.TileSelected += OnTileSelected;

        m_deleteButton.onClick.AddListener(EmptyTile);
        m_upgradeButton.onClick.AddListener(UpgradeTile);

        m_rectTransform = GetComponent<RectTransform>();

        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            SetMenuToGameObjectPosition(m_tileService.ActiveTownTile);
        }
    }

    private void OnDestroy()
    {
        m_tileService.TileSelected -= OnTileSelected;

        m_deleteButton.onClick.RemoveAllListeners();
        m_moveButton.onClick.RemoveAllListeners();
        m_upgradeButton.onClick.RemoveAllListeners();
    }
    private void EmptyTile()
    {
        m_tileService.ActiveTownTile.SetTileContent(null);
        gameObject.SetActive(false);
    }

    private void UpgradeTile()
    {
        m_tileService.UpgradeActiveTile();
    }

    private void OnTileSelected(TownTile tile)
    {
        if (tile.TownTileData.IsOccupied)
        {
            SetMenuToGameObjectPosition(tile);

            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void SetMenuToGameObjectPosition(TownTile tile)
    {
        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(tile.transform.position);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        (ViewportPosition.x * m_uiRectTransform.sizeDelta.x) - (m_uiRectTransform.sizeDelta.x * 0.5f),
        (ViewportPosition.y * m_uiRectTransform.sizeDelta.y) - (m_uiRectTransform.sizeDelta.y * 0.5f));

        m_rectTransform.anchoredPosition = WorldObject_ScreenPosition;
    }
}

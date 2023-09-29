using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class TileUpgradeMenu : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private MenuController m_menuController;
    [SerializeField] private MenuPage m_menuPage;
    [SerializeField] private TileUpgradeMenuItem m_tileUpgradeMenuItemPrefab;

    [SerializeField] private RectTransform m_container;
    [SerializeField] private float m_speedMultiplier = 0.2f;
    [SerializeField] private float m_dragMultiplier = 1.2f;

    private float m_tileItemWidth;
    private TownHousingService m_townHousingService;
    private TileUpgradeMenuItem.Factory m_tileUpgradeMenuItemFactory;
    private TweenerCore<Vector3, Vector3, VectorOptions> m_dragTask;

    [Inject]
    private void Construct(TownHousingService townHousingService, TileUpgradeMenuItem.Factory tileUpgradeMenuItemFactory)
    {
        m_townHousingService = townHousingService;
        m_tileUpgradeMenuItemFactory = tileUpgradeMenuItemFactory;
    }

    private void Awake()
    {
        m_townHousingService.TileHousingUpgradeRequested += OnTileUpgradeRequested;

        m_tileItemWidth = m_tileUpgradeMenuItemPrefab.GetComponent<RectTransform>().sizeDelta.x;
    }

    private void OnEnable()
    {
        m_container.localPosition = new Vector3(-m_tileItemWidth / 2, 0, 0);

        m_container.ClearChildren();
        foreach (HousingData housingData in m_townHousingService.AvailableHousingData)
        {
            TileUpgradeMenuItem tile = m_tileUpgradeMenuItemFactory.Create();
            tile.transform.SetParent(m_container.transform, false);
            tile.SetHousingInfo(housingData);
        }
        foreach (HousingData housingData in m_townHousingService.AvailableHousingData)
        {
            TileUpgradeMenuItem tile = m_tileUpgradeMenuItemFactory.Create();
            tile.transform.SetParent(m_container.transform, false);
            tile.SetHousingInfo(housingData);
        }
        foreach (HousingData housingData in m_townHousingService.AvailableHousingData)
        {
            TileUpgradeMenuItem tile = m_tileUpgradeMenuItemFactory.Create();
            tile.transform.SetParent(m_container.transform, false);
            tile.SetHousingInfo(housingData);
        }
        foreach (HousingData housingData in m_townHousingService.AvailableHousingData)
        {
            TileUpgradeMenuItem tile = m_tileUpgradeMenuItemFactory.Create();
            tile.transform.SetParent(m_container.transform, false);
            tile.SetHousingInfo(housingData);
        }
    }

    private void OnDestroy()
    {
        m_townHousingService.TileHousingUpgradeRequested -= OnTileUpgradeRequested;
    }

    private void OnTileUpgradeRequested(HousingData obj)
    {
        m_menuController.PushMenuPage(m_menuPage);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_dragTask.Kill();
    }

    public void OnDrag(PointerEventData eventData)
    {
        m_container.localPosition = new Vector3(m_container.localPosition.x + eventData.delta.x * m_dragMultiplier, m_container.localPosition.y, m_container.localPosition.z);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        int goalTile = Mathf.RoundToInt((m_container.localPosition.x + m_tileItemWidth / 2) / -m_tileItemWidth);

        if (Mathf.Abs(eventData.delta.x * 0.2f) > 1f)
        {
            goalTile = Mathf.RoundToInt(goalTile * -eventData.delta.x * 0.05f);
        }


        float travelDistance = -m_tileItemWidth * goalTile - m_tileItemWidth / 2;
        float travelSpeed = Mathf.Clamp(Mathf.Abs(m_container.localPosition.x - travelDistance) / m_tileItemWidth, -5, 5);

        float containerPosition = Mathf.Clamp(travelDistance,
            (-m_tileItemWidth / 2) - (m_container.childCount - 1) * m_tileItemWidth,
            -m_tileItemWidth / 2);

        m_dragTask = m_container.DOLocalMoveX(containerPosition, ((float)travelSpeed + 1f) * m_speedMultiplier).SetEase(Ease.OutQuint);
    }
}

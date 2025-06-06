using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class TileModificationMenu : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("References")]
    [SerializeField] private MenuController m_menuController;
    [SerializeField] private MenuPage m_menuPage;
    [SerializeField] private TileModificationMenuItem m_tileModificationMenuItemPrefab;
    [SerializeField] private RectTransform m_tileContainer;
    [SerializeField] private RectTransform m_tileModificationMenu;
    [SerializeField] private List<Button> m_buttonList;
    [SerializeField] private AvailableModificationsMenu m_availableModificationMenu;

    [Header("Settings")]
    [SerializeField] private float m_scrollSpeedMultiplier = 0.2f;
    [SerializeField] private float m_dragMultiplier = 1.2f;

    private RectTransform m_rectTransform;
    private float m_tileItemWidth;
    private TownHousingService m_townHousingService;
    private TileModificationMenuItem.Factory m_tileModificationMenuItemFactory;
    private TownTileService m_townTileService;
    private TweenerCore<Vector3, Vector3, VectorOptions> m_dragTask;

    private Guid m_selectedTileGuid = Guid.Empty;

    [Inject]
    private void Construct(TownHousingService townHousingService, TileModificationMenuItem.Factory tileModificationMenuItemFactory, TownTileService townTileService)
    {
        m_townHousingService = townHousingService;
        m_tileModificationMenuItemFactory = tileModificationMenuItemFactory;
        m_townTileService = townTileService;
    }

    private void Awake()
    {
        m_townHousingService.TileHousingModificationRequested += OnTileModificationRequested;
        m_tileItemWidth = m_tileModificationMenuItemPrefab.GetComponent<RectTransform>().sizeDelta.x;
        m_rectTransform = transform.GetComponent<RectTransform>();

        for (int i = 0; i < m_buttonList.Count; i++)
        {
            Button button = m_buttonList[i];
            int buttonIndex = i;
            button.onClick.AddListener(() => ShowAvailableModifications(buttonIndex));
        }
    }

    private void ShowAvailableModifications(int buttonIndex)
    {
        m_availableModificationMenu.SetModifications(m_selectedTileGuid, buttonIndex);

        DOTween.To(() => m_rectTransform.offsetMax.x,
            (value) => m_rectTransform.offsetMax = new Vector2(value, 0),
            -m_tileModificationMenu.sizeDelta.x, 1);
    }

    private void HideAvailableModifications()
    {
        DOTween.To(() => m_rectTransform.offsetMax.x,
            (value) => m_rectTransform.offsetMax = new Vector2(value, 0),
            0, 1);
    }

    private void OnEnable()
    {
        if (m_townTileService.ActiveTownTile != null)
        {
            int tileIndex = m_townHousingService.AvailableHousingData.Select((item, i) => new { item = item, index = i })
                .Where(itemIndex => itemIndex.item.TowerTypeGuid == m_townTileService.ActiveTownTile.ConnectedTowerID).First().index;
            m_tileContainer.localPosition = new Vector3(-m_tileItemWidth * tileIndex - m_tileItemWidth / 2, 0, 0);
        }
        else
        {
            m_tileContainer.localPosition = new Vector3(-m_tileItemWidth / 2, 0, 0);
        }

        m_tileContainer.ClearChildren();
        foreach (HousingData housingData in m_townHousingService.AvailableHousingData)
        {
            TileModificationMenuItem tile = m_tileModificationMenuItemFactory.Create();
            tile.transform.SetParent(m_tileContainer.transform, false);
            tile.SetHousingInfo(housingData);
        }

        SetTileGuid(0);
    }

    private void OnDestroy()
    {
        m_townHousingService.TileHousingModificationRequested -= OnTileModificationRequested;

        foreach (Button button in m_buttonList)
        {
            button.onClick.RemoveAllListeners();
        }
    }

    private void OnTileModificationRequested(HousingData obj)
    {
        m_menuController.PushMenuPage(m_menuPage);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_dragTask.Kill();
    }

    public void OnDrag(PointerEventData eventData)
    {
        m_tileContainer.localPosition = new Vector3(m_tileContainer.localPosition.x + (eventData.delta.x * m_dragMultiplier), m_tileContainer.localPosition.y, m_tileContainer.localPosition.z);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        HideAvailableModifications();

        int goalTile = Mathf.RoundToInt((m_tileContainer.localPosition.x + m_tileItemWidth / 2) / -m_tileItemWidth);
        goalTile += Mathf.RoundToInt(goalTile * -eventData.delta.x * 0.05f);
        goalTile = Mathf.Clamp(goalTile, 0, m_tileContainer.childCount - 1);

        float travelDistance = -m_tileItemWidth * goalTile - m_tileItemWidth / 2;
        float travelSpeed = Mathf.Clamp(Mathf.Abs(m_tileContainer.localPosition.x - travelDistance) / m_tileItemWidth, -5, 5);

        m_dragTask = m_tileContainer.DOLocalMoveX(travelDistance, ((float)travelSpeed + 1f) * m_scrollSpeedMultiplier).SetEase(Ease.OutQuint).
            OnComplete(() => SetTileGuid(goalTile));
    }

    private void SetTileGuid(int goalTile)
    {
        m_selectedTileGuid = m_tileContainer.GetChild(goalTile).GetComponent<TileModificationMenuItem>().TileGuid;
    }
}

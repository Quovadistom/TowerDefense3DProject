using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class TownTile : MonoBehaviour, IPointerClickHandler, IDragHandler
{
    [Button]
    private void SaveService()
    {
        m_townTileService.Save();
    }

    [SerializeField] private Transform m_tile;
    [SerializeField] private Transform m_tileSideHead;
    [SerializeField] private Transform m_tileSideTail;
    [SerializeField] private RectTransform m_tileContextMenu;
    [SerializeField] private Map m_connectedMap;

    private Sequence m_sequence;

    public Map ConnectedMap => m_connectedMap;
    public string Coordinates { get; private set; }
    public Guid ConnectedTowerID { get; private set; } = Guid.Empty;
    public bool IsCaptured { get; set; }
    public bool IsOccupied => ConnectedTowerID != Guid.Empty;

    private TownTileService m_townTileService;
    private TowerAvailabilityService m_towerAvailabilityService;
    private TowerTileVisual.Factory m_townTileVisualFactory;
    private bool m_dragged;

    [Inject]
    private void Construct(TownTileService townTileService, TowerAvailabilityService towerAvailabilityService, TowerTileVisual.Factory townTileVisualFactory)
    {
        m_townTileService = townTileService;
        m_towerAvailabilityService = towerAvailabilityService;
        m_townTileVisualFactory = townTileVisualFactory;
    }

    private void Awake()
    {
        Coordinates = $"{transform.parent.GetSiblingIndex()}-{transform.GetSiblingIndex()}";

        m_townTileService.ServiceRead += OnServiceRead;
    }

    private void OnDestroy()
    {
        m_townTileService.ServiceRead -= OnServiceRead;
    }

    private void OnServiceRead()
    {
        if (m_townTileService.TryGetTileData(Coordinates, out var tileData))
        {
            IsCaptured = tileData.IsCaptured;
            ConnectedTowerID = tileData.ConnectedTowerID;
        }

        if (ConnectedTowerID != Guid.Empty && m_towerAvailabilityService.TryGetTowerAssets(ConnectedTowerID, out TowerAssets towerAssets))
        {
            UpdateTile(towerAssets);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!m_dragged)
        {
            SelectTile();
        }
        else
        {
            m_dragged = false;
        }
    }

    public void SelectTile()
    {
        m_townTileService.ActiveTownTile = this;
    }

    public void UpdateTile(TowerAssets turretAssets)
    {
        SetTileContent(turretAssets);
        // m_townTileService.UpdateTile(Coordinates, TownTileData);
    }

    public void SetTileContent(TowerAssets towerAssets)
    {
        m_sequence?.Kill(true);

        if (towerAssets?.HousingPrefab != null)
        {
            Transform bottomSide = m_tile.localEulerAngles.z > 95 ? m_tileSideHead : m_tileSideTail;
            TowerTileVisual visual = m_townTileVisualFactory.Create(towerAssets.HousingPrefab, towerAssets.ID);
            visual.transform.SetParent(bottomSide, false);
        }

        m_sequence = DOTween.Sequence();
        m_sequence.Append(m_tile.DOLocalRotate(new Vector3(m_tile.localEulerAngles.x, m_tile.localEulerAngles.y, m_tile.localEulerAngles.z + 180), 2f, RotateMode.FastBeyond360).SetEase(Ease.OutElastic, 0.5f, 1f))
            .AppendCallback(() =>
            {
                Transform bottomSide = m_tile.localEulerAngles.z > 95 ? m_tileSideHead : m_tileSideTail;
                bottomSide.ClearChildren();
            });

        ConnectedTowerID = towerAssets != null ? towerAssets.ID : Guid.Empty;
    }

    public void OnDrag(PointerEventData eventData)
    {
        m_dragged = true;
    }
}

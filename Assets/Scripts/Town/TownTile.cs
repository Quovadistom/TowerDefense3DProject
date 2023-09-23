using DG.Tweening;
using NaughtyAttributes;
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

    private Sequence m_sequence;

    private HousingData m_housingData;
    public HousingData HousingData
    {
        get => m_housingData;
        set
        {
            if (m_housingData != null)
            {
                m_housingData.TileUpdated -= OnTileUpdated;
            }

            m_housingData = value;

            if (m_housingData != null)
            {
                m_housingData.TileUpdated += OnTileUpdated;
            }
        }
    }

    public TownTileData TownTileData { get; private set; } = new();

    public string Coordinates { get; private set; }

    private TownTileService m_townTileService;
    private TurretCollection m_towerCollection;
    private TownTileVisual.Factory m_townTileVisualFactory;
    private bool m_dragged;
    private TowerAssets m_currentContent;

    [Inject]
    private void Construct(TownTileService townTileService, TurretCollection towerCollection, TownTileVisual.Factory townTileVisualFactory)
    {
        m_townTileService = townTileService;
        m_towerCollection = towerCollection;
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
        if (m_townTileService.TryGetTileData(Coordinates, out TownTileData townTileData))
        {
            TownTileData = townTileData;

            // TODO: Change Add HousingData and read after service read is complete
            //if (TownTileData.IsOccupied)
            //{
            //    SetVisual(m_towerCollection.TurretAssetsList.FirstOrDefault(assets => assets.TowerPrefab.ComponentID == TownTileData.TowerTypeGuid)?.HousingPrefab);
            //}
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
        m_townTileService.UpdateTile(Coordinates, TownTileData);
    }

    private void OnTileUpdated()
    {
        SetTileContent(m_currentContent);
    }

    public void SetTileContent(TowerAssets towerAssets)
    {
        m_currentContent = towerAssets;

        m_sequence?.Kill(true);

        HousingData = towerAssets == null ? null : m_townTileService.GetHousingData(towerAssets.TowerPrefab.ComponentID);

        if (towerAssets?.HousingPrefab != null)
        {
            Transform bottomSide = m_tile.localEulerAngles.z > 95 ? m_tileSideHead : m_tileSideTail;
            TownTileVisual visual = m_townTileVisualFactory.Create(towerAssets.HousingPrefab);
            visual.transform.SetParent(bottomSide, false);
            visual.SetTileUpgrades(HousingData.ActiveUpgrades);
        }

        m_sequence = DOTween.Sequence();
        m_sequence.Append(m_tile.DOLocalRotate(new Vector3(m_tile.localEulerAngles.x, m_tile.localEulerAngles.y, m_tile.localEulerAngles.z + 180), 2f, RotateMode.FastBeyond360).SetEase(Ease.OutElastic, 0.5f, 1f))
            .AppendCallback(() =>
            {
                Transform bottomSide = m_tile.localEulerAngles.z > 95 ? m_tileSideHead : m_tileSideTail;
                bottomSide.ClearChildren();
            });

        TownTileData.IsOccupied = towerAssets != null;
    }

    public void OnDrag(PointerEventData eventData)
    {
        m_dragged = true;
    }
}

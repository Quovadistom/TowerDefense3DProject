using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class SpawnTowerButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Button m_button;
    private TowerInfoComponent.Factory m_turretFactory;
    private TouchInputService m_touchInputService;
    private LayerSettings m_layerSettings;
    private LevelService m_levelService;
    private DraggingService m_draggingService;

    public TowerAssets TurretAssets { get; set; }

    [Inject]
    public void Construct(TowerInfoComponent.Factory turretFactory,
        TouchInputService touchInputService,
        LayerSettings layerSettings,
        LevelService levelService,
        DraggingService draggingService)
    {
        m_turretFactory = turretFactory;
        m_touchInputService = touchInputService;
        m_layerSettings = layerSettings;
        m_levelService = levelService;
        m_draggingService = draggingService;
    }

    private void Awake()
    {
        m_levelService.MoneyChanged += OnMoneyChanged;
        m_draggingService.PlacementProgressChanged += OnPlacementProgressChanged;
    }

    private void OnDestroy()
    {
        m_levelService.MoneyChanged -= OnMoneyChanged;
    }

    private void OnPlacementProgressChanged(bool busy)
    {
        m_button.interactable = IsButtonInteractable();
    }

    private void OnMoneyChanged(int money)
    {
        m_button.interactable = IsButtonInteractable();
    }

    private bool IsButtonInteractable()
    {
        bool canBuyTurret = true; // TurretToSpawn.Value <= m_levelService.Money;
        bool canPlaceTurret = !m_draggingService.IsDraggingInProgress;

        return canBuyTurret && canPlaceTurret;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!m_button.IsInteractable())
        {
            return;
        }

        TowerInfoComponent turret = m_turretFactory.Create(TurretAssets.TowerPrefab, TurretAssets.ID);

        turret.StartTowerPlacement();

        if (m_touchInputService.TryGetRaycast(m_layerSettings.GameBoardLayer, out RaycastHit hit))
        {
            turret.transform.position = new Vector3(hit.point.x, 0, hit.point.z);
        }
    }

    public class Factory : PlaceholderFactory<SpawnTowerButton> { }
}

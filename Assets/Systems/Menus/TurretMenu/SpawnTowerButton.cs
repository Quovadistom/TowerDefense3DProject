using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class SpawnTowerButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Button m_button;
    private TowerModule.Factory m_turretFactory;
    private TowerService m_towerService;
    private TownTileService m_townTileService;
    private TouchInputService m_touchInputService;
    private LayerSettings m_layerSettings;
    private LevelService m_levelService;
    private DraggingService m_draggingService;
    private InflationService m_inflationService;
    private ResourceService m_resourceService;

    public TowerAssets TurretAssets { get; set; }

    [Inject]
    public void Construct(TowerModule.Factory turretFactory,
        TowerService towerService,
        TownTileService townTileService,
        TouchInputService touchInputService,
        LayerSettings layerSettings,
        LevelService levelService,
        DraggingService draggingService,
        InflationService inflationService,
        ResourceService resourceService)
    {
        m_turretFactory = turretFactory;
        m_towerService = towerService;
        m_townTileService = townTileService;
        m_touchInputService = touchInputService;
        m_layerSettings = layerSettings;
        m_levelService = levelService;
        m_draggingService = draggingService;
        m_inflationService = inflationService;
        m_resourceService = resourceService;
    }

    private void Awake()
    {
        m_draggingService.PlacementProgressChanged += OnPlacementProgressChanged;
        m_towerService.TowerModuleAdded += OnTowerModuleChanged;
        m_towerService.TowerModuleRemoved += OnTowerModuleChanged;
        m_inflationService.InflationChanged += OnInflationChanged;
        m_resourceService.ResourceChanged += OnResourcesChanged;
    }

    private void Start()
    {
        SetButtonInteraction();
    }

    private void OnDestroy()
    {
        m_draggingService.PlacementProgressChanged -= OnPlacementProgressChanged;
        m_towerService.TowerModuleAdded -= OnTowerModuleChanged;
        m_towerService.TowerModuleRemoved -= OnTowerModuleChanged;
        m_inflationService.InflationChanged -= OnInflationChanged;
        m_resourceService.ResourceChanged -= OnResourcesChanged;
    }

    private void OnPlacementProgressChanged(bool busy)
    {
        SetButtonInteraction();
    }

    private void OnInflationChanged()
    {
        SetButtonInteraction();
    }

    private void OnTowerModuleChanged(TowerModule towerModule)
    {
        if (towerModule.ID != TurretAssets.ID)
        {
            return;
        }

        SetButtonInteraction();
    }

    private void OnResourcesChanged(object sender, ResourcesChangeEventArgs e)
    {
        if (e.Resource is BattleFunds)
        {
            SetButtonInteraction();
        }
    }

    private void SetButtonInteraction()
    {
        bool hasTurretsLeft = m_townTileService.GetTowerTileAmount(TurretAssets.ID) * 3 > m_towerService.GetPlacedTowerAmount(TurretAssets.ID);
        bool canBuyTurret = TurretAssets.TowerPrefab.TowerCost.AddPercentage(
            m_inflationService.CalculateInflationPercentage(TurretAssets.TowerPrefab)) <= m_resourceService.GetAvailableResourceAmount<BattleFunds>();
        bool canPlaceTurret = !m_draggingService.IsDraggingInProgress;

        m_button.interactable = canBuyTurret && canPlaceTurret && hasTurretsLeft;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!m_button.IsInteractable())
        {
            return;
        }

        TowerModule turret = m_turretFactory.Create(TurretAssets.TowerPrefab, TurretAssets.ID);

        turret.StartTowerPlacement();

        if (m_touchInputService.TryGetRaycast(m_layerSettings.GameBoardLayer, out RaycastHit hit))
        {
            turret.transform.position = new Vector3(hit.point.x, 0, hit.point.z);
        }
    }

    public class Factory : PlaceholderFactory<SpawnTowerButton> { }
}

using System;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class SpawnTowerButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Button m_button;
    private TurretInfoComponent.Factory m_turretFactory;
    private TouchInputService m_touchInputService;
    private LayerSettings m_layerSettings;
    private LevelService m_levelService;
    private PlacementService m_placementService;

    public TurretInfoComponent TurretToSpawn { get; set; }

    [Inject]
    public void Construct(TurretInfoComponent.Factory turretFactory, 
        TouchInputService touchInputService, 
        LayerSettings layerSettings, 
        LevelService levelService, 
        PlacementService placementService)
    {
        m_turretFactory = turretFactory;
        m_touchInputService = touchInputService;
        m_layerSettings = layerSettings;
        m_levelService = levelService;
        m_placementService = placementService;
    }

    private void Awake()
    {
        m_levelService.MoneyChanged += OnMoneyChanged;
        m_placementService.PlacementProgressChanged += OnPlacementProgressChanged;
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
        bool canBuyTurret = TurretToSpawn.Value <= m_levelService.Money;
        bool canPlaceTurret = !m_placementService.IsPlacementInProgress;

        return canBuyTurret && canPlaceTurret;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!m_button.IsInteractable())
        {
            return;
        }

        TurretInfoComponent turret = m_turretFactory.Create(TurretToSpawn);
        turret.StartTowerPlacemet();

        if (m_touchInputService.TryGetRaycast(m_layerSettings.GameBoardLayer, out RaycastHit hit))
        {
            turret.transform.position = new Vector3(hit.point.x, 0, hit.point.z);
        }
    }

    public class Factory : PlaceholderFactory<SpawnTowerButton> { }
}

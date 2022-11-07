using System;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class SpawnTower : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private TurretInfoComponent m_turretToSpawn;
    [SerializeField] private Button m_button;
    private TurretInfoComponent.Factory m_turretFactory;
    private TouchInputService m_touchInputService;
    private LayerSettings m_layerSettings;
    private LevelService m_levelService;

    [Inject]
    public void Construct(TurretInfoComponent.Factory turretFactory, TouchInputService touchInputService, LayerSettings layerSettings, LevelService levelService)
    {
        m_turretFactory = turretFactory;
        m_touchInputService = touchInputService;
        m_layerSettings = layerSettings;
        m_levelService = levelService;
    }

    private void Awake()
    {
        m_levelService.MoneyChanged += OnMoneyChanged;
    }

    private void OnDestroy()
    {
        m_levelService.MoneyChanged -= OnMoneyChanged;
    }

    private void OnMoneyChanged(int money)
    {
        m_button.interactable = m_turretToSpawn.Cost <= money;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!m_button.IsInteractable())
        {
            return;
        }

        TurretInfoComponent turret = m_turretFactory.Create(m_turretToSpawn);

        if (m_touchInputService.TryGetRaycast(m_layerSettings.GameBoardLayer, out RaycastHit hit))
        {
            turret.transform.position = new Vector3(hit.point.x, 0, hit.point.z);
        }
    }
}

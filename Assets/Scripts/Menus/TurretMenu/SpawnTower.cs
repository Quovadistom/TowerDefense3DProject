using System;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class SpawnTower : MonoBehaviour, IPointerDownHandler
{
    public TurretBase TurretToSpawn;
    private SelectionService m_selectionService;
    private TurretBase.Factory m_turretFactory;
    private TouchInputService m_touchInputService;
    private LayerSettings m_layerSettings;

    [Inject]
    public void Construct(SelectionService selectionService, TurretBase.Factory turretFactory, TouchInputService touchInputService, LayerSettings layerSettings)
    {
        m_selectionService = selectionService;
        m_turretFactory = turretFactory;
        m_touchInputService = touchInputService;
        m_layerSettings = layerSettings;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        var turret = m_turretFactory.Create(TurretToSpawn);

        if (m_touchInputService.TryGetRaycast(m_layerSettings.GameBoardLayer, out RaycastHit hit))
        {
            turret.transform.position = new Vector3(hit.point.x, 0, hit.point.z);
            m_selectionService.ForceSetSelected(turret.transform);
        }
    }
}

using Assets.Scripts.Interactables;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TowerModule : ModuleParent
{
    [SerializeField] private TowerUpgradeTree m_upgradeTreeData;

    public Selectable Selectable;
    public Draggable Draggable;

    private TowerService m_turretService;
    private SelectionService m_selectionService;

    public int AvailableUpgradeAmount = 5;

    public Guid TowerID { get; private set; }
    public List<Guid> ConnectedSupportTowers { get; set; } = new List<Guid>();
    public bool IsTowerPlaced { get; private set; } = false;
    public TowerUpgradeTreeData UpgradeTreeData { get; private set; }

    [Inject]
    private void Construct(Guid id, TowerService turretService, SelectionService selectionService)
    {
        ID = id;
        m_turretService = turretService;
        m_selectionService = selectionService;
    }

    protected override void Awake()
    {
        base.Awake();

        if (m_upgradeTreeData != null)
        {
            UpgradeTreeData = Instantiate(m_upgradeTreeData).TowerUpgradeTreeData;
            UpgradeTreeData.Initialize();
        }

        Draggable.PlacementRequested += OnTowerPlaced;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        Draggable.PlacementRequested -= OnTowerPlaced;
    }

    private void OnTowerPlaced()
    {
        Draggable.CanDrag = false;
        m_turretService.AddTower(this);
        IsTowerPlaced = true;

        m_selectionService.ForceSetSelected(transform);
    }

    public void EnableTowerDragging() => Draggable.CanDrag = true;

    public void StartTowerPlacement()
    {
        TowerID = Guid.NewGuid();
        m_selectionService.ForceClearSelected();
        EnableTowerDragging();
        //SubtractCost();
    }

    public void PlaceNewTower(Guid towerID, Vector3 position, TowerUpgradeTreeData treeData, List<Guid> connectedSupportTowers)
    {
        TowerID = towerID;
        transform.position = position;
        UpgradeTreeData.CopyTreeData(treeData, this);
        ConnectedSupportTowers = connectedSupportTowers;
        Draggable.CanDrag = false;
        m_turretService.AddTower(this);
        Selectable.SetSelected(false);
        IsTowerPlaced = true;
    }

    public class Factory : PlaceholderFactory<TowerModule, Guid, TowerModule>
    {
    }
}
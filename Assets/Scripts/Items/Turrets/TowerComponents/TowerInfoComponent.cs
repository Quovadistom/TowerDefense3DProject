using Assets.Scripts.Interactables;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TowerInfoComponent : ValueComponent, ITowerComponent
{
    [SerializeField] private TowerUpgradeTreeData m_upgradeTreeData;

    [ReadOnly] public string TowerTypeID;
    public Selectable Selectable;
    public Draggable Draggable;

    private TowerService m_turretService;
    private SelectionService m_selectionService;

    public int AvailableUpgradeAmount = 5;
    public Guid TowerID { get; private set; }
    public List<Guid> ConnectedSupportTowers { get; set; } = new List<Guid>();
    public bool IsTowerPlaced { get; private set; } = false;
    public TowerUpgradeTreeData UpgradeTreeData { get; private set; }

    [Button]
    public void GenerateID()
    {
        TowerTypeID = Guid.NewGuid().ToString();
    }

    [Inject]
    public void Construct(TowerService turretService, SelectionService selectionService)
    {
        m_turretService = turretService;
        m_selectionService = selectionService;
    }

    private void Awake()
    {
        if (UpgradeTreeData != null)
        {
            UpgradeTreeData = Instantiate(m_upgradeTreeData);
            UpgradeTreeData.Initialize();
        }

        Draggable.PlacementRequested += OnTowerPlaced;
    }

    private void OnDestroy()
    {
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
        SubtractCost();
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

    public class Factory : PlaceholderFactory<TowerInfoComponent, TowerInfoComponent>
    {
        private TowerBoostService m_towerUpgradeService;

        public Factory(TowerBoostService towerUpgradeService)
        {
            m_towerUpgradeService = towerUpgradeService;
        }

        public override TowerInfoComponent Create(TowerInfoComponent param)
        {
            TowerInfoComponent tower = base.Create(param);
            m_towerUpgradeService.SetTowerBoosts(tower);
            return tower;
        }
    }
}

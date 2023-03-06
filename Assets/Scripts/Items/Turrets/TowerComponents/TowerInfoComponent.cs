using Assets.Scripts.Interactables;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public enum TowerType
{
    NONE = 0,
    STANDARD = 1,
    EXPLOSION = 2,
    LASER = 3,
    SUPPORT = 4
}

public class TowerInfoComponent : ValueComponent, ITowerComponent
{
    public TurretUpgradeTreeBase UpgradeTreeAsset;
    public TowerType TurretType;
    public Selectable Selectable;
    public Draggable Draggable;

    private TowerService m_turretService;
    private SelectionService m_selectionService;

    public Guid TowerID { get; private set; }
    public List<Guid> ConnectedSupportTowers { get; set; } = new List<Guid>();
    public bool IsTowerPlaced { get; private set; } = false;

    [Inject]
    public void Construct(TowerService turretService, SelectionService selectionService)
    {
        m_turretService = turretService;
        m_selectionService = selectionService;
    }

    private void Awake()
    {
        Draggable.PlacementRequested += OnTowerPlaced;
    }

    private void OnDestroy()
    {
        Draggable.PlacementRequested -= OnTowerPlaced;
    }

    private void OnTowerPlaced()
    {
        m_selectionService.ForceSetSelected(transform);
        Draggable.CanDrag = false;
        m_turretService.AddTower(this);
        IsTowerPlaced = true;
    }

    public void EnableTowerDragging() => Draggable.CanDrag = true;

    public void StartTowerPlacement()
    {
        TowerID = Guid.NewGuid();
        m_selectionService.ForceClearSelected();
        EnableTowerDragging();
        SubtractCost();
    }

    public void PlaceNewTower(Guid towerID, Vector3 position, List<string> upgrades, List<Guid> connectedSupportTowers)
    {
        TowerID = towerID;
        transform.position = position;
        UpgradeTreeAsset.SetUnlockedUpgrades(upgrades);
        ConnectedSupportTowers = connectedSupportTowers;
        Draggable.CanDrag = false;
        m_turretService.AddTower(this);
        Selectable.SetSelected(false);
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

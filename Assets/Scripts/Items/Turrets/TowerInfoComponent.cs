using NaughtyAttributes;
using System.ComponentModel.Design;
using UnityEngine;
using Zenject;

public enum TowerType
{
    NONE = 0,
    STANDARD = 1,
    EXPLOSION = 2,
    LASER = 3
}

public class TowerInfoComponent : ValueComponent, ITowerComponent
{
    public TurretUpgradeTreeBase UpgradeTreeAsset;
    public TowerType TurretType;
    public Draggable Draggable;

    private TowerService m_turretService;
    private SelectionService m_selectionService;

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
    }

    [Button]
    public void StartTowerPlacement()
    {
        m_selectionService.ForceClearSelected();
        Draggable.CanDrag = true;
        SubtractCost();
    }

    public void FinalizeTowerPlacement()
    {
        Draggable.CanDrag = false;
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

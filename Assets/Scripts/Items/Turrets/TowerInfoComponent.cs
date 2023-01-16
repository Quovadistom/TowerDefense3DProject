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

    [Inject]
    public void Construct(TowerService turretService)
    {
        m_turretService = turretService;
    }

    public void Awake()
    {
        Draggable.TowerPlaced += OnTowerPlaced;
    }

    private void OnTowerPlaced()
    {
        m_turretService.AddTower(this);
    }

    public void StartTowerPlacemet()
    {
        Draggable.StartDragging();
    }

    public void FinalizeTowerPlacement()
    {
        Draggable.ForceEndDragging();
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

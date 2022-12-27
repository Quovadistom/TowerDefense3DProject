using UnityEngine;
using Zenject;

public enum TurretType
{
    STANDARD = 0,
    EXPLOSION = 1,
    LASER = 2
}

public class TurretInfoComponent : ValueComponent
{
    public TurretUpgradeTreeBase UpgradeTreeAsset;
    public TurretType TurretType;
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

    public class Factory : PlaceholderFactory<TurretInfoComponent, TurretInfoComponent>
    {
    }
}

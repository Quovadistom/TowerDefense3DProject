using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class TurretInfo
{
    public Guid TowerID { get; set; }
    public string TurretName { get; set; }
    public Vector3 Position { get; set; }
    public TowerUpgradeTreeData TowerUpgradeTree { get; set; }
    public List<Guid> ConnectedSupportTowers { get; set; }
    public string TargetMethodName { get; set; }
}

public class TowerService : ServiceSerializationHandler<TurretServiceDto>
{
    private List<TowerInfoComponent> m_placedTurrets = new List<TowerInfoComponent>();
    private TurretCollection m_turretCollection;
    private TowerInfoComponent.Factory m_turretFactory;

    public TowerService(TurretCollection turretCollection, TowerInfoComponent.Factory turretFactory, SerializationService serializationService, DebugSettings debugSettings) : base(serializationService, debugSettings)
    {
        m_turretCollection = turretCollection;
        m_turretFactory = turretFactory;
    }

    public void AddTower(TowerInfoComponent turretInfoComponent)
    {
        m_placedTurrets.Add(turretInfoComponent);
    }

    public void RemoveTower(TowerInfoComponent selectedTurret)
    {
        m_placedTurrets.Remove(selectedTurret);
    }

    protected override Guid Id => Guid.Parse("c35801bc-3e17-4ae9-9b46-b0b6dd990769");

    protected override void ConvertDto()
    {
        List<TurretInfo> placedTurrets = new();

        foreach (TowerInfoComponent placedTurret in m_placedTurrets)
        {
            TurretInfo turretInfo = new TurretInfo()
            {
                TowerID = placedTurret.TowerID,
                TurretName = placedTurret.TowerTypeID,
                Position = placedTurret.transform.position,
                TowerUpgradeTree = placedTurret.UpgradeTreeData,
                ConnectedSupportTowers = placedTurret.ConnectedSupportTowers
            };

            if (placedTurret.TryGetComponent(out TurretTargetingComponent turretTargetingComponent))
            {
                turretInfo.TargetMethodName = turretTargetingComponent.CurrentTargetMethod.Name;
            }

            placedTurrets.Add(turretInfo);
        }

        Dto.PlacedTurrets = placedTurrets;
    }

    protected override void ConvertDtoBack(TurretServiceDto dto)
    {
        foreach (TurretInfo selectedTurret in dto.PlacedTurrets)
        {
            TowerInfoComponent turretPrefab = m_turretCollection.TurretList.FirstOrDefault(turret => turret.TowerTypeID == selectedTurret.TurretName);
            TowerInfoComponent placedTurret = m_turretFactory.Create(turretPrefab);
            placedTurret.PlaceNewTower(selectedTurret.TowerID, selectedTurret.Position, selectedTurret.TowerUpgradeTree, selectedTurret.ConnectedSupportTowers);

            if (placedTurret.TryGetComponent(out TurretTargetingComponent turretTargetingComponent))
            {
                turretTargetingComponent.CurrentTargetMethod = m_turretCollection.TargetMethodList.First(x => x.Name == selectedTurret.TargetMethodName);
            }
        }
    }
}

public class TurretServiceDto
{
    public List<TurretInfo> PlacedTurrets;
}

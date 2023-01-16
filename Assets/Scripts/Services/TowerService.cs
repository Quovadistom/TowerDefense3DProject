using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

[Serializable]
public class TurretInfo
{
    public TowerType TurretType{ get; set; }
    public Vector3 Position { get; set; }
    public List<string> UnlockedUpgrades { get; set; }
}

public class TowerService : ServiceSerializationHandler<TurretServiceDto>
{
    private List<TowerInfoComponent> m_placedTurrets = new List<TowerInfoComponent>();
    private TurretCollection m_turretCollection;
    private TowerInfoComponent.Factory m_turretFactory;

    public TowerService(TurretCollection turretCollection, TowerInfoComponent.Factory turretFactory, SerializationService serializationService) : base(serializationService)
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
        List<TurretInfo> placedTurrets= new List<TurretInfo>();

        foreach(TowerInfoComponent placedTurret in m_placedTurrets)
        {
            placedTurrets.Add(new TurretInfo()
            {
                TurretType = placedTurret.TurretType,
                Position = placedTurret.transform.position,
                UnlockedUpgrades = placedTurret.UpgradeTreeAsset.GetUnlockedUpgrades()
            });
        }

        Dto.PlacedTurrets = placedTurrets;
    }

    protected override void ConvertDtoBack(TurretServiceDto dto)
    {
        foreach (TurretInfo selectedTurret in dto.PlacedTurrets)
        {
            TowerInfoComponent turretPrefab = m_turretCollection.TurretList.FirstOrDefault(turret => turret.TurretType == selectedTurret.TurretType);
            TowerInfoComponent placedTurret = m_turretFactory.Create(turretPrefab);
            placedTurret.transform.position = selectedTurret.Position;
            placedTurret.UpgradeTreeAsset.SetUnlockedUpgrades(selectedTurret.UnlockedUpgrades);
            placedTurret.FinalizeTowerPlacement();
        }
    }
}

public class TurretServiceDto
{
    public List<TurretInfo> PlacedTurrets;
}

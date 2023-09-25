using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class TowerBoostRow
{
    public Guid TowerType = Guid.Empty;
    public Guid[] UpgradeIDs = new Guid[3]
    {
        Guid.Empty,
        Guid.Empty,
        Guid.Empty,
    };
}

public class TowerBoostService : ServiceSerializationHandler<TowerBoostServiceDto>
{
    private TowerBoostRow[] m_towerBoostRows = new TowerBoostRow[5]
    {
        new TowerBoostRow(),
        new TowerBoostRow(),
        new TowerBoostRow(),
        new TowerBoostRow(),
        new TowerBoostRow()
    };

    private readonly BoostCollection m_boostCollection;
    private readonly BoostAvailabilityService m_boostAvailabilityService;
    private readonly BoostService m_boostService;

    public event Action<int, Guid> TurretTypeChanged;
    public event Action<Guid, int, BoostContainer> TurretUpgradeChanged;

    public ICollection<TowerBoostRow> TowerBoostRows => m_towerBoostRows.AsReadOnlyCollection();

    protected override Guid Id => Guid.Parse("fd1c3b87-e564-4187-8cc5-4a5688c953ba");

    public TowerBoostService(BoostCollection boostCollection,
        BoostAvailabilityService boostAvailabilityService,
        SerializationService serializationService,
        DebugSettings debugSettings,
        BoostService boostService) : base(serializationService, debugSettings)
    {
        m_boostCollection = boostCollection;
        m_boostAvailabilityService = boostAvailabilityService;
        m_boostService = boostService;
    }

    public bool TryGetTowerUpgradeInfo(Guid name, out BoostContainer boostContainer)
    {
        boostContainer = m_boostCollection.BoostList.FirstOrDefault(x => x.Name == name);
        return boostContainer != null;
    }

    public void UpdateTowerUpgradeCollection(int upgradeIndex, Guid turretType)
    {
        m_towerBoostRows[upgradeIndex].TowerType = turretType;
        m_towerBoostRows[upgradeIndex].UpgradeIDs = new Guid[3];

        TurretTypeChanged?.Invoke(upgradeIndex, turretType);
    }

    public void UpdateTowerBoostCollection(Guid towerType, int upgradeIndex, BoostContainer upgrade)
    {
        TowerBoostRow row = m_towerBoostRows.FirstOrDefault(x => x.TowerType == towerType);
        if (row != null)
        {
            row.UpgradeIDs[upgradeIndex] = upgrade.Name;
            m_boostAvailabilityService.RemoveAvailableBoost(upgrade.Name);
            m_boostService.AddUpgrade(upgrade);
            TurretUpgradeChanged?.Invoke(towerType, upgradeIndex, upgrade);
        }
        else
        {
            Debug.LogWarning($"No row with tower type {towerType} found!");
        }
    }

    protected override void ConvertDto()
    {
        Dto.TowerUpgradeRows = m_towerBoostRows;
    }

    protected override void ConvertDtoBack(TowerBoostServiceDto dto)
    {
        m_towerBoostRows = dto.TowerUpgradeRows;
    }
}

public class TowerBoostServiceDto
{
    public TowerBoostRow[] TowerUpgradeRows;
}

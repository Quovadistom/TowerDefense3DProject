using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class TowerBoostRow
{
    public string TowerType = string.Empty;
    public string[] UpgradeIDs = new string[3]
    {
        string.Empty,
        string.Empty,
        string.Empty,
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

    public event Action<int, string> TurretTypeChanged;
    public event Action<string, int, TowerUpgradeBase> TurretUpgradeChanged;

    public ICollection<TowerBoostRow> TowerBoostRows => m_towerBoostRows.AsReadOnlyCollection();

    protected override Guid Id => Guid.Parse("fd1c3b87-e564-4187-8cc5-4a5688c953ba");

    public TowerBoostService(BoostCollection boostCollection, BoostAvailabilityService boostAvailabilityService, SerializationService serializationService, DebugSettings debugSettings) : base(serializationService, debugSettings)
    {
        m_boostCollection = boostCollection;
        m_boostAvailabilityService = boostAvailabilityService;
    }

    public bool TryGetTowerUpgradeInfo(string id, out TowerUpgradeBase towerUpgradeBase)
    {
        towerUpgradeBase = (TowerUpgradeBase)m_boostCollection.TowerBoostList.Select(x => x.Boost).FirstOrDefault(x => x.ID == id);
        return towerUpgradeBase != null;
    }

    public void UpdateTowerUpgradeCollection(int upgradeIndex, string turretType)
    {
        m_towerBoostRows[upgradeIndex].TowerType = turretType;
        m_towerBoostRows[upgradeIndex].UpgradeIDs = new string[3];

        TurretTypeChanged?.Invoke(upgradeIndex, turretType);
    }

    public void UpdateTowerBoostCollection(string towerType, int upgradeIndex, TowerUpgradeBase upgrade)
    {
        TowerBoostRow row = m_towerBoostRows.FirstOrDefault(x => x.TowerType == towerType);
        if (row != null)
        {
            row.UpgradeIDs[upgradeIndex] = upgrade.ID;
            m_boostAvailabilityService.RemoveAvailableBoost(upgrade.ID);
            TurretUpgradeChanged?.Invoke(towerType, upgradeIndex, upgrade);
        }
        else
        {
            Debug.LogWarning($"No row with tower type {towerType} found!");
        }
    }

    public void SetTowerBoosts(TowerInfoComponent towerInfoComponent)
    {
        TowerBoostRow row = m_towerBoostRows.FirstOrDefault(x => x.TowerType == towerInfoComponent.TowerTypeID);
        if (row != null)
        {
            foreach (string upgradeID in row.UpgradeIDs)
            {
                if (!string.IsNullOrEmpty(upgradeID))
                {
                    TowerUpgradeBase upgrade = (TowerUpgradeBase)m_boostCollection.TowerBoostList.FirstOrDefault(x => x.Boost.ID == upgradeID).Boost;
                    if (upgrade != null)
                    {
                        upgrade.TryApplyUpgrade(towerInfoComponent);
                    }
                }
            }
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

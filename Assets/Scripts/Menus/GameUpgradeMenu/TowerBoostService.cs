using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class TowerBoostRow
{
    public TowerType TowerType = TowerType.NONE;
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

    private BoostCollection m_boostCollection;
    private BoostAvailabilityService m_boostAvailabilityService;

    public event Action<int, TowerType> TurretTypeChanged;
    public event Action<TowerType, int, string> TurretUpgradeChanged;

    public ICollection<TowerBoostRow> TowerBoostRows => m_towerBoostRows.AsReadOnlyCollection();

    protected override Guid Id => Guid.Parse("fd1c3b87-e564-4187-8cc5-4a5688c953ba");

    public TowerBoostService(BoostCollection boostCollection, BoostAvailabilityService boostAvailabilityService, SerializationService serializationService) : base(serializationService)
    {
        m_boostCollection = boostCollection;
        m_boostAvailabilityService = boostAvailabilityService;
    }

    public void UpdateTowerUpgradeCollection(int upgradeIndex, TowerType turretType)
    {
        m_towerBoostRows[upgradeIndex].TowerType = turretType;
        m_towerBoostRows[upgradeIndex].UpgradeIDs = new string[3];

        TurretTypeChanged?.Invoke(upgradeIndex, turretType);
    }

    public void UpdateTowerBoostCollection(TowerType towerType, int upgradeIndex, string upgradeID)
    {
        TowerBoostRow row = m_towerBoostRows.FirstOrDefault(x => x.TowerType == towerType);
        if (row != null)
        {
            row.UpgradeIDs[upgradeIndex] = upgradeID;
            m_boostAvailabilityService.RemoveAvailableBoost(upgradeID);
            TurretUpgradeChanged?.Invoke(towerType, upgradeIndex, upgradeID);
        }
        else
        {
            Debug.LogWarning($"No row with tower type {towerType} found!");
        }
    }

    public void SetTowerBoosts(TowerInfoComponent towerInfoComponent)
    {
        TowerBoostRow row = m_towerBoostRows.FirstOrDefault(x => x.TowerType == towerInfoComponent.TurretType);
        if (row != null)
        {
            foreach (string upgradeID in row.UpgradeIDs)
            {
                TowerUpgradeBase upgrade = m_boostCollection.TowerBoostList.FirstOrDefault(x => x.Boost.ID == upgradeID).Boost;
                if (upgrade != null)
                {
                    upgrade.TryApplyUpgrade(towerInfoComponent);
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

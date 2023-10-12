using System;
using System.Collections.Generic;
using System.Linq;

public class TowerAssetsAvailibilityWrapper
{
    public TowerAssetsAvailibilityWrapper(TowerAssets towerAssets, bool isAvailable = false)
    {
        TowerAssets = towerAssets;
        IsAvailable = isAvailable;
    }

    public TowerAssets TowerAssets { get; set; }
    public bool IsAvailable { get; set; }
}

public class TowerAvailabilityService : ServiceSerializationHandler<TowerAvailabilityServiceDto>
{
    private TurretCollection m_turretCollection;
    private IEnumerable<TowerAssetsAvailibilityWrapper> m_towerAssetsCollection;

    public IReadOnlyList<TowerAssets> AvailableTowers => m_towerAssetsCollection.Where(tower => tower.IsAvailable).Select(tower => tower.TowerAssets).ToList();

    public TowerAvailabilityService(SerializationService serializationService, DebugSettings debugSettings, TurretCollection turretCollection) : base(serializationService, debugSettings)
    {
        m_turretCollection = turretCollection;
        m_towerAssetsCollection = m_turretCollection.TurretAssetsList.Select(tower => new TowerAssetsAvailibilityWrapper(tower, tower.IsStartingTower));
    }

    public bool TryGetTowerAssets(Guid towerID, out TowerAssets towerAssets)
    {
        towerAssets = AvailableTowers.FirstOrDefault(tower => tower.ID == towerID);
        return towerAssets != null;
    }

    protected override Guid Id => Guid.Parse("d1284628-cd1a-4995-be56-6874c69ba45e");

    protected override void ConvertDto()
    {
        Dto.AvailableTowerID = AvailableTowers.Select(tower => tower.TowerPrefab.ID);
    }

    protected override void ConvertDtoBack(TowerAvailabilityServiceDto dto)
    {
        if (dto.AvailableTowerID != null && dto.AvailableTowerID.Count() > 0)
        {
            foreach (TowerAssetsAvailibilityWrapper item in m_towerAssetsCollection)
            {
                item.IsAvailable = dto.AvailableTowerID.Contains(item.TowerAssets.TowerPrefab.ID);
            }
        }
    }
}

public class TowerAvailabilityServiceDto
{
    public IEnumerable<Guid> AvailableTowerID;
}

using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class HousingData
{
    public HousingData(Guid towerTypeGuid)
    {
        TowerTypeGuid = towerTypeGuid;
    }

    public Guid TowerTypeGuid { get; set; }

    /// <summary>
    /// Active upgrades on this tile. There are a maximum of 4 available slots to upgrade from
    /// </summary>
    public Guid[] ActiveUpgrades { get; set; } = new Guid[4]
    {
        Guid.Empty,
        Guid.Empty,
        Guid.Empty,
        Guid.Empty,
    };
}

public class TownHousingService : ServiceSerializationHandler<TownHousingServiceDTO>
{
    public IReadOnlyList<HousingData> AvailableHousingData => m_housingData.Values.ToList();

    private Dictionary<Guid, HousingData> m_housingData = new();
    private TowerAvailabilityService m_towerAvailabilityService;
    private EnhancementService m_enhancementService;

    public event Action<HousingData> TileHousingUpgradeRequested;
    public event Action<HousingData, int> TileUpgradeApplied;

    public TownHousingService(SerializationService serializationService,
        DebugSettings debugSettings,
        TowerAvailabilityService towerAvailabilityService,
        EnhancementService enhancementService) : base(serializationService, debugSettings)
    {
        m_towerAvailabilityService = towerAvailabilityService;
        m_enhancementService = enhancementService;

        foreach (TowerAssets towerAssets in m_towerAvailabilityService.AvailableTowers)
        {
            m_housingData.Add(towerAssets.ID, new HousingData(towerAssets.ID));
        }
    }

    public HousingData GetHousingData(Guid guid)
    {
        if (!m_housingData.ContainsKey(guid))
        {
            m_housingData.Add(guid, new HousingData(guid));
        }

        return m_housingData[guid];
    }

    public void RequestTileUpgrade(Guid guid) => TileHousingUpgradeRequested?.Invoke(m_housingData[guid]);

    public void UpgradeTile(Guid tileID, EnhancementContainer upgrade, int location)
    {
        HousingData housingData = GetHousingData(tileID);

        if (housingData.ActiveUpgrades[location] != Guid.Empty)
        {
            m_enhancementService.RemoveUpgrade(housingData.ActiveUpgrades[location]);
        }

        housingData.ActiveUpgrades[location] = upgrade.ID;

        m_enhancementService.AddUpgrade(upgrade.ID);
        TileUpgradeApplied?.Invoke(housingData, location);
    }

    protected override Guid Id => Guid.Parse("89ffe769-ce80-42ca-b8e2-9eb55a8adef8");

    protected override void ConvertDto()
    {
        Dto.HousingData = m_housingData;
    }

    protected override void ConvertDtoBack(TownHousingServiceDTO dto)
    {
        foreach (KeyValuePair<Guid, HousingData> housingData in dto.HousingData)
        {
            m_housingData.AddOrOverwriteKey(housingData.Key, housingData.Value);
        }
    }
}

[Serializable]
public class TownHousingServiceDTO
{
    public Dictionary<Guid, HousingData> HousingData = new();
}

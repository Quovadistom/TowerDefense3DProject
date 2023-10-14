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
    public EnhancementContainer[] ActiveUpgrades { get; set; } = new EnhancementContainer[4]
    {
        null,
        null,
        null,
        null
    };
}

public class TownHousingService : ServiceSerializationHandler<TownHousingServiceDTO>
{
    public IReadOnlyList<HousingData> AvailableHousingData => m_housingData.Values.ToList();

    private Dictionary<Guid, HousingData> m_housingData = new();
    private TowerAvailabilityService m_towerAvailabilityService;
    private EnhancementCollection m_enhancementCollection;
    private ModuleModificationService m_enhancementService;

    public event Action<HousingData> TileHousingUpgradeRequested;
    public event Action<HousingData, int> TileUpgradeApplied;

    public TownHousingService(SerializationService serializationService,
        DebugSettings debugSettings,
        TowerAvailabilityService towerAvailabilityService,
        EnhancementCollection enhancementCollection,
        ModuleModificationService enhancementService) : base(serializationService, debugSettings)
    {
        m_towerAvailabilityService = towerAvailabilityService;
        m_enhancementCollection = enhancementCollection;
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

        if (housingData.ActiveUpgrades[location] != null)
        {
            m_enhancementService.RemoveEnhancement(housingData.ActiveUpgrades[location]);
        }

        housingData.ActiveUpgrades[location] = upgrade;

        m_enhancementService.AddEnhancement(upgrade);
        TileUpgradeApplied?.Invoke(housingData, location);
    }

    protected override Guid Id => Guid.Parse("89ffe769-ce80-42ca-b8e2-9eb55a8adef8");

    protected override void ConvertDto()
    {
        Dto.HousingData = m_housingData.ToDictionary(key => key.Key, value => value.Value.ActiveUpgrades.Select(upgrade => upgrade.ID).ToArray());
    }

    protected override void ConvertDtoBack(TownHousingServiceDTO dto)
    {
        foreach (KeyValuePair<Guid, Guid[]> keyValuePair in dto.HousingData)
        {
            for (int i = 0; i < keyValuePair.Value.Length; i++)
            {
                if (m_enhancementCollection.TryGetEnhancement(keyValuePair.Value[i], out EnhancementContainer enhancement))
                {
                    UpgradeTile(keyValuePair.Key, enhancement, i);
                }
            }
        }
    }
}

[Serializable]
public class TownHousingServiceDTO
{
    public Dictionary<Guid, Guid[]> HousingData = new();
}

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
    /// Active modifications on this tile. There are a maximum of 4 available slots to modification from
    /// </summary>
    public Blueprint[] ActiveModifications { get; set; } = new Blueprint[4]
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
    private BlueprintService m_blueprintService;

    public event Action<HousingData> TileHousingModificationRequested;
    public event Action<HousingData, int> TileModificationApplied;

    public TownHousingService(SerializationService serializationService,
        DebugSettings debugSettings,
        TowerAvailabilityService towerAvailabilityService,
        BlueprintService blueprintService) : base(serializationService, debugSettings)
    {
        m_towerAvailabilityService = towerAvailabilityService;
        m_blueprintService = blueprintService;

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

    public void RequestTileModification(Guid guid) => TileHousingModificationRequested?.Invoke(m_housingData[guid]);

    public void ModificateTile(Guid tileID, Blueprint blueprint, int location)
    {
        HousingData housingData = GetHousingData(tileID);

        if (housingData.ActiveModifications[location] != null)
        {
            m_blueprintService.SellBlueprint(blueprint);
        }

        housingData.ActiveModifications[location] = blueprint;

        m_blueprintService.BuyBlueprint(blueprint);

        TileModificationApplied?.Invoke(housingData, location);
    }

    protected override Guid Id => Guid.Parse("89ffe769-ce80-42ca-b8e2-9eb55a8adef8");

    protected override void ConvertDto()
    {
        Dto.HousingData = m_housingData.ToDictionary(key => key.Key, value => value.Value.ActiveModifications.Select(modification => modification.ID).ToArray());
    }

    protected override void ConvertDtoBack(TownHousingServiceDTO dto)
    {
        foreach (KeyValuePair<Guid, Guid[]> keyValuePair in dto.HousingData)
        {
            for (int i = 0; i < keyValuePair.Value.Length; i++)
            {
                if (m_blueprintService.TryGetBlueprint(keyValuePair.Value[i], out Blueprint modification))
                {
                    ModificateTile(keyValuePair.Key, modification, i);
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

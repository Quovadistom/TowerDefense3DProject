using System;
using System.Collections.Generic;

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
    public Guid[] ActiveUpgrades { get; set; } = new Guid[4];
}

public class TownHousingService : ServiceSerializationHandler<TownHousingServiceDTO>
{
    private Dictionary<Guid, HousingData> m_housingData = new();
    public event Action<HousingData> TileHousingUpgradeRequested;

    public TownHousingService(SerializationService serializationService, DebugSettings debugSettings) : base(serializationService, debugSettings)
    {
    }


    public HousingData GetHousingData(Guid guid)
    {
        if (!m_housingData.ContainsKey(guid))
        {
            m_housingData.Add(guid, new HousingData(guid));
        }

        return m_housingData[guid];
    }

    public void UpgradeActiveTile(Guid guid) => TileHousingUpgradeRequested?.Invoke(m_housingData[guid]);

    protected override Guid Id => Guid.Parse("89ffe769-ce80-42ca-b8e2-9eb55a8adef8");

    protected override void ConvertDto()
    {
        Dto.HousingData = m_housingData;
    }

    protected override void ConvertDtoBack(TownHousingServiceDTO dto)
    {
        m_housingData = dto.HousingData;
    }
}

[Serializable]
public class TownHousingServiceDTO
{
    public Dictionary<Guid, HousingData> HousingData = new();
}

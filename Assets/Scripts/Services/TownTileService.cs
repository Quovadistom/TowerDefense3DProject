using System;
using System.Collections.Generic;

[Serializable]
public class TownTileData
{
    /// <summary>
    /// Is the tile under the players control
    /// </summary>
    public bool IsCaptured { get; set; } = false;

    /// <summary>
    /// Is the tile empty or does it have a building
    /// </summary>
    public bool IsOccupied { get; set; } = false;
}

public class HousingData
{
    public HousingData(string towerTypeGuid)
    {
        TowerTypeGuid = towerTypeGuid;
    }

    public string TowerTypeGuid { get; set; }

    /// <summary>
    /// Active upgrades on this tile. There are a maximum of 4 available slots to upgrade from
    /// </summary>
    public string[] ActiveUpgrades { get; set; } = new string[4];

    public event Action TileUpdated;
}

public class TownTileService : ServiceSerializationHandler<TownTileServiceDTO>
{
    public event Action<TownTile> TileSelected;
    public event Action<TownTile> TileUpgradeRequested;

    private TownTile m_activeTownTile;
    private Dictionary<string, TownTileData> m_townTileData = new();
    private Dictionary<string, HousingData> m_housingData = new();

    public TownTileService(SerializationService serializationService, DebugSettings debugSettings) : base(serializationService, debugSettings)
    {
    }

    public TownTile ActiveTownTile
    {
        get => m_activeTownTile;
        set
        {
            m_activeTownTile = value;
            TileSelected?.Invoke(m_activeTownTile);
        }
    }

    public void UpdateTile(string coordinates, TownTileData townTileData)
    {
        m_townTileData.AddOrOverwriteKey(coordinates, townTileData);
    }

    public bool TryGetTileData(string coordinate, out TownTileData townTileSetup) => m_townTileData.TryGetValue(coordinate, out townTileSetup);

    public void UpgradeActiveTile() => TileUpgradeRequested?.Invoke(ActiveTownTile);

    public HousingData GetHousingData(string guid)
    {
        if (!m_housingData.ContainsKey(guid))
        {
            m_housingData.Add(guid, new HousingData(guid));
        }

        return m_housingData[guid];
    }

    protected override Guid Id => Guid.Parse("2041c455-0822-4f6d-a11e-f75fe8783f4b");

    protected override void ConvertDto()
    {
        Dto.TownTileData = m_townTileData;
        Dto.HousingData = m_housingData;

    }

    protected override void ConvertDtoBack(TownTileServiceDTO dto)
    {
        m_townTileData = dto.TownTileData;
        m_housingData = dto.HousingData;
    }
}

[Serializable]
public class TownTileServiceDTO
{
    public Dictionary<string, TownTileData> TownTileData = new();
    public Dictionary<string, HousingData> HousingData = new();
}

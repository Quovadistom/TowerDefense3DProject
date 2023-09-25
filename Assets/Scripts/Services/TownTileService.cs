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
    /// The Guid of the connecting tower
    /// </summary>
    public Guid ConnectedTowerID { get; set; }

    /// <summary>
    /// Is the tile empty or does it have a building
    /// </summary>
    public bool IsOccupied => ConnectedTowerID != Guid.Empty;
}

public class TownTileService : ServiceSerializationHandler<TownTileServiceDTO>
{
    public event Action<TownTile> TileSelected;

    private TownTile m_activeTownTile;
    private Dictionary<string, TownTileData> m_townTileData = new();

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

    protected override Guid Id => Guid.Parse("2041c455-0822-4f6d-a11e-f75fe8783f4b");

    protected override void ConvertDto()
    {
        Dto.TownTileData = m_townTileData;

    }

    protected override void ConvertDtoBack(TownTileServiceDTO dto)
    {
        m_townTileData = dto.TownTileData;
    }
}

[Serializable]
public class TownTileServiceDTO
{
    public Dictionary<string, TownTileData> TownTileData = new();
    public Dictionary<string, HousingData> HousingData = new();
}

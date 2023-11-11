using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class TownTileData
{
    /// <summary>
    /// Is the tile under the players control
    /// </summary>
    public bool IsCaptured { get; set; }

    /// <summary>
    /// The Guid of the connecting tower
    /// </summary>
    public Guid ConnectedTowerID { get; set; }
}

public class TownTileService : ServiceSerializationHandler<TownTileServiceDTO>
{
    public event Action<TownTile> TileSelected;

    private TownTile m_activeTownTile;
    private Dictionary<string, TownTileData> m_townTileData = new();

    public TownTileService(TownSettings townSettings, SerializationService serializationService, DebugSettings debugSettings) : base(serializationService, debugSettings)
    {
        foreach (StartingTownTile tileData in townSettings.StartingTownTiles)
        {
            m_townTileData.Add(tileData.TileCoordinate, new()
            {
                IsCaptured = true,
                ConnectedTowerID = tileData.ConnectingTowerID != string.Empty ? Guid.Parse(tileData.ConnectingTowerID) : Guid.Empty
            });
        }
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

    public void SetTileCapture(string coordinates, bool isCaptured)
    {
        if (m_townTileData.ContainsKey(coordinates))
        {
            m_townTileData[coordinates].IsCaptured = isCaptured;
        }
        else
        {
            m_townTileData.AddOrOverwriteKey(coordinates, new TownTileData()
            {
                IsCaptured = isCaptured,
                ConnectedTowerID = Guid.Empty
            });
        }
    }

    public void SetTileTowerID(string coordinates, Guid towerID)
    {
        if (m_townTileData.ContainsKey(coordinates))
        {
            m_townTileData[coordinates].ConnectedTowerID = towerID;
        }
    }

    public bool TryGetTileData(string coordinate, out TownTileData townTileSetup) => m_townTileData.TryGetValue(coordinate, out townTileSetup);

    public int GetTowerTileAmount(Guid towerTypeID) => m_townTileData.Count(x => x.Value.ConnectedTowerID == towerTypeID);

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
}

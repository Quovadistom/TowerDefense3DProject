using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StartingTownTile
{
    public string TileCoordinate;
    public string ConnectingTowerID;
}

public class TownSettings : ScriptableObject
{
    [SerializeField] private List<StartingTownTile> m_startingTileData;

    public List<StartingTownTile> StartingTownTiles => m_startingTileData;
}

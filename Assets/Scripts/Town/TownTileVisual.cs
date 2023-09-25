using System;
using System.Linq;
using UnityEngine;
using Zenject;

public class TownTileVisual : MonoBehaviour
{
    [SerializeField] private Transform[] m_updateLocations = new Transform[4];
    private TownTileService m_townTileService;
    private BoostCollection m_boostCollection;

    [Inject]
    private void Construct(TownTileService townTileService, BoostCollection boostCollection)
    {
        m_townTileService = townTileService;
        m_boostCollection = boostCollection;
    }

    public void SetTileUpgrades(Guid[] updates)
    {
        for (int i = 0; i < updates.Length; i++)
        {
            if (updates[i] != null)
            {
                GameObject visual = m_boostCollection.GameBoostList.FirstOrDefault(boost => boost.Name == updates[i])?.Visual;
                if (visual != null)
                {
                    Instantiate(visual, m_updateLocations[i]);
                }
            }
        }
    }

    public class Factory : PlaceholderFactory<TownTileVisual, TownTileVisual>
    {
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoostAvailabilityService : ServiceSerializationHandler<BoostCollectionServiceDto>
{
    private BoostCollection m_boostCollection;
    private Dictionary<string, int> m_availableBoosts = new();

    protected override Guid Id => Guid.Parse("57dffad0-7783-4183-a0a6-f7d2246c929d");

    public BoostAvailabilityService(BoostCollection boostCollection, SerializationService serializationService, DebugSettings debugSettings) : base(serializationService, debugSettings)
    {
        m_boostCollection = boostCollection;

        if (m_debugSettings.EnableAllBoosts)
        {
            foreach (var boost in m_boostCollection.TowerBoostList)
            {
                AddAvailableBoost(boost.Boost.ID);
            }

            foreach (var boost in m_boostCollection.GameBoostList)
            {
                AddAvailableBoost(boost.Boost.ID);
            }
        }
    }

    public Dictionary<TowerUpgradeBase, int> GetAvailableTowerBoosts() => GetAvailableBoostList(m_boostCollection.TowerBoostList);
    public Dictionary<GameUpgradeBase, int> GetAvailableGameBoosts() => GetAvailableBoostList(m_boostCollection.GameBoostList);

    public Dictionary<T, int> GetAvailableBoostList<T>(IReadOnlyList<BoostContainer<T>> boostContainerList) where T : UpgradeBase
    {
        Dictionary<T, int> keyValuePairs = new();

        foreach (var boost in m_availableBoosts)
        {
            if (TryGetBoost(boost.Key, boostContainerList, out T towerBoostBase))
            {
                keyValuePairs.Add(towerBoostBase, boost.Value);
            }
        }

        return keyValuePairs;
    }

    public bool TryGetTowerBoost(string id, out TowerUpgradeBase gameUpgradeBase) => TryGetBoost(id, m_boostCollection.TowerBoostList, out gameUpgradeBase);
    public bool TryGetGameBoost(string id, out GameUpgradeBase gameUpgradeBase) => TryGetBoost(id, m_boostCollection.GameBoostList, out gameUpgradeBase);

    public bool TryGetBoost<T>(string id, IReadOnlyList<BoostContainer<T>> boostContainerList, out T boost) where T : UpgradeBase
    {
        boost = null;

        if (!string.IsNullOrEmpty(id))
        {
            boost = boostContainerList.FirstOrDefault(boost => boost.Boost.ID == id)?.Boost;
        }

        return boost != null;
    }

    public void AddAvailableBoost(string boostID)
    {
        if (m_availableBoosts.ContainsKey(boostID))
        {
            m_availableBoosts[boostID]++;
        }
        else
        {
            m_availableBoosts.Add(boostID, 1);
        }
    }

    public void RemoveAvailableBoost(string boostID)
    {
        if (m_availableBoosts.ContainsKey(boostID))
        {
            m_availableBoosts[boostID]--;
        }
        else
        {
            Debug.LogWarning($"Could not remove boost with ID {boostID}");
        }
    }

    protected override void ConvertDto()
    {
        Dto.AvailableBoosts = m_availableBoosts;
    }

    protected override void ConvertDtoBack(BoostCollectionServiceDto dto)
    {
        m_availableBoosts = Dto.AvailableBoosts;
    }
}

public class BoostCollectionServiceDto
{
    public Dictionary<string, int> AvailableBoosts;
}

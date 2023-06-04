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
                AddAvailableBoost(boost.Name);
            }
        }
    }

    public Dictionary<BoostContainer, int> GetAvailableBoostList()
    {
        Dictionary<BoostContainer, int> keyValuePairs = new();

        foreach (var boost in m_availableBoosts.Where(x => x.Value > 0))
        {
            if (TryGetBoost(boost.Key, out BoostContainer towerBoostBase))
            {
                keyValuePairs.Add(towerBoostBase, boost.Value);
            }
        }

        return keyValuePairs;
    }

    public bool TryGetBoost(string name, out BoostContainer boost)
    {
        boost = null;

        if (!string.IsNullOrEmpty(name))
        {
            boost = m_boostCollection.TowerBoostList.FirstOrDefault(boost => boost.Name == name);
        }

        return boost != null;
    }

    public void AddAvailableBoost(string boostID)
    {
        if (string.IsNullOrEmpty(boostID))
        {
            return;
        }

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

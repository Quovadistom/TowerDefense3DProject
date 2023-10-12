using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoostAvailabilityService : ServiceSerializationHandler<BoostCollectionServiceDto>
{
    private BoostCollection m_boostCollection;
    private Dictionary<BoostContainer, int> m_availableBoosts = new();

    protected override Guid Id => Guid.Parse("57dffad0-7783-4183-a0a6-f7d2246c929d");

    public BoostAvailabilityService(BoostCollection boostCollection, SerializationService serializationService, DebugSettings debugSettings) : base(serializationService, debugSettings)
    {
        m_boostCollection = boostCollection;

        if (m_debugSettings.EnableAllBoosts)
        {
            foreach (var boost in m_boostCollection.BoostList)
            {
                AddAvailableBoost(boost);
            }
        }
    }

    public Dictionary<BoostContainer, int> GetAvailableBoostList()
    {
        Dictionary<BoostContainer, int> keyValuePairs = (Dictionary<BoostContainer, int>)m_availableBoosts.Where(x => x.Value > 0);

        return keyValuePairs;
    }

    public bool TryGetBoost(Guid id, out BoostContainer boost)
    {
        boost = null;

        if (id != Guid.Empty)
        {
            boost = m_boostCollection.BoostList.FirstOrDefault(boost => boost.ID == id);
        }

        return boost != null;
    }

    public void AddAvailableBoost(BoostContainer boostContainer)
    {
        if (m_availableBoosts.Keys.Contains(boostContainer))
        {
            m_availableBoosts[boostContainer]++;
        }
        else
        {
            m_availableBoosts.Add(boostContainer, 1);
        }
    }

    public void RemoveAvailableBoost(BoostContainer boostContainer)
    {
        if (m_availableBoosts.ContainsKey(boostContainer))
        {
            m_availableBoosts[boostContainer]--;
        }
        else
        {
            Debug.LogWarning($"Could not remove boost with ID {boostContainer.ID}");
        }
    }

    public Dictionary<BoostContainer, int> GetBoostsForComponentParent(ComponentParent componentParent, BoostType boostType)
    {
        return m_availableBoosts.Where(boost => boost.Key.BoostType == boostType && boost.Key.IsBoostSuitable(componentParent)).ToDictionary(x => x.Key, x => x.Value);
    }

    protected override void ConvertDto()
    {
        Dto.AvailableBoosts = m_availableBoosts.ToDictionary(boost => boost.Key.ID, boost => boost.Value);
    }

    protected override void ConvertDtoBack(BoostCollectionServiceDto dto)
    {
        foreach (KeyValuePair<Guid, int> pair in dto.AvailableBoosts)
        {
            if (TryGetBoost(pair.Key, out BoostContainer boost))
            {
                m_availableBoosts.Add(boost, pair.Value);
            }
        }
    }
}

public class BoostCollectionServiceDto
{
    public Dictionary<Guid, int> AvailableBoosts;
}

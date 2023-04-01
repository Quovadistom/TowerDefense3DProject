using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoostAvailabilityService : ServiceSerializationHandler<BoostCollectionServiceDto>
{
    private BoostCollection m_boostCollection;
    private Dictionary<string, int> m_availableBoosts = new();

    public IReadOnlyDictionary<string, int> AvailableBoosts => m_availableBoosts;

    protected override Guid Id => Guid.Parse("57dffad0-7783-4183-a0a6-f7d2246c929d");

    public BoostAvailabilityService(BoostCollection boostCollection, SerializationService serializationService) : base(serializationService)
    {
        m_boostCollection = boostCollection;
    }

    public bool TryGetTowerBoostInformation(string id, out TowerUpgradeBase towerBoostBase)
    {
        towerBoostBase = null;
        if (!string.IsNullOrEmpty(id))
        {
            towerBoostBase = m_boostCollection.TowerBoostList.FirstOrDefault(boost => boost.Boost.ID == id)?.Boost;
        }

        return towerBoostBase != null;
    }

    public bool TryGetGameBoostInformation(string id, out GameUpgradeBase towerBoostBase)
    {
        towerBoostBase = null;

        if (!string.IsNullOrEmpty(id))
        {
            towerBoostBase = m_boostCollection.GameBoostList.FirstOrDefault(boost => boost.Boost.ID == id)?.Boost;
        }

        return towerBoostBase != null;
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

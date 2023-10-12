using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnhancementAvailabilityService : ServiceSerializationHandler<EnhancementCollectionServiceDto>
{
    private EnhancementCollection m_enhancementCollection;
    private Dictionary<EnhancementContainer, int> m_availableEnhancements = new();

    protected override Guid Id => Guid.Parse("57dffad0-7783-4183-a0a6-f7d2246c929d");

    public EnhancementAvailabilityService(EnhancementCollection enhancementCollection, SerializationService serializationService, DebugSettings debugSettings) : base(serializationService, debugSettings)
    {
        m_enhancementCollection = enhancementCollection;

        if (m_debugSettings.EnableAllEnhancements)
        {
            foreach (var enhancement in m_enhancementCollection.EnhancementList)
            {
                AddAvailableEnhancement(enhancement);
            }
        }
    }

    public Dictionary<EnhancementContainer, int> GetAvailableEnhancementList()
    {
        Dictionary<EnhancementContainer, int> keyValuePairs = (Dictionary<EnhancementContainer, int>)m_availableEnhancements.Where(x => x.Value > 0);

        return keyValuePairs;
    }

    public bool TryGetEnhancement(Guid id, out EnhancementContainer enhancement)
    {
        enhancement = null;

        if (id != Guid.Empty)
        {
            enhancement = m_enhancementCollection.EnhancementList.FirstOrDefault(enhancement => enhancement.ID == id);
        }

        return enhancement != null;
    }

    public void AddAvailableEnhancement(EnhancementContainer enhancementContainer)
    {
        if (m_availableEnhancements.Keys.Contains(enhancementContainer))
        {
            m_availableEnhancements[enhancementContainer]++;
        }
        else
        {
            m_availableEnhancements.Add(enhancementContainer, 1);
        }
    }

    public void RemoveAvailableEnhancement(EnhancementContainer enhancementContainer)
    {
        if (m_availableEnhancements.ContainsKey(enhancementContainer))
        {
            m_availableEnhancements[enhancementContainer]--;
        }
        else
        {
            Debug.LogWarning($"Could not remove enhancement with ID {enhancementContainer.ID}");
        }
    }

    public Dictionary<EnhancementContainer, int> GetEnhancementsForComponentParent(ComponentParent componentParent, EnhancementType enhancementType)
    {
        return m_availableEnhancements.Where(enhancement => enhancement.Key.EnhancementType == enhancementType && enhancement.Key.IsEnhancementSuitable(componentParent)).ToDictionary(x => x.Key, x => x.Value);
    }

    protected override void ConvertDto()
    {
        Dto.AvailableEnhancements = m_availableEnhancements.ToDictionary(enhancement => enhancement.Key.ID, enhancement => enhancement.Value);
    }

    protected override void ConvertDtoBack(EnhancementCollectionServiceDto dto)
    {
        foreach (KeyValuePair<Guid, int> pair in dto.AvailableEnhancements)
        {
            if (TryGetEnhancement(pair.Key, out EnhancementContainer enhancement))
            {
                m_availableEnhancements.Add(enhancement, pair.Value);
            }
        }
    }
}

public class EnhancementCollectionServiceDto
{
    public Dictionary<Guid, int> AvailableEnhancements;
}

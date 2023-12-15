using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourcesChangeEventArgs : EventArgs
{
    public ResourcesChangeEventArgs(Resource resource, int newAmount)
    {
        Resource = resource;
        NewAmount = newAmount;
    }

    public Resource Resource { get; private set; }
    public int NewAmount { get; private set; }
}

public class ResourceService : ServiceSerializationHandler<ResourceCollectionServiceDto>
{
    private ResourceCollection m_resourceCollection;
    private Dictionary<Resource, int> m_availableResources = new();

    public IReadOnlyDictionary<Resource, int> AvailableResources => m_availableResources;

    public event EventHandler<ResourcesChangeEventArgs> ResourceChanged;

    public ResourceService(ResourceCollection resourceCollection,
        SerializationService serializationService,
        DebugSettings debugSettings) : base(serializationService, debugSettings)
    {
        m_resourceCollection = resourceCollection;

        foreach (var resource in m_resourceCollection.ResourceList)
        {
            int resourceAmount = m_debugSettings.EnableAllResources ? 99999 : resource.StartingAmount;
            m_availableResources.Add(resource, resourceAmount);
        }
    }

    public int GetAvailableResourceAmount<T>()
    {
        Resource resource = m_availableResources.Keys.FirstOrDefault(key => key.GetType() == typeof(T));

        return resource != null ? m_availableResources[resource] : 0;
    }

    /// <summary>
    /// Changes the amount of a specific resource type of which only one exists that inherits from Resource (e.g. 'BattleFunds.cs').
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="amount"></param>
    public void ChangeAvailableResource<T>(int amount) where T : Resource
    {
        Resource resource = m_availableResources.Keys.FirstOrDefault(key => key.GetType() == typeof(T));

        ChangeAvailableResource(resource, amount);
    }

    /// <summary>
    /// Takes in a resource and an amount. Positive amounts adds resources, negative amounts remove resources.
    /// </summary>
    /// <param name="resource"></param>
    /// <param name="amount"></param>
    public void ChangeAvailableResource(Resource resource, int amount = 1)
    {
        if (m_availableResources.Keys.Contains(resource))
        {
            m_availableResources[resource] += amount;
            ResourceChanged?.Invoke(this, new ResourcesChangeEventArgs(resource, m_availableResources[resource]));
        }
        else
        {
            Debug.LogWarning($"The resource {resource.Name} can not be found in the cached resources list!");
        }
    }

    protected override Guid Id => Guid.Parse("57dffad0-7783-4183-a0a6-f7d2246c929d");

    protected override void ConvertDto()
    {
        Dto.AvailableResources = m_availableResources.Where(pair => pair.Value != 0).ToDictionary(resource => resource.Key.ID, resource => resource.Value);
    }

    protected override void ConvertDtoBack(ResourceCollectionServiceDto dto)
    {
        foreach (KeyValuePair<Guid, int> pair in dto.AvailableResources)
        {
            if (m_resourceCollection.TryGetResource(pair.Key, out Resource resource))
            {
                m_availableResources[resource] = pair.Value;
            }
        }
    }
}

public class ResourceCollectionServiceDto
{
    public Dictionary<Guid, int> AvailableResources;
}

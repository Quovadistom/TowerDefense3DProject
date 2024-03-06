using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ResourceAmountContainer
{
    [SerializeField] private Resource m_requiredResource;
    [SerializeField] private int m_requiredResourceCount = 1;

    public Resource RequiredResource => m_requiredResource;
    public int RequiredResourceCount => m_requiredResourceCount;
}

[Serializable]
public class Blueprint
{
    [SerializeField] private BlueprintType m_blueprintType;
    [SerializeField] private string m_name;
    [SerializeField] private SerializableGuid m_id;
    [SerializeField] private GameObject m_visual;
    [SerializeField] private ModuleModificationBase[] m_modifications;
    [SerializeField] private ResourceAmountContainer[] m_requiredResources;

    public BlueprintType BlueprintType => m_blueprintType;
    public string Name => m_name;
    public Guid ID => m_id;
    public GameObject Visual => m_visual;
    public IReadOnlyCollection<ModuleModificationBase> Modifications => m_modifications;
    public IReadOnlyDictionary<Resource, int> RequiredResources => m_requiredResources.ToDictionary(key => key.RequiredResource, value => value.RequiredResourceCount);

    public Guid TargetObjectID { get; set; } = Guid.Empty;
    public bool IsUnlocked { get; set; }
    public bool IsViewed { get; set; }

    public bool IsBlueprintSuitable(ModuleParent moduleParent)
    {
        return IsUnlocked && m_modifications.All(x => x.IsObjectSuitable(moduleParent));
    }

    public bool CanBuyBlueprint(IReadOnlyDictionary<Resource, int> availableResources)
    {
        bool canBuy = m_requiredResources.Count() == 0 || m_requiredResources.Select(resource => resource.RequiredResourceCount).Sum() == 0;

        foreach (ResourceAmountContainer resourceBlueprintContainer in m_requiredResources)
        {
            if (availableResources.TryGetValue(resourceBlueprintContainer.RequiredResource, out var count))
            {
                canBuy = count >= resourceBlueprintContainer.RequiredResourceCount;
            }
        }

        return canBuy;
    }

    public void ApplyBlueprint(ModuleParent moduleParent)
    {
        foreach (ModuleModificationBase modification in m_modifications)
        {
            modification.TryApplyModification(moduleParent);
        }
    }
}

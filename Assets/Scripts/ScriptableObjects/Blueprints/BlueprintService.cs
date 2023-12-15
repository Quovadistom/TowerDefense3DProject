using System;
using System.Collections.Generic;
using System.Linq;

public class BlueprintService : ServiceSerializationHandler<BlueprintDTO>
{
    private ResourceService m_resourceService;
    private ModuleModificationService m_moduleModificationService;
    private Dictionary<Guid, Blueprint> m_blueprints;

    public BlueprintService(BlueprintCollection blueprintCollection, ResourceService resourceService, ModuleModificationService moduleModificationService,
        SerializationService serializationService, DebugSettings debugSettings) : base(serializationService, debugSettings)
    {
        m_resourceService = resourceService;
        m_moduleModificationService = moduleModificationService;

        m_blueprints = blueprintCollection.Blueprints.ToDictionary(blueprint => blueprint.ID, blueprint => blueprint);

        if (debugSettings.EnableAllBlueprints)
        {
            foreach (var blueprint in blueprintCollection.Blueprints)
            {
                blueprint.IsUnlocked = true;
            }
        }
    }

    public bool TryGetBlueprint(Guid id, out Blueprint blueprint)
    {
        blueprint = m_blueprints[id];
        return blueprint != null;
    }

    public IEnumerable<Blueprint> GetSuitableBlueprints(ModuleParent moduleParent, BlueprintType blueprintType)
    {
        return m_blueprints.Values.Where(blueprint => blueprint.BlueprintType == blueprintType && blueprint.IsBlueprintSuitable(moduleParent));
    }

    public bool CanBuyBlueprint(Blueprint blueprint) => blueprint.CanBuyBlueprint(m_resourceService.AvailableResources);

    public void BuyBlueprint(Blueprint blueprint)
    {
        m_resourceService.RemoveAvailableResources(blueprint.RequiredResources);

        m_moduleModificationService.AddBlueprint(blueprint);
    }

    public void SellBlueprint(Blueprint blueprint)
    {
        m_resourceService.AddAvailableResources(blueprint.RequiredResources);

        m_moduleModificationService.RemoveBlueprint(blueprint);
    }

    protected override Guid Id => Guid.Parse("5b7567f1-bb81-45cb-8ba9-f689ab3fbb7d");

    protected override void ConvertDto()
    {
        Dto.UnlockedBlueprints = m_blueprints.ToDictionary(blueprint => blueprint.Key, blueprint => blueprint.Value.IsUnlocked);
        Dto.ViewedBlueprints = m_blueprints.ToDictionary(blueprint => blueprint.Key, blueprint => blueprint.Value.IsViewed);
    }

    protected override void ConvertDtoBack(BlueprintDTO dto)
    {
        foreach (var blueprint in m_blueprints)
        {
            if (dto.UnlockedBlueprints.TryGetValue(blueprint.Key, out bool isUnlocked))
            {
                blueprint.Value.IsUnlocked = isUnlocked;
            }
            if (dto.ViewedBlueprints.TryGetValue(blueprint.Key, out bool viewed))
            {
                blueprint.Value.IsViewed = viewed;
            }
        }
    }
}

[Serializable]
public class BlueprintDTO
{
    public Dictionary<Guid, bool> UnlockedBlueprints { get; set; }
    public Dictionary<Guid, bool> ViewedBlueprints { get; set; }
}

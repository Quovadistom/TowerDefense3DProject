using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ModificationAvailabilityService : ServiceSerializationHandler<ModificationCollectionServiceDto>
{
    private ModificationCollection m_modificationCollection;
    private Dictionary<ModificationContainer, int> m_availableModifications = new();

    public ModificationAvailabilityService(ModificationCollection modificationCollection,
        ModuleModificationService modificationService,
        SerializationService serializationService,
        DebugSettings debugSettings) : base(serializationService, debugSettings)
    {
        m_modificationCollection = modificationCollection;

        if (m_debugSettings.EnableAllModifications)
        {
            foreach (var modification in m_modificationCollection.ModificationList)
            {
                AddAvailableModification(modification);
            }
        }
    }

    public void AddAvailableModification(ModificationContainer modificationContainer)
    {
        if (m_availableModifications.Keys.Contains(modificationContainer))
        {
            m_availableModifications[modificationContainer]++;
        }
        else
        {
            m_availableModifications.Add(modificationContainer, 1);
        }
    }

    public void RemoveAvailableModification(ModificationContainer modificationContainer)
    {
        if (m_availableModifications.ContainsKey(modificationContainer))
        {
            m_availableModifications[modificationContainer]--;
        }
        else
        {
            Debug.LogWarning($"Could not remove modification with ID {modificationContainer.ID}");
        }
    }

    public Dictionary<ModificationContainer, int> GetModificationsForComponentParent(ModuleParent componentParent, ModificationType modificationType)
    {
        return m_availableModifications.Where(modification => modification.Key.ModificationType == modificationType && modification.Key.IsModificationSuitable(componentParent)).ToDictionary(x => x.Key, x => x.Value);
    }

    protected override Guid Id => Guid.Parse("57dffad0-7783-4183-a0a6-f7d2246c929d");

    protected override void ConvertDto()
    {
        Dto.AvailableModifications = m_availableModifications.ToDictionary(modification => modification.Key.ID, modification => modification.Value);
    }

    protected override void ConvertDtoBack(ModificationCollectionServiceDto dto)
    {
        foreach (KeyValuePair<Guid, int> pair in dto.AvailableModifications)
        {
            if (m_modificationCollection.TryGetModification(pair.Key, out ModificationContainer modification))
            {
                m_availableModifications.Add(modification, pair.Value);
            }
        }
    }
}

public class ModificationCollectionServiceDto
{
    public Dictionary<Guid, int> AvailableModifications;
}

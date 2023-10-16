using System;
using System.Collections.Generic;

public class ModuleModificationService
{
    private Dictionary<ModificationContainer, int> m_modificationsToApplyOnGameLoad = new();
    private ModificationAvailabilityService m_modificationAvailabilityService;

    public event Action<ModuleModificationBase> ModificationReceived;

    public ModuleModificationService(ModificationAvailabilityService modificationAvailabilityService)
    {
        m_modificationAvailabilityService = modificationAvailabilityService;
    }

    public void AddModification(ModificationContainer modification)
    {
        if (m_modificationsToApplyOnGameLoad.ContainsKey(modification))
        {
            m_modificationsToApplyOnGameLoad[modification] += 1;
        }
        else
        {
            m_modificationsToApplyOnGameLoad.Add(modification, 1);
        }

        m_modificationAvailabilityService.RemoveAvailableModification(modification);
    }

    public void RemoveModification(ModificationContainer modification)
    {
        if (m_modificationsToApplyOnGameLoad.ContainsKey(modification))
        {
            m_modificationsToApplyOnGameLoad[modification] -= 1;
            m_modificationAvailabilityService.AddAvailableModification(modification);
        }
    }

    public void ApplyModificationsToObject(ModuleParent componentParent)
    {
        foreach (KeyValuePair<ModificationContainer, int> keyValuePair in m_modificationsToApplyOnGameLoad)
        {
            if (keyValuePair.Key.IsModificationSuitable(componentParent) &&
                (keyValuePair.Key.TargetObjectID == componentParent.ID ||
                keyValuePair.Key.TargetObjectID == Guid.Empty))
            {
                for (int i = 0; i <= keyValuePair.Value; i++)
                {
                    keyValuePair.Key.ApplyModifications(componentParent);
                }
            }
        }
    }

    public void SendModification(ModuleModificationBase modificationContainer)
    {
        ModificationReceived?.Invoke(modificationContainer);
    }
}

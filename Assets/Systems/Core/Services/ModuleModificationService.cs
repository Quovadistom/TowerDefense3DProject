using System;
using System.Collections.Generic;

public class ModuleModificationService
{
    private Dictionary<Blueprint, int> m_modificationsToApplyOnGameLoad = new();

    public event Action<ModuleModificationBase> ModificationReceived;

    public void AddBlueprint(Blueprint modification)
    {
        if (m_modificationsToApplyOnGameLoad.ContainsKey(modification))
        {
            m_modificationsToApplyOnGameLoad[modification] += 1;
        }
        else
        {
            m_modificationsToApplyOnGameLoad.Add(modification, 1);
        }
    }

    public void RemoveBlueprint(Blueprint modification)
    {
        if (m_modificationsToApplyOnGameLoad.ContainsKey(modification))
        {
            m_modificationsToApplyOnGameLoad[modification] -= 1;
        }
    }

    public void ApplyBlueprintsToObject(ModuleParent componentParent)
    {
        foreach (KeyValuePair<Blueprint, int> keyValuePair in m_modificationsToApplyOnGameLoad)
        {
            if (keyValuePair.Key.IsBlueprintSuitable(componentParent) &&
                (keyValuePair.Key.TargetObjectID == componentParent.ID ||
                keyValuePair.Key.TargetObjectID == Guid.Empty))
            {
                for (int i = 0; i <= keyValuePair.Value; i++)
                {
                    keyValuePair.Key.ApplyBlueprint(componentParent);
                }
            }
        }
    }

    public void SendModification(ModuleModificationBase modificationContainer)
    {
        ModificationReceived?.Invoke(modificationContainer);
    }
}

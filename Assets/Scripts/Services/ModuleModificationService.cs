using System;
using System.Collections.Generic;

public class ModuleModificationService
{
    private Dictionary<ModificationContainer, int> m_modificationsToApplyOnGameLoad = new();

    public event Action<ModuleModificationBase> ModificationReceived;

    public void AddModification(ModificationContainer modificationID)
    {
        if (m_modificationsToApplyOnGameLoad.ContainsKey(modificationID))
        {
            m_modificationsToApplyOnGameLoad[modificationID] += 1;
        }
        else
        {
            m_modificationsToApplyOnGameLoad.Add(modificationID, 1);
        }
    }

    public void RemoveModification(ModificationContainer modificationID)
    {
        if (m_modificationsToApplyOnGameLoad.ContainsKey(modificationID))
        {
            m_modificationsToApplyOnGameLoad[modificationID] -= 1;
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

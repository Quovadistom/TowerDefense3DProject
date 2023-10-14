using System;
using System.Collections.Generic;

public class ModuleModificationService
{
    private Dictionary<EnhancementContainer, int> m_enhancementsToApplyOnGameLoad = new();
    private EnhancementCollection m_enhancementCollection;

    public event Action<ModuleModificationBase> UpgradeReceived;

    public ModuleModificationService(EnhancementCollection enhancementCollection)
    {
        m_enhancementCollection = enhancementCollection;
    }

    public void AddEnhancement(EnhancementContainer upgradeID)
    {
        if (m_enhancementsToApplyOnGameLoad.ContainsKey(upgradeID))
        {
            m_enhancementsToApplyOnGameLoad[upgradeID] += 1;
        }
        else
        {
            m_enhancementsToApplyOnGameLoad.Add(upgradeID, 1);
        }
    }

    public void RemoveEnhancement(EnhancementContainer upgradeID)
    {
        if (m_enhancementsToApplyOnGameLoad.ContainsKey(upgradeID))
        {
            m_enhancementsToApplyOnGameLoad[upgradeID] -= 1;
        }
    }

    public void ApplyEnhancementsToObject(ModuleParent componentParent)
    {
        foreach (KeyValuePair<EnhancementContainer, int> keyValuePair in m_enhancementsToApplyOnGameLoad)
        {
            if (keyValuePair.Key.IsEnhancementSuitable(componentParent) &&
                (keyValuePair.Key.TargetObjectID == componentParent.ID ||
                keyValuePair.Key.TargetObjectID == Guid.Empty))
            {
                for (int i = 0; i <= keyValuePair.Value; i++)
                {
                    keyValuePair.Key.ApplyUpgrades(componentParent);
                }
            }
        }
    }

    public void SendEnhancement(ModuleModificationBase upgradeContainer)
    {
        UpgradeReceived?.Invoke(upgradeContainer);
    }
}

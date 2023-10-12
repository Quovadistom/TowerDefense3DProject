using System;
using System.Collections.Generic;

public class EnhancementService
{
    private Dictionary<Guid, int> m_upgradesToApplyOnGameLoad = new();
    private EnhancementAvailabilityService m_enhancementAvailabilityService;

    public event Action<UpgradeBase> UpgradeReceived;

    public EnhancementService(EnhancementAvailabilityService enhancementAvailabilityService)
    {
        m_enhancementAvailabilityService = enhancementAvailabilityService;
    }

    public void AddUpgrade(Guid upgradeID)
    {
        if (m_upgradesToApplyOnGameLoad.ContainsKey(upgradeID))
        {
            m_upgradesToApplyOnGameLoad[upgradeID] += 1;
        }
        else
        {
            m_upgradesToApplyOnGameLoad.Add(upgradeID, 1);
        }
    }

    public void RemoveUpgrade(Guid upgradeID)
    {
        if (m_upgradesToApplyOnGameLoad.ContainsKey(upgradeID))
        {
            m_upgradesToApplyOnGameLoad[upgradeID] -= 1;
        }
    }

    public void ApplyUpgradesToObject(ComponentParent componentParent)
    {
        foreach (KeyValuePair<Guid, int> keyValuePair in m_upgradesToApplyOnGameLoad)
        {
            if (m_enhancementAvailabilityService.TryGetEnhancement(keyValuePair.Key, out var enhancement) &&
                enhancement.IsEnhancementSuitable(componentParent) &&
                (enhancement.TargetObjectID == componentParent.ID ||
                enhancement.TargetObjectID == Guid.Empty))
            {
                for (int i = 0; i <= keyValuePair.Value; i++)
                {
                    enhancement.ApplyUpgrades(componentParent);
                }
            }
        }
    }

    public void SendEnhancement(UpgradeBase upgradeContainer)
    {
        UpgradeReceived?.Invoke(upgradeContainer);
    }
}

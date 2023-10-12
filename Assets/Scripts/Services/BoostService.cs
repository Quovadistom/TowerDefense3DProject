using System;
using System.Collections.Generic;

public class BoostService
{
    private Dictionary<Guid, int> m_upgradesToApplyOnGameLoad = new();
    private BoostAvailabilityService m_boostAvailabilityService;

    public event Action<UpgradeBase> UpgradeReceived;

    public BoostService(BoostAvailabilityService boostAvailabilityService)
    {
        m_boostAvailabilityService = boostAvailabilityService;
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
            if (m_boostAvailabilityService.TryGetBoost(keyValuePair.Key, out var boost) &&
                boost.IsBoostSuitable(componentParent) &&
                (boost.TargetObjectID == componentParent.ID ||
                boost.TargetObjectID == Guid.Empty))
            {
                for (int i = 0; i <= keyValuePair.Value; i++)
                {
                    boost.ApplyUpgrades(componentParent);
                }
            }
        }
    }

    public void SendBoost(UpgradeBase upgradeContainer)
    {
        UpgradeReceived?.Invoke(upgradeContainer);
    }
}

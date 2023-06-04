
using System;
using System.Collections.Generic;
using System.Linq;

public class BoostService
{
    private List<BoostContainer> m_upgradesToApplyOnGameLoad = new();

    public event Action<UpgradeBase> UpgradeReceived;

    public void AddUpgrade(BoostContainer upgrade)
    {
        m_upgradesToApplyOnGameLoad.Add(upgrade);
    }

    public void RemoveUpgrade(BoostContainer upgrade)
    {
        m_upgradesToApplyOnGameLoad.Remove(upgrade);
    }

    public void ApplyUpgradesToObject(ComponentParent componentParent)
    {
        var boost1 = m_upgradesToApplyOnGameLoad.Where(upgrade =>
        string.IsNullOrEmpty(upgrade.TargetObjectID) || upgrade.TargetObjectID == componentParent.ComponentID).ToList();

        var boost2 = m_upgradesToApplyOnGameLoad.Where(upgrade =>
        upgrade.IsBoostSuitable(componentParent)).Count();

        foreach (BoostContainer upgrade in m_upgradesToApplyOnGameLoad.Where(upgrade =>
        (string.IsNullOrEmpty(upgrade.TargetObjectID) || upgrade.TargetObjectID == componentParent.ComponentID) &&
        upgrade.IsBoostSuitable(componentParent)))
        {
            upgrade.ApplyUpgrades(componentParent);
        }
    }

    internal void SendBoost(UpgradeBase upgradeContainer)
    {
        UpgradeReceived?.Invoke(upgradeContainer);
    }
}

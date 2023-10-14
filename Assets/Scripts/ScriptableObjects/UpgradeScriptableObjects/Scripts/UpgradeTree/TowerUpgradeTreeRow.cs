using System;
using System.Collections.Generic;

[Serializable]
public class TowerUpgradeTreeRow
{
    public List<TowerUpgradeData> TowerUpgrades;

    public void CopyTreeData(TowerUpgradeTreeData treeToCopy, TowerModule towerInfoComponent)
    {
        foreach (TowerUpgradeData towerUpgradeData in TowerUpgrades)
        {
            towerUpgradeData.CopyTreeData(treeToCopy, towerInfoComponent);
        }
    }
}


using System;
using System.Collections.Generic;

[Serializable]
public class TowerUpgradeTreeRow
{
    public List<TowerUpgradeData> TowerUpgrades;

    public void CopyTreeData(TowerUpgradeTreeData treeToCopy, TowerInfoComponent towerInfoComponent)
    {
        foreach (TowerUpgradeData towerUpgradeData in TowerUpgrades)
        {
            towerUpgradeData.CopyTreeData(treeToCopy, towerInfoComponent);
        }
    }
}


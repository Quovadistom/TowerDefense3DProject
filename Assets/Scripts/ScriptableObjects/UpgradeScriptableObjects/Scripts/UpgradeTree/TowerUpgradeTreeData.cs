using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class TowerUpgradeTreeData
{
    public List<TowerUpgradeTreeRow> Structure;

    public void Initialize()
    {
        foreach (TowerUpgradeTreeRow towerUpgradeTreeRow in Structure)
        {
            for (int i = 0; i < towerUpgradeTreeRow.TowerUpgrades.Count; i++)
            {
                TowerUpgradeData towerUpgradeData = towerUpgradeTreeRow.TowerUpgrades[i];

                if (i + 1 < towerUpgradeTreeRow.TowerUpgrades.Count)
                {
                    towerUpgradeData.RequiredFor.Add(towerUpgradeTreeRow.TowerUpgrades[i + 1].TowerUpgradeID.ToString());
                }

                foreach (TowerUpgradeData upgradeData in GetTowerUpgradesDatas(towerUpgradeData.RequiredFor.Select(id => Guid.Parse(id))))
                {
                    upgradeData.UnlockSignals++;
                }
            }
        }
    }

    public void CopyTreeData(TowerUpgradeTreeData treeToCopy, TowerModule towerInfoComponent)
    {
        Initialize();
        foreach (TowerUpgradeTreeRow towerUpgradeTreeRow in Structure)
        {
            towerUpgradeTreeRow.CopyTreeData(treeToCopy, towerInfoComponent);
        }
    }

    public bool TryGetTowerUpgradeData(Guid ID, out TowerUpgradeData data)
    {
        data = null;

        foreach (TowerUpgradeTreeRow row in Structure)
        {
            data = row.TowerUpgrades.FirstOrDefault(upgrade => upgrade.TowerUpgradeID == ID);
            if (data != null)
            {
                return true;
            }
        }

        return data != null;
    }

    public IEnumerable<TowerUpgradeData> GetTowerUpgradesDatas(IEnumerable<Guid> ids)
    {
        List<TowerUpgradeData> towerUpgradeDatas = new();

        foreach (Guid id in ids)
        {
            if (TryGetTowerUpgradeData(id, out TowerUpgradeData towerUpgradeData))
            {
                towerUpgradeDatas.Add(towerUpgradeData);
            }
        }

        return towerUpgradeDatas;
    }
}


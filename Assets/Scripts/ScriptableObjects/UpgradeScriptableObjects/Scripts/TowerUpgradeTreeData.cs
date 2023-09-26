using NaughtyAttributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class TowerUpgradeData
{
    public string Name;
    public int UpgradeCost = 100;
    public bool IsBought = false;
    public List<string> RequiredFor = new();

    [JsonIgnore][Expandable] public UpgradeBase[] TowerUpgrades;

    private int m_unlockSignals;
    public int UnlockSignals
    {
        get => m_unlockSignals;
        set
        {
            m_unlockSignals = value;
            UnlockSignalsChanged?.Invoke(m_unlockSignals == 0);
        }
    }

    public event Action<bool> UnlockSignalsChanged;

    public void CopyTreeData(TowerUpgradeTreeData treeToCopy, TowerInfoComponent towerInfoComponent)
    {
        if (treeToCopy.TryGetTowerUpgradeData(Name, out TowerUpgradeData towerUpgradeData))
        {
            IsBought = towerUpgradeData.IsBought;
            UnlockSignals = towerUpgradeData.UnlockSignals;

            if (IsBought)
            {
                ApplyUpgrades(towerInfoComponent);
            }
        }
    }

    public void ApplyUpgrades(TowerInfoComponent towerInfoComponent)
    {
        foreach (UpgradeBase upgradeData in TowerUpgrades)
        {
            upgradeData.TryApplyUpgrade(towerInfoComponent);
        }
    }
}

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
                    towerUpgradeData.RequiredFor.Add(towerUpgradeTreeRow.TowerUpgrades[i + 1].Name);
                }

                foreach (TowerUpgradeData upgradeData in GetTowerUpgradesDatas(towerUpgradeData.RequiredFor))
                {
                    upgradeData.UnlockSignals++;
                }
            }
        }
    }

    public void CopyTreeData(TowerUpgradeTreeData treeToCopy, TowerInfoComponent towerInfoComponent)
    {
        Initialize();
        foreach (TowerUpgradeTreeRow towerUpgradeTreeRow in Structure)
        {
            towerUpgradeTreeRow.CopyTreeData(treeToCopy, towerInfoComponent);
        }
    }

    public bool TryGetTowerUpgradeData(string ID, out TowerUpgradeData data)
    {
        data = null;

        foreach (TowerUpgradeTreeRow row in Structure)
        {
            data = row.TowerUpgrades.FirstOrDefault(upgrade => upgrade.Name == ID);
            if (data != null)
            {
                return true;
            }
        }

        return data != null;
    }

    public IEnumerable<TowerUpgradeData> GetTowerUpgradesDatas(IEnumerable<string> ids)
    {
        List<TowerUpgradeData> towerUpgradeDatas = new();

        foreach (string id in ids)
        {
            if (TryGetTowerUpgradeData(id, out TowerUpgradeData towerUpgradeData))
            {
                towerUpgradeDatas.Add(towerUpgradeData);
            }
        }

        return towerUpgradeDatas;
    }
}


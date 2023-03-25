using NaughtyAttributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class TowerUpgradeData
{
    [AllowNesting][ReadOnly][SerializeField] private string m_name;
    [AllowNesting][ReadOnly][SerializeField] private string m_id;

    public string ID => m_id;
    public int UpgradeCost = 100;
    public bool IsBought = false;
    [InfoBox("Select the ID of the upgrade(s) this one is required for. The next element in this row is already included!")]
    public List<string> RequiredFor = new();

    [JsonIgnore][AllowNesting][OnValueChanged(nameof(ValidateTower))] public TowerUpgradeBase TowerUpgrade;

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

    public void ValidateTower()
    {
        m_id = TowerUpgrade != null ? TowerUpgrade.ID : "No upgrade added!";
        m_name = TowerUpgrade != null ? TowerUpgrade.Name : string.Empty;
    }

    public void CopyTreeData(TowerUpgradeTreeData treeToCopy, TowerInfoComponent towerInfoComponent)
    {
        if (treeToCopy.TryGetTowerUpgradeData(m_id, out TowerUpgradeData towerUpgradeData))
        {
            IsBought = towerUpgradeData.IsBought;
            UnlockSignals = towerUpgradeData.UnlockSignals;

            if (IsBought)
            {
                TowerUpgrade.TryApplyUpdate(towerInfoComponent);
            }
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
[CreateAssetMenu(fileName = "TowerUpgradeTreeData", menuName = "ScriptableObjects/Upgrades/TowerUpgradeTreeData")]
public class TowerUpgradeTreeData : ScriptableObject
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
                    towerUpgradeData.RequiredFor.Add(towerUpgradeTreeRow.TowerUpgrades[i + 1].ID);
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
            data = row.TowerUpgrades.FirstOrDefault(upgrade => upgrade.ID == ID);
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


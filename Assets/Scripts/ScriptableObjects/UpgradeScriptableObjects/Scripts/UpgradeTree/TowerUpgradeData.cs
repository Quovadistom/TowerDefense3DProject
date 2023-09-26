using NaughtyAttributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TowerUpgradeData
{
    [SerializeField] private SerializableGuid m_towerUpgradeID;
    public string Name;
    public int UpgradeCost = 100;
    public bool IsBought = false;
    public List<string> RequiredFor = new();

    public Guid TowerUpgradeID
    {
        get => m_towerUpgradeID;
        set => m_towerUpgradeID = (SerializableGuid)value;
    }

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
        if (treeToCopy.TryGetTowerUpgradeData(TowerUpgradeID, out TowerUpgradeData towerUpgradeData))
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


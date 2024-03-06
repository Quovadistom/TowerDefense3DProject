using NaughtyAttributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TowerModificationData
{
    [SerializeField] private SerializableGuid m_towerModificationID;
    public string Name;
    public int ModificationCost = 100;
    public bool IsBought = false;
    public List<string> RequiredFor = new();

    public Guid TowerModificationID
    {
        get => m_towerModificationID;
        set => m_towerModificationID = (SerializableGuid)value;
    }

    [JsonIgnore][Expandable] public ModuleModificationBase[] TowerModifications;

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

    public void CopyTreeData(TowerModificationTreeData treeToCopy, TowerModule towerInfoComponent)
    {
        if (treeToCopy.TryGetTowerModificationData(TowerModificationID, out TowerModificationData towerModificationData))
        {
            IsBought = towerModificationData.IsBought;
            UnlockSignals = towerModificationData.UnlockSignals;

            if (IsBought)
            {
                ApplyModifications(towerInfoComponent);
            }
        }
    }

    public void ApplyModifications(TowerModule towerInfoComponent)
    {
        foreach (ModuleModificationBase modificationData in TowerModifications)
        {
            modificationData.TryApplyModification(towerInfoComponent);
        }
    }
}


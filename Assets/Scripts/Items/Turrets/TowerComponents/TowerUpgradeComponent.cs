using System;
using UnityEngine;

public class TowerUpgradeComponent : MonoBehaviour, ITowerComponent
{
    [SerializeField] private int m_towerAvailableUpgradeCount;

    public event Action<int> AvailableUpgradeCountChanged;

    public int TowerAvailableUpgradeCount
    {
        get { return m_towerAvailableUpgradeCount; }
        set
        {
            if (value == m_towerAvailableUpgradeCount)
            {
                return;
            }

            m_towerAvailableUpgradeCount = value;
            AvailableUpgradeCountChanged?.Invoke(m_towerAvailableUpgradeCount);
        }
    }
}

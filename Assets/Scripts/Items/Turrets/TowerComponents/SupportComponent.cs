using System;
using UnityEngine;

[Serializable]
public class SupportComponent : ComponentBase
{
    [SerializeField] private float m_upgradePercentageSingleTower = 100;
    [Range(0, 1)][SerializeField] private float m_sharedTowerFactor = 1;

    public event Action<float> UpgradePercentageUpdated;
    public event Action<float> SharedTowerFactorUpdated;

    public float UpgradePercentage
    {
        get => m_upgradePercentageSingleTower;
        set
        {
            m_upgradePercentageSingleTower = value;
            UpgradePercentageUpdated?.Invoke(m_upgradePercentageSingleTower);
        }
    }

    /// <summary>
    /// Factor that runs from 0 - 1
    /// </summary>
    public float SharedTowerFactor
    {
        get => m_sharedTowerFactor;
        set
        {
            m_sharedTowerFactor = Mathf.Clamp(0, 1, value);
            SharedTowerFactorUpdated?.Invoke(m_sharedTowerFactor);
        }
    }
}

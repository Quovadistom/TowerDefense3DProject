using System;
using UnityEngine;

public class TowerSupportComponent : ChangeVisualComponent
{
    [SerializeField] private float m_upgradePercentageSingleTower = 100;
    [Range(0, 1)][SerializeField] private float m_sharedTowerFactor = 1;

    public event Action<float> UpgradePercentageUpdated;
    public event Action<float> SharedTowerFactorUpdated;

    protected virtual void Start()
    {
        UpgradePercentage = m_upgradePercentageSingleTower;
    }

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
            if (value < 0 || value > 1)
            {
                Debug.LogWarning($"The SharedTowerFactor of {gameObject.name} is {value}, but should be between 0 and 1", this);
            }

            m_sharedTowerFactor = Mathf.Clamp(0, 1, value);
            SharedTowerFactorUpdated?.Invoke(m_sharedTowerFactor);
        }
    }
}

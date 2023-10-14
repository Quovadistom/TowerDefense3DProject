using System;
using UnityEngine;

[Serializable]
public class SupportModule : ModuleBase
{
    public ModuleDataTypeWithEvent<float> UpgradePercentage;

    [Range(0, 1)][SerializeField] private float m_sharedTowerFactor = 1;

    public event Action<float> SharedTowerFactorUpdated;
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

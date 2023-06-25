using Assets.Scripts.Interactables;
using System;
using UnityEngine;

public abstract class TowerFloatSupportHandler<T> : TowerSupportHandler<T> where T : ComponentBase
{
    [SerializeField] private SupportComponent m_towerSupportComponent;

    protected abstract Action<T, float> ComponentFunc { get; }

    private float m_currentBuff = 0;

    protected override void AddTowerBuff(ComponentParent componentParent)
    {
        float newBuff = GetPercentageForOneTower();

        foreach (Selectable tower in m_supportTowerSelector.ConnectedTowers)
        {
            if (tower.GameObjectToSelect.TryGetComponent(out ComponentParent towerComponentParent))
            {
                float buff = towerComponentParent != componentParent ? newBuff - m_currentBuff : newBuff;
                BuffComponent(towerComponentParent, buff);
            }
        }

        m_currentBuff = newBuff;
    }

    protected override void RemoveTowerBuff(ComponentParent componentParent)
    {
        BuffComponent(componentParent, -m_currentBuff);

        float newBuff = GetPercentageForOneTower();

        foreach (Selectable tower in m_supportTowerSelector.ConnectedTowers)
        {
            if (tower.GameObjectToSelect.TryGetComponent(out ComponentParent towerComponentParent) && towerComponentParent != componentParent)
            {
                BuffComponent(towerComponentParent, newBuff - m_currentBuff);
            }
        }

        m_currentBuff = newBuff;
    }

    protected override void ResetConnectedTowers()
    {
        foreach (Selectable tower in m_supportTowerSelector.ConnectedTowers)
        {
            if (tower.GameObjectToSelect.TryGetComponent(out ComponentParent towerComponentParent))
            {
                BuffComponent(towerComponentParent, -m_currentBuff);
            }
        }
    }

    protected void BuffComponent(ComponentParent componentParent, float buffPercentage)
    {
        componentParent.TryFindAndActOnComponent<T>((component) => ComponentFunc?.Invoke(component, buffPercentage));
    }

    private float GetPercentageForOneTower()
    {
        if (m_supportTowerSelector.ConnectedTowerCount == 0)
        {
            return m_towerSupportComponent.UpgradePercentage.Value;
        }

        float totalPercentagePool = m_towerSupportComponent.UpgradePercentage.Value + (m_supportTowerSelector.ConnectedTowerCount - 1) * m_towerSupportComponent.SharedTowerFactor * m_towerSupportComponent.UpgradePercentage.Value;
        return totalPercentagePool * (1 / ((float)m_supportTowerSelector.ConnectedTowerCount));
    }
}

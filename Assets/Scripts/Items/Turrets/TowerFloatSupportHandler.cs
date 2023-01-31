using Assets.Scripts.Interactables;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerFloatSupportHandler<T> : TowerSupportHandler<T> where T : MonoBehaviour
{
    [SerializeField] protected T m_supportComponent;
    [SerializeField] private TowerSupportComponent m_towerSupportComponent;

    private Dictionary<T, float> m_appliedValues = new();

    protected override void RecalculateValues(T removedTower = null)
    {
        base.RecalculateValues(removedTower);

        if (removedTower != null)
        {
            if (m_appliedValues.TryGetValue(removedTower, out float oldPercentage))
            {
                SetFLoat(removedTower, GetFloat(removedTower) - GetFloat(removedTower) * oldPercentage);
                m_appliedValues.Remove(removedTower);
            }
        }

        if (m_connectedTowers.Count > 0)
        {
            float newPercentage = GetPercentageForOneTower(m_towerSupportComponent.UpgradePercentage, m_towerSupportComponent.SharedTowerFactor);

            foreach (Selectable tower in m_connectedTowers)
            {
                T component = tower.GameObjectToSelect.GetComponent<T>();

                if (m_appliedValues.TryGetValue(component, out float oldPercentage))
                {
                    SetFLoat(component, GetFloat(component) - GetFloat(component) * oldPercentage);
                }

                float valueAdded = GetFloat(component).PercentageOf(newPercentage);
                SetFLoat(component, GetFloat(component) + valueAdded);
                float partOfTotal = valueAdded / GetFloat(component);
                m_appliedValues.AddOrOverwriteKey(component, partOfTotal);
            }
        }
    }

    protected override void ResetConnectedTowers()
    {
        foreach (Selectable selectable in m_connectedTowers)
        {
            T component = selectable.GameObjectToSelect.GetComponent<T>();

            if (m_appliedValues.TryGetValue(component, out float oldPercentage))
            {
                SetFLoat(component, GetFloat(component) - GetFloat(component) * oldPercentage);
            }
        }
    }

    protected abstract float GetFloat(T component);
    protected abstract void SetFLoat(T component, float value);

    private float GetPercentageForOneTower(float availablePercentage, float factor)
    {
        float totalPercentagePool = availablePercentage + ((float)ConnectedTowerCount - 1) * factor * availablePercentage;
        return totalPercentagePool * (1 / (float)ConnectedTowerCount);
    }
}

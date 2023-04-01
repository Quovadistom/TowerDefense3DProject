using System;
using UnityEngine;

public abstract class TowerUpgrade<T> : TowerUpgradeBase where T : Component
{
    public override void TryApplyUpgrade(TowerInfoComponent towerInfoComponent)
    {
        if (towerInfoComponent.TryGetComponent(out T turretComponent))
        {
            ApplyUpdate(turretComponent);
        }
        else
        {
            Debug.LogError($"This tower does not contain type {typeof(T)}, unable to apply update!");
        }
    }

    public override Type TowerComponentType => typeof(T);

    protected abstract void ApplyUpdate(T turretComponent);
}


using UnityEngine;

public abstract class TowerUpgrade<T> : TowerUpgradeBase where T : Component
{
    public override void TryApplyUpdate(TowerInfoComponent towerInfoComponent)
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

    protected abstract void ApplyUpdate(T turretComponent);
}


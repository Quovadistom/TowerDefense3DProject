using System;
using UnityEngine;

public class DamageUpgradeTemplate : TowerBoostBase
{
    [SerializeField] private float m_percentage;

    public override Type TowerComponentType => typeof(TurretProjectileComponent);

    public override void ApplyBoost(TowerInfoComponent towerInfoComponent)
    {
        if (towerInfoComponent.TryGetComponent(out TurretProjectileComponent turretProjectileComponent))
        {
            turretProjectileComponent.ProjectileProfile.Damage = turretProjectileComponent.ProjectileProfile.Damage.AddPercentage(m_percentage);
        }
    }
}

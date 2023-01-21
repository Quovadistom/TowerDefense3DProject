using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageBoost", menuName = "ScriptableObjects/Boosts/DamageBoost")]
public class DamageBoost : TowerBoostBase
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

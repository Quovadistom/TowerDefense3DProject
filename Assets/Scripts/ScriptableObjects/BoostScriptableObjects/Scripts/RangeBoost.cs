using System;
using UnityEngine;

[CreateAssetMenu(fileName = "RangeBoost", menuName = "ScriptableObjects/Boosts/RangeBoost")]
public class RangeBoost : TowerBoostBase
{
    [SerializeField] private float m_percentage;

    public override Type TowerComponentType => typeof(TurretRangeComponent);

    public override void ApplyBoost(TowerInfoComponent towerInfoComponent)
    {
        if (towerInfoComponent.TryGetComponent(out TurretRangeComponent turretRangeComponent))
        {
            turretRangeComponent.Range = turretRangeComponent.Range.AddPercentage(m_percentage);
        }
    }
}

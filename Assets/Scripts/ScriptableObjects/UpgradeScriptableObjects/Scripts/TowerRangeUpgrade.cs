using UnityEngine;

[CreateAssetMenu(fileName = "TowerRangeUpgrade", menuName = "ScriptableObjects/Upgrades/TowerRangeUpgrade")]
public class TowerRangeUpgrade : TowerUpgrade<TurretRangeComponent>
{
    [SerializeField] private float m_increasePercentage;

    protected override void ApplyUpdate(TurretRangeComponent turretComponent)
    {
        turretComponent.Range = turretComponent.Range.AddPercentage(m_increasePercentage);
        turretComponent.Visual = m_newVisual;
    }
}


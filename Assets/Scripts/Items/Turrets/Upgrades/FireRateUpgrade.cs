using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRateUpgrade : UpgradeBase<TurretProjectileComponent>
{
    [SerializeField] private float m_decreaseFireRatePercentage;
    [SerializeField] BulletSpawnPoints m_newBulletSpawnPoints;

    protected override void OnUpgradeButtonClicked()
    {
        base.OnUpgradeButtonClicked();

        m_turretMediator.Firerate = m_turretMediator.Firerate.RemovePercentage(Mathf.Abs(m_decreaseFireRatePercentage));
        m_turretMediator.BulletSpawnPoints = m_newBulletSpawnPoints;
    }
}

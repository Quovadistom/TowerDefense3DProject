using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRateUpgrade : UpgradeBase<TurretProjectileComponent>
{
    [SerializeField] private float m_decreaseFireRatePercentage;
    [SerializeField] BulletSpawnPoints m_newBulletSpawnPoints;
    private UpgradeNode m_node;

    private void Awake()
    {
        m_node = GetComponent<UpgradeNode>();
        m_node.ButtonClicked += OnUpgradeButtonClicked;
    }

    private void OnUpgradeButtonClicked()
    {
        m_turretMediator.Firerate = m_turretMediator.Firerate.RemovePercentage(Mathf.Abs(m_decreaseFireRatePercentage));
        m_turretMediator.BulletSpawnPoints = m_newBulletSpawnPoints;
    }
}

using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BarrelBase : Swappable<BarrelBulletSpawnPoints>
{
    [SerializeField] private TurretEnemyHandler m_turret;
    [SerializeField] private TurretData m_turretData;
    [SerializeField] private BarrelBulletSpawnPoints m_bulletSpawnPoints;

    private Transform m_targetTransform;
    private BulletService m_bulletService;

    [Inject]
    public void Construct(BulletService bulletService)
    {
        m_bulletService = bulletService;
    }

    private void Update()
    {
        if (m_turret.IsTargetValid())
        {
            m_targetTransform = m_turret.Target.EnemyMiddle;
            var lookPos = m_targetTransform.position - transform.position;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * m_turretData.TurnSpeed);

            LookAtTarget();
        }
    }

    private void LookAtTarget()
    {
        if (IsLockedOnTarget())
        {
            m_turretData.FiringMethod.Shoot(m_bulletService, m_bulletSpawnPoints.SpawnPoints, m_turret.Target);
        }
    }

    private bool IsLockedOnTarget()
    {
        Vector3 dirFromAtoB = (m_targetTransform.position - transform.position).normalized;
        float dotProd = Vector3.Dot(dirFromAtoB, transform.forward);
        return dotProd >= 0.95;
    }
}

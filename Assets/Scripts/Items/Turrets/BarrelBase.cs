using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BarrelBase : Swappable<BarrelBulletSpawnPoints>
{
    public TurretBase TurretBase;
    [SerializeField] private List<Transform> m_bulletSpawnPoints;

    private Transform m_targetTransform;
    private IReadOnlyList<Transform> m_bulletSpawnPointsList;
    private float m_fireCountdown = 0f;
    private BulletService m_bulletService;

    [Inject]
    public void Construct(BulletService bulletService)
    {
        m_bulletService = bulletService;
    }

    private void OnEnable()
    {
        m_bulletSpawnPointsList = m_bulletSpawnPoints;
    }

    private void Update()
    {
        if (TurretBase.IsTargetValid())
        {
            m_targetTransform = TurretBase.Target.EnemyMiddle;
            var lookPos = m_targetTransform.position - transform.position;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * TurretBase.TurretData.TurnSpeed);

            LookAtTarget();
        }

        m_fireCountdown -= Time.deltaTime;
    }

    private void LookAtTarget()
    {
        if (CanShoot())
        {
            Shoot();
            m_fireCountdown = 1f / TurretBase.TurretData.Firerate;
        }
    }

    private bool CanShoot() => m_fireCountdown <= 0f && IsLockedOnTarget();

    private bool IsLockedOnTarget()
    {
        Vector3 dirFromAtoB = (m_targetTransform.position - transform.position).normalized;
        float dotProd = Vector3.Dot(dirFromAtoB, transform.forward);
        return dotProd >= 0.95;
    }

    private void Shoot()
    {
        foreach (Transform spawnPoint in m_bulletSpawnPointsList)
        {
            m_bulletService.CreateNewBullet(TurretBase.TurretData.BulletPrefab, spawnPoint.position, TurretBase.TurretData.BulletProfile, TurretBase.Target);
        }
    }

    public override void SwapVisuals(BarrelBulletSpawnPoints newVisuals)
    {
        base.SwapVisuals(newVisuals);
        m_bulletSpawnPointsList = newVisuals.SpawnPoints;
    }
}

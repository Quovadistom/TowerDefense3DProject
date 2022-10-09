using System;
using UnityEngine;

[Serializable]
public class BasicTurretData : TurretAndAttackData<IBulletProfile>
{
    public Bullet BulletPrefab;

    private void Awake()
    {
        AttackProfile = new StandardBulletProfile(BulletSpeed, BulletDamage);
        FiringMethod = new ProjectileFiringMethod(this);
    }
}
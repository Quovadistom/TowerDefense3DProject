using System;
using UnityEngine;

[Serializable]
public class BasicTurretData : TurretDataBase<IBulletProfile>
{
    public BasicTurretData()
    {
        BulletProfile = new StandardBulletProfile(BulletSpeed, BulletDamage);
    }
}

public class TurretDataBase<T> where T : IBulletProfile
{
    public float Range = 4;
    public float TurnSpeed = 8;
    public float Firerate = 1f;
    public float BulletSpeed = 20;
    public int BulletDamage = 50;

    public T BulletProfile; 
    public ProjectileBase<T> BulletPrefab;
}

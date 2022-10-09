public class TurretAndAttackData<T> : TurretData where T : IAttackProfileBase
{
    public float Firerate = 1f;
    public float BulletSpeed = 20;
    public float BulletDamage = 50;

    public T AttackProfile;
}

// laser
// range, turnspeed, firerate/damagerate, damage, pierce

//explosion
// range, turnspeed, firerate, damage, explosionrange

// lightning
// range, turnspeed, firerate, damage, explosionrange, bounce

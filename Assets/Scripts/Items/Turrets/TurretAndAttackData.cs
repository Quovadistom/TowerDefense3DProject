using System.Collections.Generic;
using UnityEngine;

public class TurretAndAttackData<T> : TurretData
{
    public float Firerate = 1f;
    public float Damage = 50;

    public T ProjectileSpawnPoints;
}

// laser
// range, turnspeed, firerate/damagerate, damage, pierce

//explosion
// range, turnspeed, firerate, damage, explosionrange

// lightning
// range, turnspeed, firerate, damage, explosionrange, bounce

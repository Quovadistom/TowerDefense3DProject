using UnityEngine;

public class TurretData : MonoBehaviour
{
    public float Range = 4;
    public float TurnSpeed = 8;

    public IAttackMethod FiringMethod;
}

// laser
// range, turnspeed, firerate/damagerate, damage, pierce

//explosion
// range, turnspeed, firerate, damage, explosionrange

// lightning
// range, turnspeed, firerate, damage, explosionrange, bounce

using System;

[Serializable]
public class LaserTurretData : TurretAndAttackData<ILaserProfile>
{
    private void Awake()
    {
        FiringMethod = new LaserFiringMethod();
    }
}

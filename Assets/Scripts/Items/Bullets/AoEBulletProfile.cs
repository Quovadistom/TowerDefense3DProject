public class AoEBulletProfile : IAoEBulletProfile
{
    public float Speed { get; }

    public float Damage { get; }

    public float Range { get; }

    public AoEBulletProfile(float speed, float damage, float range)
    {
        Speed = speed;
        Damage = damage;
        Range = range;
    }
}
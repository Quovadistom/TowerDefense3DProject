public class AoEBulletProfile : IAoEBulletProfile
{
    public float Speed { get; }

    public int Damage { get; }

    public float Range { get; }

    public AoEBulletProfile(float speed, int damage, float range)
    {
        Speed = speed;
        Damage = damage;
        Range = range;
    }
}
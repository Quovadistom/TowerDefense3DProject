public class StandardBulletProfile : IBulletProfile
{
    public float Speed { get; }

    public float Damage { get; }

    public StandardBulletProfile(float speed, float damage)
    {
        Speed = speed;
        Damage = damage;
    }
}

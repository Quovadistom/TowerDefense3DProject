public class StandardBulletProfile : IBulletProfile
{
    public float Speed { get; }

    public int Damage { get; }

    public StandardBulletProfile(float speed, int damage)
    {
        Speed = speed;
        Damage = damage;
    }
}

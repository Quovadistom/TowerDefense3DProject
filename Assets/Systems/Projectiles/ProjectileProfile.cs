public class ProjectileProfile
{
    public ProjectileProfile(float speed, float damage, float explosionRange = 0)
    {
        Speed = speed;
        Damage = damage;
        ExplosionRange = explosionRange;
    }

    public float Speed { get; set; }

    public float Damage { get; set; }

    public float ExplosionRange { get; set; }
}

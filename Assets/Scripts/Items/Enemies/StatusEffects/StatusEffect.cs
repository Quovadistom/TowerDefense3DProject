public abstract class StatusEffect
{
    public float DamageRate { get; private set; }

    public BasicEnemy Enemy { get; private set; }

    public StatusEffect(BasicEnemy basicEnemy, float damageRate)
    {
        Enemy = basicEnemy;
        DamageRate = damageRate;
    }

    public abstract void ApplyEffect();

    public abstract void ChangeState(StatusEffect newStatusEffect);
}

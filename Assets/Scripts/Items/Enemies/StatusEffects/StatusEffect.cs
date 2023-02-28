public abstract class StatusEffect
{
    public float DamageRate { get; private set; }

    protected StatusContext StatusContext;

    public StatusEffect(StatusContext statusContext, float damageRate)
    {
        StatusContext = statusContext;
        DamageRate = damageRate;
    }

    public abstract void ApplyEffect();

    public abstract void ChangeState(StatusEffect newStatusEffect);
}

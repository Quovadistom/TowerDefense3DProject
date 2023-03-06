public class StatusEffectContext
{
    public StatusEffect StatusEffect { get; private set; }

    public StatusEffectContext(StatusEffect statusEffect)
    {
        StatusEffect = statusEffect;
    }

    public void RequestChangeState(EffectType m_resistAgainstEffectType, StatusEffect statusEffect)
    {
        // Block changing the effect if enemy is resistent OR effect blocks current effect (e.g. frost blocks corrosion).
        // Can also change to reduction in effect instead of not applying the effect at all
        if (statusEffect.RequestEffectChange(statusEffect) && !m_resistAgainstEffectType.HasFlag(statusEffect.EffectTypeType))
        {
            StatusEffect = statusEffect;
        }
    }
}

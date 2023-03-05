public class StatusEffectContext
{
    public StatusEffect StatusEffect { get; private set; }

    public StatusEffectContext(StatusEffect statusEffect)
    {
        StatusEffect = statusEffect;
    }

    public void RequestChangeState(EffectType m_resistAgainstEffectType, StatusEffect statusEffect)
    {
        statusEffect.RequestEffectChange(statusEffect);

        // Can also change to reduction in effect instead of not applying the effect at all
        if (!m_resistAgainstEffectType.HasFlag(statusEffect.EffectTypeType))
        {
            StatusEffect = statusEffect;
        }
    }
}

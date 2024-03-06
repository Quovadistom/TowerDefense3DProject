using System;

[Flags]
public enum EffectType
{
    None = 0,
    Fire = 1,
    Corrosion = 2,
    Water = 4,
    Electricity = 8
}

public abstract class StatusEffect
{
    public abstract EffectType EffectTypeType { get; }
    public abstract string EffectName { get; }

    public float DamageRateInSeconds { get; private set; }
    public float DurationInSeconds { get; private set; }

    public StatusEffectContext Context { get; set; }

    public StatusEffect(float damageRate, float effectTime)
    {
        DamageRateInSeconds = damageRate;
        DurationInSeconds = effectTime;
    }

    // Bool because effect (e.g. frost) can block other effects from taking effect (e.g. corrosion)
    public abstract bool RequestEffectChange(StatusEffect newStatusEffect);

    public abstract void ApplyEffect(BasicEnemy basicEnemy);
}

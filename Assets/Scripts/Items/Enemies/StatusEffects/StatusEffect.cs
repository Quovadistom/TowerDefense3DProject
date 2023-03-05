using System;

[Flags]
public enum EffectType
{
    NONE = 0,
    FIRE = 1,
    CORROSION = 2,
    WATER = 4,
    ELECTRICITY = 8
}

public abstract class StatusEffect
{
    public abstract EffectType EffectTypeType { get; }

    public float DamageRate { get; private set; }
    public float EffectTime { get; private set; }

    public StatusEffectContext Context { get; set; }

    public StatusEffect(float damageRate, float effectTime)
    {
        DamageRate = damageRate;
        EffectTime = effectTime;
    }

    public abstract void RequestEffectChange(StatusEffect newStatusEffect);

    public abstract void ApplyEffect(BasicEnemy basicEnemy);
}

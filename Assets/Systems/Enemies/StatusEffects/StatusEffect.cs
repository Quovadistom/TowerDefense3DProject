using System;
using UnityEngine;

[Flags]
public enum EffectType
{
    None = 0,
    Fire = 1,
    Corrosion = 2,
    Water = 4,
    Electricity = 8,
    Bleeding = 16
}

public abstract class StatusEffect : ScriptableObject
{
    public float DamageRateInSeconds;
    public float DurationInSeconds;
    public float Damage;

    public abstract EffectType EffectTypeType { get; }
    public abstract string EffectName { get; }

    public StatusEffectContext Context { get; set; }

    // Bool because effect (e.g. frost) can block other effects from taking effect (e.g. corrosion)
    public abstract bool RequestEffectChange(StatusEffect newStatusEffect);

    public abstract void ApplyEffect(BasicEnemy basicEnemy);
}

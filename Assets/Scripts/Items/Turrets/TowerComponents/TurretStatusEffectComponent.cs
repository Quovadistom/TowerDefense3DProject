using System;
using UnityEngine;

public class TurretStatusEffectComponent : MonoBehaviour, ITowerComponent
{
    private StatusEffect m_currentStatusEffect;

    public event Action<StatusEffect> StatusEffectChanged;

    protected void Start()
    {
        StatusEffect ??= new NoneStatusEffect();
    }

    public StatusEffect StatusEffect
    {
        get => m_currentStatusEffect;
        set
        {
            m_currentStatusEffect = value;
            StatusEffectChanged?.Invoke(m_currentStatusEffect);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusContext : MonoBehaviour
{
    private StatusEffect m_statusEffect;

    public StatusEffect StatusEffect
    {
        get => m_statusEffect;
        set => m_statusEffect = value;
    }

    public BasicEnemy Enemy { get; private set; }

    public StatusContext(BasicEnemy basicEnemy)
    {
        m_statusEffect = new NoneStatusEffect(this);
        Enemy = basicEnemy;
    }

    public void ChangeStatusEffect(StatusEffect newStatusEffect)
    {
        m_statusEffect.ChangeState(newStatusEffect);
    }
}

using UnityEngine;

[RequireComponent(typeof(BasicEnemy))]
public class StatusEffectContext : MonoBehaviour
{
    private float m_statusEffectCounter = 0;
    private StatusEffect m_statusEffect;

    public StatusEffect StatusEffect
    {
        get => m_statusEffect;
        private set => m_statusEffect = value;
    }

    public BasicEnemy Enemy { get; private set; }

    private void Awake()
    {
        Enemy = GetComponent<BasicEnemy>();
    }

    private void OnEnable()
    {
        m_statusEffect = new NoneStatusEffect(Enemy);
    }

    private void Update()
    {
        m_statusEffectCounter += Time.deltaTime;

        if (StatusEffect is not NoneStatusEffect)
        {
            if (m_statusEffectCounter >= StatusEffect.DamageRate)
            {
                StatusEffect.ApplyEffect();
                m_statusEffectCounter = 0;
            }
        }
    }

    public void ChangeStatusEffect(StatusEffect newStatusEffect)
    {
        StatusEffect.ChangeState(newStatusEffect);
        StatusEffect = newStatusEffect;
    }
}

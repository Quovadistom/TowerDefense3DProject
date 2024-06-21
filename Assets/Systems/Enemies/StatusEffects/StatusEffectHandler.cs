using UnityEngine;

[RequireComponent(typeof(BasicEnemy))]
public class StatusEffectHandler : MonoBehaviour
{
    [SerializeField] private EffectType m_resistAgainstEffectType;

    private float m_statusEffectCounter = 0;
    private float m_statusDamageTime = 0;
    private StatusEffectContext m_statusEffectContext;

    private BasicEnemy m_enemy;

    private void Awake()
    {
        m_enemy = GetComponent<BasicEnemy>();
    }

    private void OnEnable()
    {
        m_statusEffectContext = new StatusEffectContext(ScriptableObject.CreateInstance<NoneStatusEffect>());
    }

    private void Update()
    {
        StatusEffect statusEffect = m_statusEffectContext.StatusEffect;

        m_statusEffectCounter += Time.deltaTime;

        if (statusEffect is not NoneStatusEffect)
        {
            if (m_statusEffectCounter >= statusEffect.DamageRateInSeconds)
            {
                statusEffect.ApplyEffect(m_enemy);
                m_statusEffectCounter = 0;
            }

            m_statusDamageTime += Time.deltaTime;

            if (m_statusDamageTime >= statusEffect.DurationInSeconds)
            {
                RequestChangeStatusEffect(ScriptableObject.CreateInstance<NoneStatusEffect>());
                m_statusDamageTime = 0;
            }
        }
    }

    public void RequestChangeStatusEffect(StatusEffect statusEffect)
    {
        m_statusEffectContext.RequestChangeState(m_resistAgainstEffectType, statusEffect);
    }
}

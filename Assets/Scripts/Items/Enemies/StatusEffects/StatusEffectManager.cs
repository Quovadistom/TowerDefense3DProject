using UnityEngine;

[RequireComponent(typeof(BasicEnemy))]
public class StatusEffectManager : MonoBehaviour
{
    private float m_statusEffectCounter = 0;
    private BasicEnemy m_enemy;

    public StatusContext StatusContext { get; set; }

    private void Awake()
    {
        m_enemy = GetComponent<BasicEnemy>();
        StatusContext = new StatusContext(m_enemy);
    }

    private void Update()
    {
        m_statusEffectCounter += Time.deltaTime;

        if (StatusContext.StatusEffect is not NoneStatusEffect)
        {
            if (m_statusEffectCounter >= StatusContext.StatusEffect.DamageRate)
            {
                StatusContext.StatusEffect.ApplyEffect();
                m_statusEffectCounter = 0;
            }
        }

    }
}

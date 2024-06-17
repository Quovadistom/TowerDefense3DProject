using UnityEngine;

public class CloseRangeDamager : MonoBehaviour
{
    [SerializeField] private TurretCloseRange m_turretCloseRange;

    private int m_targetsHit = 0;

    private void Awake()
    {
        m_turretCloseRange.NeckExtended += OnNeckPositionChanged;
        m_turretCloseRange.NeckRetracted += OnNeckPositionChanged;
    }

    private void OnDestroy()
    {
        m_turretCloseRange.NeckExtended += OnNeckPositionChanged;
        m_turretCloseRange.NeckRetracted += OnNeckPositionChanged;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null &&
            other.attachedRigidbody.TryGetComponent(out BasicEnemy enemy) &&
            !m_turretCloseRange.UpdateTarget)
        {
            enemy.TakeDamage(m_turretCloseRange.DamageModule.Damage.Value);

            if (enemy.TryGetComponent(out StatusEffectHandler statusEffectHandler))
            {
                statusEffectHandler.RequestChangeStatusEffect(m_turretCloseRange.DamageModule.StatusEffect.Value);
            }

            m_targetsHit++;

            if (m_targetsHit >= m_turretCloseRange.DamageModule.Piercing.Value + 1)
            {
                m_turretCloseRange.Retract();
            }
        }
    }

    private void OnNeckPositionChanged()
    {
        m_targetsHit = 0;
    }
}

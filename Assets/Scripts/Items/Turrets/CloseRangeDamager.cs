using UnityEngine;

public class CloseRangeDamager : MonoBehaviour
{
    [SerializeField] private TurretCloseRange m_turretCloseRange;

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null &&
            other.attachedRigidbody.TryGetComponent(out BasicEnemy enemy) &&
            m_turretCloseRange.IsMovingToTarget)
        {
            enemy.TakeDamage(100);
            m_turretCloseRange.Retract();
        }
    }
}

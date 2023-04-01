using UnityEngine;

public class CloseRangeDamager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null && other.attachedRigidbody.TryGetComponent(out BasicEnemy enemy))
        {
            enemy.TakeDamage(100);
        }
    }
}

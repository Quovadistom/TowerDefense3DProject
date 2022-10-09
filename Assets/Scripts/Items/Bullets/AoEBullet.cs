using UnityEngine;
using Zenject;

public class AoEBullet : ProjectileBase<IAoEBulletProfile>
{
    private LayerSettings m_layerSettings;

    [Inject]
    public void Construct(LayerSettings layerSettings)
    {
        m_layerSettings = layerSettings;
    }

    protected override void OnCollisionWithEnemy(BasicEnemy enemy)
    {
        Collider[] hitColliders = Physics.OverlapSphere(enemy.EnemyMiddle.transform.position, BulletProfile.Range, m_layerSettings.EnemyLayer);
        foreach (var hitCollider in hitColliders)
        {
            Debug.Log($"Hit {hitCollider.name} - {Vector3.Distance(transform.position, hitCollider.transform.position)}");
        }
    }
}

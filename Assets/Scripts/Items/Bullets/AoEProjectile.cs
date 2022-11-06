using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class AoEProjectile : ProjectileBase
{
    private LayerSettings m_layerSettings;

    [Inject]
    public void Construct(LayerSettings layerSettings)
    {
        m_layerSettings = layerSettings;
    }

    protected override void OnCollisionWithEnemy(BasicEnemy enemy)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, ProjectileProfile.ExplosionRange, m_layerSettings.EnemyLayer);
        foreach (var hitCollider in hitColliders)
        {
            BasicEnemy hitEnemy = hitCollider.attachedRigidbody.GetComponent<BasicEnemy>();

            float distanceOffset = (Vector3.Distance(transform.position, hitEnemy.EnemyMiddle.transform.position) / ProjectileProfile.ExplosionRange);
            hitEnemy.TakeDamage(ProjectileProfile.Damage - (distanceOffset * ProjectileProfile.Damage));
        }

        m_poolingService.ReturnPooledObject(this);
    }
}

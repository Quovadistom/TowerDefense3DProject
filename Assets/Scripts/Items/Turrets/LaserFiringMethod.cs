using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LaserFiringMethod : IAttackMethod
{
    private LineRenderer m_lineRenderer;

    public void Shoot(BulletService bulletService, IReadOnlyList<Transform> bulletSpawnPointsList, BasicEnemy target)
    {
        foreach (Transform transform in bulletSpawnPointsList)
        {
            LineRenderer lineRenderer = GetLineRenderer(transform);
            Vector3[] positions = new Vector3[]
            {
                transform.position,
                target.transform.position
            };

            lineRenderer.SetPositions(positions);
        }
    }

    private LineRenderer GetLineRenderer(Transform transform)
    {
        if (m_lineRenderer == null)
        {
            m_lineRenderer = transform.AddComponent<LineRenderer>();
        }

        return m_lineRenderer;
    }
}
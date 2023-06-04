using System;
using UnityEngine;

[Serializable]
public class ProjectileComponent : ComponentBase
{
    [SerializeField] private ProjectileBase m_bulletPrefab;

    public ProjectileBase BulletPrefab
    {
        get { return m_bulletPrefab; }
        set { m_bulletPrefab = value; }
    }

    [SerializeField] private float m_bulletSpeed;

    public float BulletSpeed
    {
        get { return m_bulletSpeed; }
        set { m_bulletSpeed = value; }
    }

    [SerializeField] private float m_explosionRange;

    public float EplosionRange
    {
        get { return m_explosionRange; }
        set { m_explosionRange = value; }
    }
}

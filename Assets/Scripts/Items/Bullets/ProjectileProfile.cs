using System;
using UnityEngine;

[Serializable]
public class ProjectileProfile
{
    [SerializeField] private float m_bulletSpeed;
    [SerializeField] private float m_bulletDamage;
    [SerializeField] private float m_explosionRange;

    public float Speed
    {
        get => m_bulletSpeed;
        set => m_bulletSpeed = value;
    }

    public float Damage
    {
        get => m_bulletDamage;
        set => m_bulletDamage = value;
    }

    public float ExplosionRange
    {
        get => m_explosionRange;
        set => m_explosionRange = value;
    }
}

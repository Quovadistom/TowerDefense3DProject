using System;
using UnityEngine;

[Serializable]
public class DamageComponent : ComponentBase
{
    [SerializeField] private float m_damage;

    public float Damage
    {
        get { return m_damage; }
        set { m_damage = value; }
    }
}
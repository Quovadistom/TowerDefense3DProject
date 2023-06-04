using System;
using UnityEngine;

[Serializable]
public class FireRateComponent : ComponentBase
{
    [SerializeField] private float m_fireRate;

    public float FireRate
    {
        get { return m_fireRate; }
        set { m_fireRate = value; }
    }
}

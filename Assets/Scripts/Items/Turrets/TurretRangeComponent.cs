using NaughtyAttributes;
using System;
using UnityEngine;

public class TurretRangeComponent : ChangeVisualComponent, ITowerComponent
{
    [SerializeField] private float m_range = 4;

    public event Action<float> RangeUpdated;
    public event Action<GameObject> RangeVisualUpdated;

    protected virtual void Start()
    {
        Range = m_range;
    }

    public float Range
    {
        get => m_range;
        set
        {
            m_range = value;
            RangeUpdated?.Invoke(m_range);
        }
    }
}
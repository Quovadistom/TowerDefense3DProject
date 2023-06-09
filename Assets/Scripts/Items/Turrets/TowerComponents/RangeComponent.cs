using System;
using UnityEngine;

[Serializable]
public class RangeComponent : ComponentBase
{
    [SerializeField] private float m_baseRange;
    public Action<float> RangeChanged;

    private float m_buffPercentage = 0;

    public float BuffPercentage
    {
        get => m_buffPercentage;
        set
        {
            m_buffPercentage = value;
            RangeChanged?.Invoke(BaseRange);
        }
    }

    public float BaseRange
    {
        get { return m_baseRange.AddPercentage(BuffPercentage); }
        set
        {
            m_baseRange = value;
            RangeChanged?.Invoke(BaseRange);
        }
    }

    public VisualComponent<Transform> VisualComponent;
}

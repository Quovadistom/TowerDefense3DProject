using System;
using UnityEngine;

[Serializable]
public class ComponentDataTypeFloatWithBuff
{
    [SerializeField] private float m_baseValue;
    public Action<float> ValueChanged;

    private float m_buffPercentage = 0;

    public float BuffPercentage
    {
        get => m_buffPercentage;
        set
        {
            m_buffPercentage = value;
            ValueChanged?.Invoke(BaseValue.AddPercentage(BuffPercentage));
        }
    }

    public float BaseValue
    {
        get { return m_baseValue; }
        set
        {
            m_baseValue = value;
            ValueChanged?.Invoke(m_baseValue.AddPercentage(BuffPercentage));
        }
    }
}

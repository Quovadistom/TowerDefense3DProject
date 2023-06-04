using System;
using UnityEngine;

[Serializable]
public class RangeComponent : ComponentBase
{
    [SerializeField] private float m_range;
    public Action<float> RangeChanged;

    public float Range
    {
        get { return m_range; }
        set
        {
            m_range = value;
            RangeChanged?.Invoke(value);
        }
    }

    public VisualComponent<Transform> VisualComponent;
}

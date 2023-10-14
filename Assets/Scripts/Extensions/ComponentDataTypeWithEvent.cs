using System;
using UnityEngine;

[Serializable]
public class ModuleDataTypeWithEvent<T>
{
    [SerializeField] private T m_value;
    public Action<T> ValueChanged;

    public T Value
    {
        get => m_value;
        set
        {
            m_value = value;
            ValueChanged.Invoke(m_value);
        }
    }
}
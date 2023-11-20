using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class ModuleWithModificationBase : MonoBehaviour
{
    private List<FieldInfo> m_fields = new();
    private bool m_isInitialized = false;

    public List<FieldInfo> Fields
    {
        get
        {
            if (!m_isInitialized || gameObject.scene == default)
            {
                m_fields = this.GetType().GetFields().ToList();
                m_isInitialized = true;
            }

            return m_fields;
        }
    }

    public bool HasComponent<T>() => Fields.FirstOrDefault(field => field.FieldType == typeof(T)) != null;
    public bool HasComponent(Type type) => Fields.FirstOrDefault(field => field.FieldType == type) != null;

    /// <summary>
    /// Can be used to check if a component is available and use the callback to do something with this component,
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="modificationFunc"></param>
    /// <returns></returns>
    public bool TryFindAndActOnComponent<T>(Action<T> callback)
    {
        foreach (var field in Fields.Where(field => field.FieldType == typeof(T)))
        {
            T value = (T)field.GetValue(this);
            callback.Invoke(value);
            return true;
        }

        return false;
    }
}

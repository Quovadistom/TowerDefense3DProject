using System;
using UnityEngine;

[Serializable]
public class VisualComponent<T> : ComponentBase where T : Component
{
    [SerializeField] private T m_visual;
    public Action<T> VisualChanged;

    public T Visual
    {
        get { return m_visual; }
        set
        {
            T newBarrel = GameObject.Instantiate(value, m_visual.transform.parent, false);
            GameObject.Destroy(m_visual.gameObject);
            m_visual = newBarrel;
            VisualChanged?.Invoke(newBarrel);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseVisualChanger<T> : MonoBehaviour where T : ChangeVisualComponent
{
    public T Component;
    [SerializeField] Transform m_baseVisualToChange;

    protected virtual void Awake()
    {
        Component.VisualChanged += OnVisualUpdated;
    }

    protected virtual void OnDestroy()
    {
        Component.VisualChanged += OnVisualUpdated;
    }

    private void OnVisualUpdated(Transform newVisual)
    {
        newVisual.SetParent(m_baseVisualToChange.parent, false);
        Destroy(m_baseVisualToChange.gameObject);
        m_baseVisualToChange = newVisual;
    }
}

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
        Component.VisualChanged += OnRangeVisualUpdated;
    }

    protected virtual void OnDestroy()
    {
        Component.VisualChanged += OnRangeVisualUpdated;
    }

    private void OnRangeVisualUpdated(Transform newVisual)
    {
        Transform visual = GameObject.Instantiate(newVisual, m_baseVisualToChange.transform.parent);
        Destroy(m_baseVisualToChange.gameObject);
        m_baseVisualToChange = visual;
    }
}

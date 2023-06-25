using System;
using UnityEngine;

/// <summary>
/// Special type of component data type that destroys the original value and creates a new instance of the new value under the same parent.
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable]
public class ComponentDataTypeVisual<T> : ComponentBase where T : Component
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

            if (m_visual.gameObject.TryGetComponent(out Outline outline))
            {
                TowerInfoComponent towerInfoComponent = newBarrel.GetComponentInParent<TowerInfoComponent>(true);
                outline.OutlineColor = towerInfoComponent.Selectable.CurrentColor;
            }

            m_visual = newBarrel;
            VisualChanged?.Invoke(newBarrel);
        }
    }
}
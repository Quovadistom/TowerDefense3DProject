using System;
using UnityEngine;

public class ChangeVisualComponent : MonoBehaviour
{
    [SerializeField] protected TowerInfoComponent TowerInfoComponent;

    private Transform m_visual;

    public event Action<Transform> VisualChanged;

    public Transform Visual
    {
        get => m_visual;
        set
        {
            m_visual = Instantiate(value);
            if (m_visual.gameObject.TryGetComponent(out Outline outline))
            {
                outline.OutlineColor = TowerInfoComponent.Selectable.CurrentColor;
            }
            VisualChanged?.Invoke(m_visual);
        }
    }
}
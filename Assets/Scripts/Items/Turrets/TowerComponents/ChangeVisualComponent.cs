using System;
using UnityEngine;

public class ChangeVisualComponent : MonoBehaviour
{
    private Transform m_visual;

    public event Action<Transform> VisualChanged;

    public Transform Visual
    {
        get => m_visual;
        set
        {
            m_visual = Instantiate(value);
            VisualChanged?.Invoke(m_visual);
        }
    }
}
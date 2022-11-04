using System;
using UnityEngine;

public class ChangeVisualComponent : MonoBehaviour
{
    private Transform m_visual;

    public event Action<Transform> VisualChanged;

    public Transform Visual
    {
        set
        {
            m_visual = value;
            VisualChanged?.Invoke(m_visual);
        }
    }
}
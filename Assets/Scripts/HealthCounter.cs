using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class HealthCounter : MonoBehaviour
{
    public TMP_Text m_text;
    private LevelService m_levelService;

    [Inject]
    public void Construct(LevelService levelService)
    {
        m_levelService = levelService;
    }

    private void Awake()
    {
        OnHealthChanged(m_levelService.Health);
        m_levelService.HealthChanged += OnHealthChanged;
    }

    private void OnDestroy()
    {
        m_levelService.HealthChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(int currentHealth)
    {
        m_text.text = currentHealth.ToString();
    }
}

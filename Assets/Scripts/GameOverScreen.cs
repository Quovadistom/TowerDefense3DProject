using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup m_canvasGroup;
    [SerializeField] private float m_fadeInTime;

    private LevelService m_levelService;

    [Inject]
    public void Construct(LevelService levelService)
    {
        m_levelService = levelService;
    }

    private void Awake()
    {
        m_canvasGroup.gameObject.SetActive(false);
        m_canvasGroup.alpha = 0f;

        m_levelService.HealthChanged += OnHealthChanged;
    }

    private void OnDestroy()
    {
        m_levelService.HealthChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(int health)
    {
        if (health == 0)
        {
            m_canvasGroup.gameObject.SetActive(true);
            m_canvasGroup.DOFade(1, m_fadeInTime).SetUpdate(true);
        }
    }
}

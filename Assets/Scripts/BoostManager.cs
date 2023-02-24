using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BoostManager : MonoBehaviour
{
    [SerializeField] private ItemMenuButton m_prefab;
    [SerializeField] private CanvasGroup m_canvasGroup;
    [SerializeField] private Transform m_boostParent;
    [SerializeField] private float m_fadeTime = 0.5f;
    [SerializeField] private Button m_button;

    private WaveService m_waveService;
    private BoostAvailabilityService m_boostAvailabilityService;

    [Inject]
    private void Construct(WaveService waveService, BoostAvailabilityService boostAvailabilityService)
    {
        m_waveService = waveService;
        m_boostAvailabilityService = boostAvailabilityService;
    }

    private void Awake()
    {
        m_waveService.BoostsDrawn += OnBoostsDrawn;
        m_button.onClick.AddListener(OnButtonClicked);

        m_canvasGroup.gameObject.SetActive(false);
        m_canvasGroup.alpha = 0f;
    }

    private void OnDestroy()
    {
        m_waveService.BoostsDrawn -= OnBoostsDrawn;
        m_button.onClick.RemoveListener(OnButtonClicked);
    }

    private void OnBoostsDrawn(List<BoostBase> boostList)
    {
        m_canvasGroup.gameObject.SetActive(true);
        m_canvasGroup.DOFade(1, m_fadeTime).SetUpdate(true).OnComplete(() =>
        {
            foreach (var boost in boostList)
            {
                m_boostAvailabilityService.AddAvailableBoost(boost.BoostID);
                ItemMenuButton spawnedButton = Instantiate(m_prefab, m_boostParent);
                spawnedButton.SetContent(new ButtonInfo()
                {
                    Title = boost.UpgradeName
                });
            }
        });
    }

    private void OnButtonClicked()
    {
        m_canvasGroup.DOFade(0, m_fadeTime).SetUpdate(true).OnComplete(() =>
        {
            foreach (Transform child in m_boostParent)
            {
                Destroy(child.gameObject);
            }

            m_canvasGroup.gameObject.SetActive(false);
        });
    }
}

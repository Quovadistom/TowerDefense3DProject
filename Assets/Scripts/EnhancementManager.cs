using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class EnhancementManager : MonoBehaviour
{
    [SerializeField] private ItemMenuButton m_prefab;
    [SerializeField] private CanvasGroup m_canvasGroup;
    [SerializeField] private Transform m_enhancementParent;
    [SerializeField] private float m_fadeTime = 0.5f;
    [SerializeField] private Button m_button;

    private WaveService m_waveService;
    private EnhancementAvailabilityService m_enhancementAvailabilityService;

    [Inject]
    private void Construct(WaveService waveService, EnhancementAvailabilityService enhancementAvailabilityService)
    {
        m_waveService = waveService;
        m_enhancementAvailabilityService = enhancementAvailabilityService;
    }

    private void Awake()
    {
        m_waveService.EnhancementsDrawn += OnEnhancementsDrawn;
        m_button.onClick.AddListener(OnButtonClicked);

        m_canvasGroup.gameObject.SetActive(false);
        m_canvasGroup.alpha = 0f;
    }

    private void OnDestroy()
    {
        m_waveService.EnhancementsDrawn -= OnEnhancementsDrawn;
        m_button.onClick.RemoveListener(OnButtonClicked);
    }

    private void OnEnhancementsDrawn(List<EnhancementContainer> enhancementList)
    {
        m_canvasGroup.gameObject.SetActive(true);
        m_canvasGroup.DOFade(1, m_fadeTime).SetUpdate(true).OnComplete(() =>
        {
            foreach (var enhancement in enhancementList)
            {
                m_enhancementAvailabilityService.AddAvailableEnhancement(enhancement);
                ItemMenuButton spawnedButton = Instantiate(m_prefab, m_enhancementParent);
                spawnedButton.SetContent(new ButtonInfo()
                {
                    Title = enhancement.Name
                });
            }
        });
    }

    private void OnButtonClicked()
    {
        m_canvasGroup.DOFade(0, m_fadeTime).SetUpdate(true).OnComplete(() =>
        {
            foreach (Transform child in m_enhancementParent)
            {
                Destroy(child.gameObject);
            }

            m_canvasGroup.gameObject.SetActive(false);
        });
    }
}

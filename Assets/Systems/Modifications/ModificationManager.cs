using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] private ItemMenuButton m_prefab;
    [SerializeField] private CanvasGroup m_canvasGroup;
    [SerializeField] private Transform m_resourceParent;
    [SerializeField] private float m_fadeTime = 0.5f;
    [SerializeField] private Button m_button;

    private WaveService m_waveService;
    private ResourceService m_resourceAvailabilityService;
    private LevelService m_levelService;

    [Inject]
    private void Construct(WaveService waveService, ResourceService resourceAvailabilityService, LevelService levelService)
    {
        m_waveService = waveService;
        m_resourceAvailabilityService = resourceAvailabilityService;
        m_levelService = levelService;
    }

    private void Awake()
    {
        m_waveService.ModificationsDrawn += OnResourcesDrawn;
        m_button.onClick.AddListener(OnButtonClicked);

        m_canvasGroup.gameObject.SetActive(false);
        m_canvasGroup.alpha = 0f;
    }

    private void OnDestroy()
    {
        m_waveService.ModificationsDrawn -= OnResourcesDrawn;
        m_button.onClick.RemoveListener(OnButtonClicked);
    }

    private void OnResourcesDrawn(List<Resource> resourceList)
    {
        m_canvasGroup.gameObject.SetActive(true);
        m_canvasGroup.DOFade(1, m_fadeTime).SetUpdate(true).OnComplete(() =>
        {
            foreach (var resource in resourceList)
            {
                m_resourceAvailabilityService.ChangeAvailableResource(resource);
                ItemMenuButton spawnedButton = Instantiate(m_prefab, m_resourceParent);
                spawnedButton.SetContent(new ButtonInfo()
                {
                    Title = resource.Name
                });
            }
        });
    }

    private void OnButtonClicked()
    {
        m_canvasGroup.DOFade(0, m_fadeTime).SetUpdate(true).OnComplete(() =>
        {
            foreach (Transform child in m_resourceParent)
            {
                Destroy(child.gameObject);
            }

            m_canvasGroup.gameObject.SetActive(false);
        });

        if (m_waveService.IsLastWave)
        {
            m_levelService.EndLevel();
        }
    }
}

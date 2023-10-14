using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ModificationManager : MonoBehaviour
{
    [SerializeField] private ItemMenuButton m_prefab;
    [SerializeField] private CanvasGroup m_canvasGroup;
    [SerializeField] private Transform m_modificationParent;
    [SerializeField] private float m_fadeTime = 0.5f;
    [SerializeField] private Button m_button;

    private WaveService m_waveService;
    private ModificationAvailabilityService m_modificationAvailabilityService;

    [Inject]
    private void Construct(WaveService waveService, ModificationAvailabilityService modificationAvailabilityService)
    {
        m_waveService = waveService;
        m_modificationAvailabilityService = modificationAvailabilityService;
    }

    private void Awake()
    {
        m_waveService.ModificationsDrawn += OnModificationsDrawn;
        m_button.onClick.AddListener(OnButtonClicked);

        m_canvasGroup.gameObject.SetActive(false);
        m_canvasGroup.alpha = 0f;
    }

    private void OnDestroy()
    {
        m_waveService.ModificationsDrawn -= OnModificationsDrawn;
        m_button.onClick.RemoveListener(OnButtonClicked);
    }

    private void OnModificationsDrawn(List<ModificationContainer> modificationList)
    {
        m_canvasGroup.gameObject.SetActive(true);
        m_canvasGroup.DOFade(1, m_fadeTime).SetUpdate(true).OnComplete(() =>
        {
            foreach (var modification in modificationList)
            {
                m_modificationAvailabilityService.AddAvailableModification(modification);
                ItemMenuButton spawnedButton = Instantiate(m_prefab, m_modificationParent);
                spawnedButton.SetContent(new ButtonInfo()
                {
                    Title = modification.Name
                });
            }
        });
    }

    private void OnButtonClicked()
    {
        m_canvasGroup.DOFade(0, m_fadeTime).SetUpdate(true).OnComplete(() =>
        {
            foreach (Transform child in m_modificationParent)
            {
                Destroy(child.gameObject);
            }

            m_canvasGroup.gameObject.SetActive(false);
        });
    }
}

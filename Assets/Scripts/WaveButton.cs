using System;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class WaveButton : MonoBehaviour
{
    [SerializeField] private Button m_button;
    private WaveService m_waveService;

    [Inject]
    public void Construct(WaveService waveService)
    {
        m_waveService = waveService;
    }

    private void Awake()
    {
        m_waveService.WaveComplete += OnWaveCompleted;
        m_button.onClick.AddListener(OnButtonClick);
    }

    private void OnDestroy()
    {
        m_waveService.WaveComplete -= OnWaveCompleted;
        m_button.onClick.RemoveListener(OnButtonClick);
    }

    private void OnWaveCompleted()
    {
        m_button.interactable = true;
    }

    private void OnButtonClick()
    {
        m_waveService.StartNextWave();
        m_button.interactable = false;
    }
}

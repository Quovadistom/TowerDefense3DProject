using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class SpeedInformation
{
    public string m_speedText;
    public float m_speed;
}

public class GameTimeButton : MonoBehaviour
{
    [SerializeField] private Button m_button;
    [SerializeField] private TMP_Text m_text;
    [SerializeField] private SpeedInformation[] m_speedInformation;

    private int m_currentIndex = 0;

    private void Awake()
    {
        m_button.onClick.AddListener(OnButtonClick);
    }

    private void OnDestroy()
    {
        m_button.onClick.RemoveListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        m_currentIndex = m_currentIndex == m_speedInformation.Length - 1 ? 0 : m_currentIndex + 1; 

        SpeedInformation speedInformation = m_speedInformation[m_currentIndex];
        m_text.text = speedInformation.m_speedText;
        Time.timeScale = speedInformation.m_speed;
    }
}

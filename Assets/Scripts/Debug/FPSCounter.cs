using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private float m_updateInterval = 0.5f;
    [SerializeField] private TMP_Text m_fpsCounter;

    private float m_accum = 0.0f;
    private int m_frames = 0;
    private float m_timeleft;

    void Start()
    {
        Application.targetFrameRate = 60;
        m_timeleft = m_updateInterval;
    }

    void Update()
    {
        m_timeleft -= Time.deltaTime;
        m_accum += Time.timeScale / Time.deltaTime;
        ++m_frames;

        // Interval ended - update GUI text and start new interval
        if (m_timeleft <= 0.0)
        {
            // display two fractional digits (f2 format)
            m_fpsCounter.text = Mathf.FloorToInt(m_accum / m_frames).ToString();
            m_timeleft = m_updateInterval;
            m_accum = 0.0f;
            m_frames = 0;
        }
    }
}
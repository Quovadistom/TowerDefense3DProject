using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private float m_updateInterval = 0.5f; //How often should the number update
    [SerializeField] private TMP_Text m_fpsCounter; //How often should the number update

    private float m_accum = 0.0f;
    private int m_frames = 0;
    private float m_timeleft;

    GUIStyle textStyle = new GUIStyle();

    // Use this for initialization
    void Start()
    {
        m_timeleft = m_updateInterval;

        textStyle.fontStyle = FontStyle.Bold;
        textStyle.normal.textColor = Color.white;
    }

    // Update is called once per frame
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

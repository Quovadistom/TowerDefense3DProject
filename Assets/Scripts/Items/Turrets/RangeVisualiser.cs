using UnityEngine;

public class RangeVisualiser : MonoBehaviour
{
    [SerializeField] private BarrelTurretMediator m_turretData;
    [SerializeField] private SpriteRenderer m_renderer;
    [SerializeField] private SphereCollider m_rangeCollider;

    private void Awake()
    {
        m_turretData.RangeUpdated += OnRangeUpdated;
    }

    private void OnDestroy()
    {
        m_turretData.RangeUpdated -= OnRangeUpdated;
    }

    private void OnRangeUpdated(float newRange)
    {
        m_renderer.transform.localScale = new Vector3(newRange * 2, newRange * 2, 0);
        m_rangeCollider.radius = newRange;
    }

    public void SetRangeColor(Color color)
    {
        m_renderer.color = color;
    }
}

using UnityEngine;

public class RangeVisualiser : BaseVisualChanger<TurretRangeComponent>
{
    [SerializeField] private SpriteRenderer m_renderer;
    [SerializeField] private SphereCollider m_rangeCollider;

    protected override void Awake()
    {
        base.Awake();

        Component.RangeUpdated += OnRangeUpdated;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        Component.RangeUpdated -= OnRangeUpdated;
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

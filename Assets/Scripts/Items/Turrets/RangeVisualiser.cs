using UnityEngine;
using Zenject;

public class RangeVisualiser : ComponentWithUpgradeBase
{
    [SerializeField] private SpriteRenderer m_renderer;
    [SerializeField] private SphereCollider m_rangeCollider;
    [SerializeField] private Draggable m_draggable;

    public RangeComponent RangeComponent;

    private ColorSettings m_colorSettings;

    [Inject]
    public void Construct(ColorSettings colorSettings)
    {
        m_colorSettings = colorSettings;
    }

    protected void Awake()
    {
        m_draggable.InvalidPlacementDetected += OnInvalidPlacementDetected;
        m_draggable.ValidPlacementDetected += OnValidPlacementDetected;
        RangeComponent.RangeChanged += OnRangeChanged;

        OnRangeChanged(RangeComponent.Range);
    }

    protected void OnDestroy()
    {
        m_draggable.InvalidPlacementDetected -= OnInvalidPlacementDetected;
        m_draggable.ValidPlacementDetected -= OnValidPlacementDetected;
        RangeComponent.RangeChanged -= OnRangeChanged;
    }

    protected void OnRangeChanged(float newRange)
    {
        m_renderer.transform.localScale = new Vector3(newRange * 2, newRange * 2, 0);
        m_rangeCollider.radius = newRange;
    }

    private void OnInvalidPlacementDetected()
    {
        m_renderer.color = m_colorSettings.RangeBlockedToPlaceColor;
    }

    private void OnValidPlacementDetected()
    {
        m_renderer.color = m_colorSettings.RangeFreeToPlaceColor;
    }
}

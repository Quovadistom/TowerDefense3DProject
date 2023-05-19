using DG.Tweening;
using UnityEngine;

public enum MenuDirection
{
    Left,
    Right,
    Top,
    Bottom
}

[RequireComponent(typeof(RectTransform))]
public class MenuPage : MonoBehaviour
{
    [SerializeField] private MenuDirection m_movementDirection;
    [SerializeField] private float m_movementDuration = 0.5f;
    [SerializeField] private bool m_keepPageOpen = false;

    private RectTransform m_rectTransform;
    private Tween m_tween;

    public bool KeepPageOpen => m_keepPageOpen;

    private void Awake()
    {
        m_rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        m_rectTransform.anchoredPosition = GetDisabledPosition(m_movementDirection);
        Exit();
    }

    public void Enter()
    {
        gameObject.SetActive(true);
        m_tween?.Kill();
        m_tween = m_rectTransform.DOAnchorPos(Vector2.zero, m_movementDuration).SetEase(Ease.InOutQuad).SetUpdate(true);
    }

    public void Exit()
    {
        m_tween?.Kill();
        m_tween = m_rectTransform.DOAnchorPos(GetDisabledPosition(m_movementDirection), m_movementDuration)
            .OnComplete(() => gameObject.SetActive(false)).SetEase(Ease.InOutQuad).SetUpdate(true);
    }

    private Vector2 GetDisabledPosition(MenuDirection menuDirection)
    {
        return menuDirection switch
        {
            MenuDirection.Left => new Vector2(-2100, 0),
            MenuDirection.Right => new Vector2(2100, 0),
            MenuDirection.Top => new Vector2(0, 1200),
            MenuDirection.Bottom => new Vector2(0, -1200),
            _ => throw new System.NotImplementedException(),
        };
    }
}

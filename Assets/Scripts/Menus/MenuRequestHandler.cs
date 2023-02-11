using DG.Tweening;
using NaughtyAttributes;
using System;
using UnityEngine;
using Zenject;

public enum MenuDirection
{
    Left,
    Right,
    Top,
    Bottom
}

[RequireComponent(typeof(RectTransform))]
public class MenuRequestHandler : MonoBehaviour
{
    [SerializeField] private MenuDirection m_movementDirection;
    [SerializeField] private float m_movementDuration = 0.5f;
    [SerializeField] private bool m_moveOtherActiveMenus;

    private RectTransform m_rectTransform;
    private MenuService m_menuService;
    private Tween m_tween;

    public bool MoveOtherMenus => m_moveOtherActiveMenus;
    public bool IsOpen { get; private set; }

    [Inject]
    public void Construct(MenuService menuService)
    {
        m_menuService = menuService;
    }

    private void Awake()
    {
        m_menuService.AddMenuRequesters(this);
        m_menuService.ShowMenuRequested += OnShowMenuRequested;
        m_menuService.HideMenuRequested += OnHideMenuRequested;
        m_rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        m_rectTransform.anchoredPosition = GetDisabledPosition(m_movementDirection);
    }

    private void OnDestroy()
    {
        m_menuService.ShowMenuRequested -= OnShowMenuRequested;
    }

    private void OnShowMenuRequested(MenuRequestHandler menuRequestHandler)
    {
        if (menuRequestHandler != this && menuRequestHandler.MoveOtherMenus)
        {
            HideMenu();
        }
        else if (menuRequestHandler == this && !IsOpen)
        {
            ShowMenu();
        }
    }

    private void OnHideMenuRequested(MenuRequestHandler menuRequestHandler)
    {
        if (menuRequestHandler == this && IsOpen)
        {
            HideMenu();
        }
    }

    public void ShowMenu()
    {
        IsOpen = true;
        m_menuService.RequestShowMenu(this);
        gameObject.SetActive(true);
        m_tween?.Kill();
        m_tween = m_rectTransform.DOAnchorPos(Vector2.zero, m_movementDuration).SetEase(Ease.InOutQuad).SetUpdate(true);
    }

    public void HideMenu()
    {
        IsOpen = false;
        m_tween?.Kill();
        m_tween = m_rectTransform.DOAnchorPos(GetDisabledPosition(m_movementDirection), m_movementDuration)
            .OnComplete(() => gameObject.SetActive(true)).SetEase(Ease.InOutQuad).SetUpdate(true);
    }

    private Vector2 GetDisabledPosition(MenuDirection menuDirection)
    {
        return menuDirection switch
        {
            MenuDirection.Left => new Vector2(-2100, 0),
            MenuDirection.Right => new Vector2(2100, 0),
            MenuDirection.Top => new Vector2(0, 1200),
            _ => new Vector2(0, -1200),
        };
    }
}

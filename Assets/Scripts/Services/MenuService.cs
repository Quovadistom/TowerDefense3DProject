using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MenuService
{
    private List<MenuRequestHandler> m_menuRequestHandlers = new List<MenuRequestHandler>();

    public event Action<List<ButtonInfo>> ItemMenuRequested;

    public event Action<MenuRequestHandler> ShowMenuRequested;
    public event Action<MenuRequestHandler> HideMenuRequested;

    public bool TryGetMenuRequestHandler<T>(out MenuRequestHandler menuRequestHandler) where T : MonoBehaviour
    {
        menuRequestHandler = m_menuRequestHandlers.FirstOrDefault(handler => handler.TryGetComponent<T>(out T compoment) == true);
        return menuRequestHandler != null;
    }

    public void RequestShowMenu<T>() where T : MonoBehaviour
    {
        if (TryGetMenuRequestHandler<T>(out MenuRequestHandler menuRequestHandler))
        {
            RequestShowMenu(menuRequestHandler);
        }
    }

    public void RequestHideMenu<T>() where T : MonoBehaviour
    {
        if (TryGetMenuRequestHandler<T>(out MenuRequestHandler menuRequestHandler))
        {
            RequestHideMenu(menuRequestHandler);
        }
    }

    public void RequestShowMenu(MenuRequestHandler menuRequestHandler) => ShowMenuRequested?.Invoke(menuRequestHandler);
    public void RequestHideMenu(MenuRequestHandler menuRequestHandler) => HideMenuRequested?.Invoke(menuRequestHandler);

    public void RequestItemMenu(List<ButtonInfo> buttonInfos)
    {
        ItemMenuRequested?.Invoke(buttonInfos);
        RequestShowMenu<ItemsMenu>();
    }

    public void RequestCloseItemMenu()
    {
        RequestHideMenu<ItemsMenu>();
    }

    public void AddMenuRequesters(MenuRequestHandler menuRequestHandler) => m_menuRequestHandlers.Add(menuRequestHandler);
}

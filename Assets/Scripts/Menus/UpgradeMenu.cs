using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour
{
    [SerializeField] private MenuController m_controller;
    [SerializeField] private ItemsMenu m_itemsMenu;

    private MenuPage m_itemMenuPage;

    private void Awake()
    {
        m_itemMenuPage = m_itemsMenu.GetComponent<MenuPage>();
    }

    public void OpenItemMenu(List<ButtonInfo> items)
    {
        m_itemsMenu.FillItemMenu(items);
        m_controller.PushMenuPage(m_itemMenuPage);
    }

    public void CloseItemMenu()
    {
        if (m_controller.IsPageActive(m_itemMenuPage))
        {
            m_controller.PopMenuPage();
        }
    }
}

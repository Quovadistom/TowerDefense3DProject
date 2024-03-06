using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    private Stack<MenuPage> m_pageStack = new();

    public bool IsPageActive(MenuPage page)
    {
        if (m_pageStack.TryPeek(out MenuPage topPage))
        {
            return topPage == page;
        }

        return false;
    }

    public void PushMenuPage(MenuPage page)
    {
        if (m_pageStack.TryPeek(out MenuPage currentPage) && currentPage == page)
        {
            return;
        }

        page.Enter();

        if (currentPage != null && !currentPage.KeepPageOpen)
        {
            currentPage.Exit();
        }

        m_pageStack.Push(page);
    }

    public void PopMenuPage()
    {
        if (m_pageStack.TryPop(out MenuPage poppedPage))
        {
            poppedPage.Exit();

            if (m_pageStack.TryPeek(out MenuPage newCurrentPage))
            {
                newCurrentPage.Enter();
            }
        }
    }

    public void PushOrPopMenuPage(MenuPage page)
    {
        if (IsPageActive(page))
        {
            PopMenuPage();
        }
        else
        {
            PushMenuPage(page);
        }
    }
}

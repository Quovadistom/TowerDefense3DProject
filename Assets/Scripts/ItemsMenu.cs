using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ButtonInfo
{
    public string Title { get; set; }
    public Action Callback;
}

public class ItemsMenu : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup m_content;
    [SerializeField] private Vector2 m_standardItemSize;
    [SerializeField] private RectTransform m_menu;

    private ItemMenuButton.Factory m_itemMenuButtonFactory;

    [Inject]
    public void Construct(ItemMenuButton.Factory itemMenuButtonFactory)
    {
        m_itemMenuButtonFactory = itemMenuButtonFactory;
    }

    private void Awake()
    {
        m_content.transform.ClearChildren();
    }

    public void FillItemMenu(List<ButtonInfo> items) => SetContent(items, new Vector2(m_standardItemSize.x, m_standardItemSize.y));

    public void SetContent(List<ButtonInfo> items, Vector2 itemSize)
    {
        m_content.transform.ClearChildren();
        m_content.cellSize = m_standardItemSize;

        foreach (ButtonInfo info in items)
        {
            ItemMenuButton button = m_itemMenuButtonFactory.Create();
            button.transform.SetParent(m_content.transform, false);
            button.SetContent(info);
        }
    }
}

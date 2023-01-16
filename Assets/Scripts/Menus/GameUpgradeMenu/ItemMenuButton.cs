using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ItemMenuButton : MonoBehaviour
{
    [SerializeField] TMP_Text m_titleText;
    [SerializeField] Button m_button;

    private Action m_callback;
    private ItemMenuService m_itemMenuService;

    [Inject]
    public void Construct(ItemMenuService itemMenuService)
    {
        m_itemMenuService = itemMenuService;
    }

    private void Awake()
    {
        m_button.onClick.AddListener(OnButtonClick);
    }

    private void OnDestroy()
    {
        m_button.onClick.RemoveListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        m_callback?.Invoke();
        m_itemMenuService.RequestCloseItemMenu();
    }

    public void SetContent(ButtonInfo buttonInfo)
    {
        m_titleText.text = buttonInfo.Title;
        m_callback = buttonInfo.Callback;
    }

    public class Factory : PlaceholderFactory<ItemMenuButton> { }
}

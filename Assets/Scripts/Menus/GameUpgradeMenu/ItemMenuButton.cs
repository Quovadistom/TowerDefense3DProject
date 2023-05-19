using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ItemMenuButton : MonoBehaviour
{
    [SerializeField] TMP_Text m_titleText;
    [SerializeField] Button m_button;

    private Action m_callback;

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
    }

    public void SetContent(ButtonInfo buttonInfo)
    {
        m_titleText.text = buttonInfo.Title;
        m_callback = buttonInfo.Callback;
    }

    public class Factory : PlaceholderFactory<ItemMenuButton> { }
}

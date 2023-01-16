using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button m_mainMenuButton;
    [SerializeField] private RectTransform m_upgradeMenu;
    [SerializeField] private RectTransform m_itemsMenu;

    private void Awake()
    {
        m_mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
    }

    private void OnDestroy()
    {
        m_mainMenuButton.onClick.RemoveListener(OnMainMenuButtonClicked);
    }

    private void OnMainMenuButtonClicked()
    {
        m_upgradeMenu.gameObject.SetActive(true);
    }
}

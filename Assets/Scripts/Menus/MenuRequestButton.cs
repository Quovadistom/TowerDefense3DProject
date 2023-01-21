using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MenuRequestButton : MonoBehaviour
{
    [SerializeField] private MenuRequestHandler m_menuRequestHandler;

    private Button m_button;

    private void Awake()
    {
        m_button = GetComponent<Button>();
        m_button.onClick.AddListener(OnbuttonClick);
    }

    private void OnDestroy()
    {
        m_button.onClick.RemoveListener(OnbuttonClick);
    }

    private void OnbuttonClick()
    {
        if (m_menuRequestHandler.IsOpen)
        {
            m_menuRequestHandler.HideMenu();
        }
        else
        {
            m_menuRequestHandler.ShowMenu();
        }
    }
}

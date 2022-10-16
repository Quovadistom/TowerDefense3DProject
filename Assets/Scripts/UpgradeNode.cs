using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeNode : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool m_isVisible;
    [SerializeField] private bool m_isUnlocked = false;

    [Header("UI References")]
    [SerializeField] Button m_upgradeButton;
    [SerializeField] TMP_Text m_text;
    [SerializeField] Transform m_input;
    [SerializeField] Transform m_output;
    [SerializeField] List<UpgradeNode> m_connectingNodes;

    public Vector3 InputPosition => m_input.position;

    private void OnValidate()
    {
        SetVisuals();
    }

    private void Awake()
    {
        m_upgradeButton.onClick.AddListener(OnButtonClick); 
        SetVisuals();
    }

    private void SetVisuals()
    {
        m_text.text = name = transform.GetSiblingIndex().ToString();

        m_upgradeButton.gameObject.SetActive(m_isVisible);
        m_upgradeButton.interactable = m_isUnlocked;
    }

    private void OnButtonClick()
    {
        foreach (UpgradeNode node in m_connectingNodes)
        {
            node.Unlock();
        }
    }

    private void Unlock()
    {
        if (!m_isVisible)
        {
            Debug.LogWarning("This node is hidden. Please show the node or remove dependency.", this);
        }

        m_isUnlocked = true;
        m_upgradeButton.interactable = m_isUnlocked;
    }
}

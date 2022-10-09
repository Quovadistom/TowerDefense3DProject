using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeNode : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] Button m_upgradeButton;

    [SerializeField] List<UpgradeNode> m_connectingNodes;

    private bool m_bought;
    private bool m_unlocked;

    private void Awake()
    {
        m_upgradeButton.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        m_bought = true;
        foreach (UpgradeNode node in m_connectingNodes)
        {
            node.Unlock();
        }
    }

    private void Unlock()
    {
        m_unlocked = true;
    }
}

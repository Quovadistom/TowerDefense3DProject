using NaughtyAttributes;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeNode : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TurretUpgradeTreeBase m_treeBase;
    [SerializeField] private Button m_upgradeButton;
    [SerializeField] private TMP_Text m_text;

    public List<UpgradeNode> UnlockedByNodes;

    public event Action ButtonClicked;

    [HideInInspector] public bool m_isVisible = true;
    [HideInInspector] public bool IsUnlocked = false;

    public bool IsBought { get; private set; } = false;

    private int m_unlockSignals = 0;

    private void OnValidate()
    {
        m_text.text = name = transform.GetSiblingIndex().ToString();
    }

    private void Awake()
    {
        m_upgradeButton.onClick.AddListener(OnButtonClick);

        m_upgradeButton.interactable = IsUnlocked;
        m_upgradeButton.gameObject.SetActive(m_isVisible);

        foreach (UpgradeNode upgradeNode in UnlockedByNodes)
        {
            upgradeNode.ButtonClicked += Unlock;
        }

        if (!m_treeBase.IsUpgradeLeft)
        {
            LockButton();
        }

        m_treeBase.AllUpgradesSpend += LockButton;
    }

    private void OnDestroy()
    {
        ButtonClicked = null;
        m_treeBase.AllUpgradesSpend -= LockButton;
    }

    private void LockButton()
    {
        IsUnlocked = false;
        m_upgradeButton.interactable = false;
    }

    [Button]
    private void LockOrUnlock()
    {
        IsUnlocked = !IsUnlocked;
        m_upgradeButton.interactable = IsUnlocked;
    }

    [Button]
    private void HideOrShow()
    {
        m_isVisible = !m_isVisible;
        m_upgradeButton.gameObject.SetActive(m_isVisible);
    }

    public void OnButtonClick()
    {
        m_upgradeButton.interactable = false;
        IsBought = true;
        ButtonClicked?.Invoke();
        m_treeBase.LowerUpgradeCount();
    }

    public void Unlock()
    {
        m_unlockSignals++;

        if (!m_isVisible)
        {
            Debug.LogWarning("This node is hidden. Please show the node or remove dependency.", this);
        }

        if (m_unlockSignals < UnlockedByNodes.Count)
        {
            return;
        }

        IsUnlocked = true;
        m_upgradeButton.interactable = IsUnlocked;
    }
}

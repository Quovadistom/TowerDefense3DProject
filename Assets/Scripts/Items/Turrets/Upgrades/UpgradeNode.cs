using JetBrains.Annotations;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class UpgradeNode : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] TurretUpgradeTreeBase m_treeBase;
    [SerializeField] Button m_upgradeButton;
    [SerializeField] TMP_Text m_text;
    [SerializeField] Transform m_input;
    [SerializeField] Transform m_output;
    [SerializeField] UILineRenderer m_lineRendererAsset;
    [SerializeField] List<UpgradeNode> m_unlockedByNodes;

    public Vector3 InputPosition => m_output.position;

    public event Action ButtonClicked;

    [HideInInspector] public bool m_isVisible = true;
    [HideInInspector] public bool m_isUnlocked = false;

    private int m_unlockSignals = 0;

    private void OnValidate()
    {
        m_text.text = name = transform.GetSiblingIndex().ToString();
    }

    private void Awake()
    {
        m_upgradeButton.onClick.AddListener(OnButtonClick);

        m_upgradeButton.interactable = m_isUnlocked;
        m_upgradeButton.gameObject.SetActive(m_isVisible);

        foreach (UpgradeNode upgradeNode in m_unlockedByNodes)
        {
            upgradeNode.ButtonClicked += Unlock;
        }
    }

    private void OnDestroy()
    {
        ButtonClicked = null;
    }

    [Button]
    private void LockOrUnlock()
    {
        m_isUnlocked = !m_isUnlocked;
        m_upgradeButton.interactable = m_isUnlocked;
    }

    [Button]
    private void HideOrShow()
    {
        m_isVisible = !m_isVisible;
        m_upgradeButton.gameObject.SetActive(m_isVisible);
    }

    [Button]
    public void UpdateLineRenderers()
    {        
        foreach (UILineRenderer lineRenderer in GetComponentsInChildren<UILineRenderer>(true))
        {
            DestroyImmediate(lineRenderer.gameObject);
        }

        foreach (UpgradeNode upgradeNode in m_unlockedByNodes)
        {
            Vector2[] coordinates = new Vector2[]
            {
                Vector2.zero,
                m_input.InverseTransformPoint(upgradeNode.InputPosition)
            };

            UILineRenderer lineRenderer = Instantiate(m_lineRendererAsset, m_input);
            lineRenderer.Points = coordinates;
        }
    }

    private void OnButtonClick()
    {
        m_upgradeButton.interactable = false;

        ButtonClicked?.Invoke();
    }

    private void Unlock()
    {
        m_unlockSignals++;
        
        if (!m_isVisible)
        {
            Debug.LogWarning("This node is hidden. Please show the node or remove dependency.", this);
        }

        if (m_unlockSignals < m_unlockedByNodes.Count)
        {
            return;
        }

        m_isUnlocked = true;
        m_upgradeButton.interactable = m_isUnlocked;
    }
}

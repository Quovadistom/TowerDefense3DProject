using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeTree : MonoBehaviour
{
    [SerializeField] SelectedTurretMenu m_selectedTurretMenu;
    [SerializeField] ScrollRect m_scrollRect;
    [SerializeField] RectTransform m_viewPort;

    private TurretUpgradeTreeBase m_activeTurretUpgradeTreeBase;

    private Dictionary<TurretMediatorBase, TurretUpgradeTreeBase> m_turretUpgradeTrees = new Dictionary<TurretMediatorBase, TurretUpgradeTreeBase>();

    void Awake()
    {
        m_selectedTurretMenu.TurretDataChanged += OnTurretChanged;
    }

    private void OnDestroy()
    {
        m_selectedTurretMenu.TurretDataChanged -= OnTurretChanged;
    }

    private void OnTurretChanged(TurretMediatorBase selectedTurret)
    {
        if (m_activeTurretUpgradeTreeBase != null)
        {
            m_activeTurretUpgradeTreeBase.gameObject.SetActive(false);
        }

        if (selectedTurret == null)
        {
            return;
        }

        if (m_turretUpgradeTrees.TryGetValue(selectedTurret, out TurretUpgradeTreeBase upgradeTree))
        {
            m_activeTurretUpgradeTreeBase = upgradeTree;
        }
        else
        {
            selectedTurret.UpgradeTreeAsset.transform.SetParent(m_viewPort, false);
            m_activeTurretUpgradeTreeBase = selectedTurret.UpgradeTreeAsset;
            m_turretUpgradeTrees.Add(selectedTurret, m_activeTurretUpgradeTreeBase);
        }

        m_scrollRect.StopMovement();
        m_scrollRect.content = (RectTransform)m_activeTurretUpgradeTreeBase.transform;

        if (m_activeTurretUpgradeTreeBase.TryGetComponent(out Canvas canvas))
        {
            Destroy(canvas);
        }

        m_activeTurretUpgradeTreeBase.gameObject.SetActive(true);
    }
}

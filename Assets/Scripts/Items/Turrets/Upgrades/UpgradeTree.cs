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

    void Awake()
    {
        m_selectedTurretMenu.TurretDataChanged += OnTurretChanged;
    }

    private void OnDestroy()
    {
        m_selectedTurretMenu.TurretDataChanged -= OnTurretChanged;
    }

    private void OnTurretChanged(TurretMediator selectedTurret)
    {
        m_activeTurretUpgradeTreeBase = Instantiate(selectedTurret.UpgradeTreeAsset, m_viewPort);
        m_scrollRect.content = (RectTransform) m_activeTurretUpgradeTreeBase.transform;
    }
}

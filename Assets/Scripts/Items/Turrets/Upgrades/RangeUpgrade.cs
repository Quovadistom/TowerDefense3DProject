using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UpgradeNode))]
public class RangeUpgrade : UpgradeBase<TurretMediatorBase>
{
    [SerializeField] private float m_increaseRangePercentage;
    [SerializeField] GameObject m_newVisual;
    private UpgradeNode m_node;

    private void Awake()
    {
        m_node = GetComponent<UpgradeNode>();
        m_node.ButtonClicked += OnUpgradeButtonClicked; 
    }

    private void OnUpgradeButtonClicked()
    {
        m_turretMediator.Range = m_turretMediator.Range.AddPercentage(m_increaseRangePercentage);
        m_turretMediator.RangeVisual = m_newVisual;
    }
}

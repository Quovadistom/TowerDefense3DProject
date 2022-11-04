using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeUpgrade : UpgradeBase<TurretRangeComponent>
{
    [SerializeField] private float m_increaseRangePercentage;
    [SerializeField] Transform m_newVisual;
    private UpgradeNode m_node;

    private void Awake()
    {
        m_node = GetComponent<UpgradeNode>();
        m_node.ButtonClicked += OnUpgradeButtonClicked; 
    }

    private void OnUpgradeButtonClicked()
    {
        m_turretMediator.Range = m_turretMediator.Range.AddPercentage(m_increaseRangePercentage);
        m_turretMediator.Visual = m_newVisual;
    }
}

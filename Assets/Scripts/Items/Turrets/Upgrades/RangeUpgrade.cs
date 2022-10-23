using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UpgradeNode))]
public class RangeUpgrade : UpgradeBase<TurretMediatorBase>
{
    [SerializeField] float IncreaseRangePercentage;
    [SerializeField] GameObject NewVisual;
    private UpgradeNode node;

    private void Awake()
    {
        node = GetComponent<UpgradeNode>();
        node.ButtonClicked += OnUpgradeButtonClicked; 
    }

    private void OnUpgradeButtonClicked()
    {
        m_turretMediator.SetRange(new RangeUpdate(
            m_turretMediator.Range.AddPercentage(IncreaseRangePercentage),
            NewVisual));
    }
}

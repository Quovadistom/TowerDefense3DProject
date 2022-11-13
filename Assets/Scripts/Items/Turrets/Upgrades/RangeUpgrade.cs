using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeUpgrade : UpgradeBase<TurretRangeComponent>
{
    [SerializeField] private float m_increaseRangePercentage;
    [SerializeField] Transform m_newVisual;

    protected override void OnUpgradeButtonClicked()
    {
        base.OnUpgradeButtonClicked();

        m_turretMediator.Range = m_turretMediator.Range.AddPercentage(m_increaseRangePercentage);
        m_turretMediator.Visual = m_newVisual;
    }
}

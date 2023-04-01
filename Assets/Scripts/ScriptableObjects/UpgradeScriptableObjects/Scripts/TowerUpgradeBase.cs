using NaughtyAttributes;
using System;
using UnityEngine;

public abstract class TowerUpgradeBase : UpgradeBase
{
    [InfoBox("Select the new visual (optional)")]
    [SerializeField] protected Transform m_newVisual;

    public abstract Type TowerComponentType { get; }

    public abstract void TryApplyUpgrade(TowerInfoComponent towerInfoComponent);
}
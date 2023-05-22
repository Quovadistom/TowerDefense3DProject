using NaughtyAttributes;
using UnityEngine;

public abstract class TowerUpgradeBase : UpgradeBase
{
    [InfoBox("Select the new visual (optional)")]
    [SerializeField] protected Transform m_newVisual;

    public abstract bool IsTowerSuitable(TowerInfoComponent component);

    public abstract void TryApplyUpgrade(TowerInfoComponent towerInfoComponent);
}
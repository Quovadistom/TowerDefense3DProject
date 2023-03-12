using UnityEngine;

public abstract class TowerUpgradeBase : ScriptableObject, IIDProvider
{
    [SerializeField] protected string m_upgradeName;
    [SerializeField] protected float m_upgradeCost;
    [SerializeField] protected Transform m_newVisual;

    public float UpgradeCost => m_upgradeCost;
    public string ID => m_upgradeName;

    public abstract void TryApplyUpdate(TowerInfoComponent towerInfoComponent);
}


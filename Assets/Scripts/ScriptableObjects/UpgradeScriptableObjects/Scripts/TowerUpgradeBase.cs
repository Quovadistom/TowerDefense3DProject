using NaughtyAttributes;
using System;
using UnityEngine;

public abstract class TowerUpgradeBase : ScriptableObject, IIDProvider
{
    [ReadOnly][SerializeField] private string m_id;

    [SerializeField] protected string m_upgradeName;
    [SerializeField] protected Transform m_newVisual;

    public string ID => m_id.ToString();

    public string Name => m_upgradeName;

    private void Reset()
    {
        m_id = Guid.NewGuid().ToString();
    }

    public abstract void TryApplyUpdate(TowerInfoComponent towerInfoComponent);
}


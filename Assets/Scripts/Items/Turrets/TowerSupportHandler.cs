using Assets.Scripts.Interactables;
using System;
using UnityEngine;

[RequireComponent(typeof(SupportTowerSelector))]
public abstract class TowerSupportHandler<T> : MonoBehaviour where T : ModuleBase
{
    [SerializeField] private TowerModule m_towerInfoComponent;

    protected SupportTowerSelector m_supportTowerSelector;

    protected virtual void Awake()
    {
        m_supportTowerSelector = GetComponent<SupportTowerSelector>();
        m_supportTowerSelector.AddArgument((component) => component.HasModule<T>());

        m_supportTowerSelector.TowerAdded += OnTowerAdded;
        m_supportTowerSelector.TowerRemoved += OnTowerRemoved;
    }

    protected virtual void OnDestroy()
    {
        ResetConnectedTowers();
    }

    protected void AddArgument(Func<TowerModule, bool> argument)
    {
        m_supportTowerSelector.AddArgument(argument);
    }

    protected abstract void AddTowerBuff(ModuleParent componentParent);

    protected abstract void RemoveTowerBuff(ModuleParent componentParent);

    protected abstract void ResetConnectedTowers();

    private void OnTowerAdded(Selectable selectable)
    {
        if (selectable.GameObjectToSelect.TryGetComponent(out ModuleParent componentParent))
        {
            AddTowerBuff(componentParent);
        }
    }

    private void OnTowerRemoved(Selectable selectable)
    {
        if (selectable.GameObjectToSelect.TryGetComponent(out ModuleParent componentParent))
        {
            RemoveTowerBuff(componentParent);
        }
    }
}

using Assets.Scripts.Interactables;
using System;
using UnityEngine;

[RequireComponent(typeof(SupportTowerSelector))]
public abstract class TowerSupportHandler<T> : MonoBehaviour where T : ComponentBase
{
    [SerializeField] private TowerInfoComponent m_towerInfoComponent;

    protected SupportTowerSelector m_supportTowerSelector;

    protected virtual void Awake()
    {
        m_supportTowerSelector = GetComponent<SupportTowerSelector>();
        m_supportTowerSelector.AddArgument((component) => component.HasComponent<T>());

        m_supportTowerSelector.TowerAdded += OnTowerAdded;
        m_supportTowerSelector.TowerRemoved += OnTowerRemoved;
    }

    protected virtual void OnDestroy()
    {
        ResetConnectedTowers();
    }

    protected void AddArgument(Func<TowerInfoComponent, bool> argument)
    {
        m_supportTowerSelector.AddArgument(argument);
    }

    protected abstract void AddTowerBuff(ComponentParent componentParent);

    protected abstract void RemoveTowerBuff(ComponentParent componentParent);

    protected abstract void ResetConnectedTowers();

    private void OnTowerAdded(Selectable selectable)
    {
        if (selectable.GameObjectToSelect.TryGetComponent(out ComponentParent componentParent))
        {
            AddTowerBuff(componentParent);
        }
    }

    private void OnTowerRemoved(Selectable selectable)
    {
        if (selectable.GameObjectToSelect.TryGetComponent(out ComponentParent componentParent))
        {
            RemoveTowerBuff(componentParent);
        }
    }
}

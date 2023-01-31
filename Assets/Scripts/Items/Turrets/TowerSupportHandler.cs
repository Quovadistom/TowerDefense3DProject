using Assets.Scripts.Interactables;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(SupportTowerSelector))]
public abstract class TowerSupportHandler<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private TowerInfoComponent m_towerInfoComponent;

    private SupportTowerSelector m_supportTowerSelector;
    private ColorSettings m_colorSettings;

    protected List<Selectable> m_connectedTowers = new List<Selectable>();

    public int ConnectedTowerCount => m_connectedTowers.Count;

    [Inject]
    public void Construct(ColorSettings colorSettings)
    {
        m_colorSettings = colorSettings;
    }

    protected virtual void Awake()
    {
        m_supportTowerSelector = GetComponent<SupportTowerSelector>();
        m_supportTowerSelector.TowerClicked += OnTowerClicked;
        m_supportTowerSelector.SerializedTowerAdded += AddConnectedTower;
        m_supportTowerSelector.TowerRemoved += OnTowerRemoved;
        m_supportTowerSelector.ThisTowerSelected += OnThisTowerSelected;

        m_supportTowerSelector.AddSuitableType<T>();
    }

    protected virtual void OnDestroy()
    {
        ResetConnectedTowers();

        m_supportTowerSelector.TowerClicked -= OnTowerClicked;
        m_supportTowerSelector.SerializedTowerAdded -= AddConnectedTower;
        m_supportTowerSelector.TowerRemoved -= OnTowerRemoved;
        m_supportTowerSelector.ThisTowerSelected -= OnThisTowerSelected;
    }

    private void AddConnectedTower(Selectable selectable)
    {
        m_connectedTowers.Add(selectable);
        RecalculateValues();
    }

    private void RemoveConnectedTower(Selectable selectable)
    {
        m_connectedTowers.Remove(selectable);
        RecalculateValues(selectable.GameObjectToSelect.GetComponent<T>());
    }

    private void OnTowerClicked(Selectable selectable)
    {
        if (!selectable.GameObjectToSelect.TryGetComponent<T>(out T component))
        {
            return;
        }

        if (!m_connectedTowers.Contains(selectable))
        {
            selectable.OutlineObject(true, m_colorSettings.ConnectedOutline);
            AddConnectedTower(selectable);
            selectable.GameObjectToSelect.GetComponent<TowerInfoComponent>().ConnectedSupportTowers.AddSafely(m_towerInfoComponent.TowerID);
        }
        else if (m_connectedTowers.Contains(selectable))
        {
            selectable.OutlineObject(true, m_colorSettings.FocusOutline);
            RemoveConnectedTower(selectable);
            selectable.GameObjectToSelect.GetComponent<TowerInfoComponent>().ConnectedSupportTowers.Remove(m_towerInfoComponent.TowerID);
        }
    }

    private void OnTowerRemoved(Selectable selectable)
    {
        if (m_connectedTowers.Contains(selectable))
        {
            m_connectedTowers.Remove(selectable);
            RecalculateValues(selectable.GameObjectToSelect.GetComponent<T>());
        }
    }

    private void OnThisTowerSelected()
    {
        foreach (Selectable selectable in m_connectedTowers)
        {
            selectable.OutlineObject(true, m_colorSettings.ConnectedOutline);
        }
    }

    protected abstract void ResetConnectedTowers();

    protected virtual void RecalculateValues(T removedTower = null) { }
}

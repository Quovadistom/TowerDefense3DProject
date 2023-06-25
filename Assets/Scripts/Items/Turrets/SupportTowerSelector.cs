using Assets.Scripts.Interactables;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class SupportTowerSelector : MonoBehaviour
{
    [SerializeField] private Selectable m_selectable;
    [SerializeField] private TowerInfoComponent m_towerInfoComponent;

    public SupportSelectorComponent SupportSelectorComponent;

    private SelectionService m_selectionService;
    private ColorSettings m_colorSettings;
    private List<Selectable> m_suitableTowers = new();
    private List<Func<TowerInfoComponent, bool>> m_suitableTowerArguments = new();

    public List<Selectable> ConnectedTowers { get; private set; } = new();
    public int ConnectedTowerCount => ConnectedTowers.Count;

    public event Action<Selectable> TowerAdded;
    public event Action<Selectable> TowerRemoved;

    [Inject]
    public void Construct(SelectionService selectionService, ColorSettings colorSettings)
    {
        m_selectionService = selectionService;
        m_colorSettings = colorSettings;
    }

    protected virtual void Awake()
    {
        m_selectable.ObjectSelected += OnSelected;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Selectable selectable) &&
            selectable != m_selectable &&
            selectable.GameObjectToSelect.TryGetComponent(out TowerInfoComponent towerInfoComponent) &&
            m_suitableTowerArguments.All(isTowerSuitable => isTowerSuitable.Invoke(towerInfoComponent)))
        {
            selectable.Destroyed += RemoveTower;

            m_suitableTowers.Add(selectable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Selectable selectable))
        {
            selectable.Destroyed -= RemoveTower;

            RemoveTower(selectable);
        }
    }

    protected virtual void OnDestroy()
    {
        CancelTowerSelection();
        m_selectable.ObjectSelected -= OnSelected;
    }

    public void AddArgument(Func<TowerInfoComponent, bool> func) => m_suitableTowerArguments.Add(func);

    private void RemoveTower(Selectable selectable)
    {
        m_suitableTowers.TryRemove(selectable);

        if (ConnectedTowers.TryRemove(selectable))
        {
            TowerRemoved?.Invoke(selectable);
        }
    }

    private void OnSelected()
    {
        m_selectionService.LockSelection = true;
        m_selectionService.GameObjectSelected += OnGameObjectSelected;
        m_selectionService.GameObjectClickedWhileSelectionLocked += OnGameObjectSelectedWhileLocked;

        ShowAvailableTowers();

        foreach (Selectable selectable in ConnectedTowers)
        {
            selectable.OutlineObject(true, m_colorSettings.ConnectedOutline);
        }
    }

    private void ShowAvailableTowers()
    {
        foreach (Selectable selectable in m_suitableTowers.Where(tower => !ConnectedTowers.Contains(tower)))
        {
            bool focusTower = ConnectedTowerCount < SupportSelectorComponent.AllowedTowerAmountChanged.Value;
            Color color = focusTower ? m_colorSettings.FocusOutline : m_colorSettings.DefaultOutline;
            selectable.OutlineObject(focusTower, color);
        }
    }

    private void CancelTowerSelection()
    {
        m_selectionService.LockSelection = false;
        m_selectionService.GameObjectSelected -= OnGameObjectSelected;
        m_selectionService.GameObjectClickedWhileSelectionLocked -= OnGameObjectSelectedWhileLocked;

        foreach (Selectable selectable in m_suitableTowers)
        {
            selectable.OutlineObject(false, m_colorSettings.DefaultOutline);
        }
    }

    private void OnGameObjectSelected(Selectable selectable)
    {
        if (selectable != m_selectable)
        {
            CancelTowerSelection();
        }
    }

    private void OnGameObjectSelectedWhileLocked(Selectable selectable)
    {
        if (!m_suitableTowers.Contains(selectable))
        {
            return;
        }

        TowerClicked(selectable);
    }

    private void TowerClicked(Selectable selectable)
    {
        if (!ConnectedTowers.Contains(selectable))
        {
            if (ConnectedTowerCount == SupportSelectorComponent.AllowedTowerAmountChanged.Value)
            {
                return;
            }

            selectable.OutlineObject(true, m_colorSettings.ConnectedOutline);
            selectable.GameObjectToSelect.GetComponent<TowerInfoComponent>().ConnectedSupportTowers.AddSafely(m_towerInfoComponent.TowerID);
            ConnectedTowers.Add(selectable);
            TowerAdded?.Invoke(selectable);
        }
        else if (ConnectedTowers.Contains(selectable))
        {
            selectable.OutlineObject(true, m_colorSettings.FocusOutline);
            selectable.GameObjectToSelect.GetComponent<TowerInfoComponent>().ConnectedSupportTowers.Remove(m_towerInfoComponent.TowerID);
            ConnectedTowers.Remove(selectable);
            TowerRemoved?.Invoke(selectable);
        }

        ShowAvailableTowers();
    }
}

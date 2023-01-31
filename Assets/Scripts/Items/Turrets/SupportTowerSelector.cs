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

    private SelectionService m_selectionService;
    private ColorSettings m_colorSettings;
    private List<Type> m_suitableTowerTypes = new();
    private List<Selectable> m_suitableTowers = new();

    public event Action<Selectable> TowerClicked;
    public event Action<Selectable> SerializedTowerAdded;
    public event Action<Selectable> TowerRemoved;
    public event Action ThisTowerSelected;

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

    protected virtual void OnDestroy()
    {
        CancelTowerSelection();
        m_selectable.ObjectSelected -= OnSelected;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Selectable selectable) && selectable != m_selectable)
        {
            if (selectable.GameObjectToSelect.GetComponents<ITowerComponent>().Any(component => m_suitableTowerTypes.Contains(component.GetType())))
            {
                selectable.Destroyed += RemoveTower;

                m_suitableTowers.Add(selectable);

                if (selectable.GameObjectToSelect.GetComponent<TowerInfoComponent>().ConnectedSupportTowers.Contains(m_towerInfoComponent.TowerID))
                {
                    SerializedTowerAdded?.Invoke(selectable);
                }
            }
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

    private void RemoveTower(Selectable selectable)
    {
        m_suitableTowers.TryRemove(selectable);
        TowerRemoved?.Invoke(selectable);
    }

    public void AddSuitableType<T>() => m_suitableTowerTypes.Add(typeof(T));

    private void OnSelected()
    {
        m_selectionService.LockSelection = true;
        m_selectionService.GameObjectSelected += OnGameObjectSelected;
        m_selectionService.GameObjectClickedWhileSelectionLocked += OnGameObjectSelectedWhileLocked;

        foreach (Selectable selectable in m_suitableTowers)
        {
            selectable.OutlineObject(true, m_colorSettings.FocusOutline);
        }

        ThisTowerSelected?.Invoke();
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

        TowerClicked?.Invoke(selectable);
    }
}

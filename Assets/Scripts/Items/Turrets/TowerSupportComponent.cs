using Assets.Scripts.Interactables;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TowerSupportComponent<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private Selectable m_selectable;

    private SelectionService m_selectionService;
    private ColorSettings m_colorSettings;
    private Dictionary<Selectable, T> m_suitableTowers = new Dictionary<Selectable, T>();
    private List<Selectable> m_connectedTowers = new List<Selectable>();
    private bool m_isInTowerSelectionMode = false;

    [Inject]
    public void Construct(SelectionService selectionService, ColorSettings colorSettings)
    {
        m_selectionService = selectionService;
        m_colorSettings = colorSettings;
    }

    private void Awake()
    {
        m_selectable.Selected += OnSelected;
    }

    private void OnDestroy()
    {
        m_selectable.Selected -= OnSelected;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Selectable selectable) &&
            selectable != m_selectable &&
            selectable.GameObjectToSelect.TryGetComponent(out T towerInfoComponent))
        {
            m_suitableTowers.Add(selectable, towerInfoComponent);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Selectable selectable) &&
            selectable != m_selectable &&
            selectable.GameObjectToSelect.TryGetComponent(out T towerInfoComponent))
        {
            m_suitableTowers.Remove(selectable);
        }
    }

    private void OnSelected()
    {
        m_isInTowerSelectionMode = true;
        m_selectionService.LockSelection = true;
        m_selectionService.GameObjectSelected += OnGameObjectSelected;
        m_selectionService.GameObjectClickedWhileSelectionLocked += OnGameObjectSelectedWhileLocked;

        foreach (Selectable selectable in m_suitableTowers.Keys)
        {
            Color outlineColor = m_connectedTowers.Contains(selectable) ? m_colorSettings.ConnectedOutline : m_colorSettings.FocusOutline;
            selectable.OutlineObject(true, outlineColor);
        }
    }

    private void CancelTowerSelection()
    {
        m_isInTowerSelectionMode = false;
        m_selectionService.LockSelection = false;
        m_selectionService.GameObjectSelected -= OnGameObjectSelected;
        m_selectionService.GameObjectClickedWhileSelectionLocked -= OnGameObjectSelectedWhileLocked;

        foreach (Selectable selectable in m_suitableTowers.Keys)
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
        if (m_suitableTowers.ContainsKey(selectable) && !m_connectedTowers.Contains(selectable))
        {
            selectable.OutlineObject(true, m_colorSettings.ConnectedOutline);
            m_connectedTowers.Add(selectable);
        }
        else if (m_suitableTowers.ContainsKey(selectable) && m_connectedTowers.Contains(selectable))
        {
            selectable.OutlineObject(true, m_colorSettings.FocusOutline);
            m_connectedTowers.Remove(selectable);
        }
    }
}

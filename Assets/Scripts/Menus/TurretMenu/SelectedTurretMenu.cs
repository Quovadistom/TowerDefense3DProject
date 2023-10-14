using Assets.Scripts.Interactables;
using System;
using UnityEngine;
using Zenject;

public class SelectedTurretMenu : MonoBehaviour
{
    private SelectionService m_selectionService;

    public event Action<TowerModule> TurretDataChanged;
    public TowerModule SelectedTurret { get; private set; }

    [Inject]
    public void Construct(SelectionService selectionService)
    {
        m_selectionService = selectionService;
    }

    private void Awake()
    {
        m_selectionService.GameObjectSelected += OnGameObjectSelected;
    }

    private void Start()
    {
        this.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        m_selectionService.GameObjectSelected -= OnGameObjectSelected;
    }

    private void OnGameObjectSelected(Selectable selectable)
    {
        SelectedTurret = null;

        if (selectable != null && selectable.GameObjectToSelect.TryGetComponent(out TowerModule towerInfoComponent) && towerInfoComponent.IsTowerPlaced)
        {
            SelectedTurret = towerInfoComponent;
        }

        this.gameObject.SetActive(SelectedTurret != null);
        TurretDataChanged?.Invoke(SelectedTurret);
    }
}

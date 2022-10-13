using System;
using UnityEngine;
using Zenject;

public class SelectedTurretMenu : MonoBehaviour
{
    private SelectionService m_selectionService;

    public event Action TurretDataChanged;
    public TurretMediator SelectedTurret { get; private set; }

    [Inject]
    public void Construct(SelectionService selectionService)
    {
        m_selectionService = selectionService;
    }

    private void Awake()
    {
        m_selectionService.ObjectSelected += OnObjectSelected;

        this.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        m_selectionService.ObjectSelected -= OnObjectSelected;
    }

    private void OnObjectSelected(Component component)
    {
        TurretMediator turretBase = (TurretMediator)component;

        SelectedTurret = turretBase;

        TurretDataChanged?.Invoke();

        this.gameObject.SetActive(turretBase != null);
    }
}

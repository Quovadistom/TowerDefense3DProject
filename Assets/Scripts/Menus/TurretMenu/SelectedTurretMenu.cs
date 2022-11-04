using System;
using UnityEngine;
using Zenject;

public class SelectedTurretMenu : MonoBehaviour
{
    private SelectionService m_selectionService;

    public event Action<TurretInfoComponent> TurretDataChanged;
    public TurretInfoComponent SelectedTurret { get; private set; }

    [Inject]
    public void Construct(SelectionService selectionService)
    {
        m_selectionService = selectionService;
    }

    private void Awake()
    {
        m_selectionService.ObjectSelected += OnObjectSelected;
    }

    private void Start()
    {
        this.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        m_selectionService.ObjectSelected -= OnObjectSelected;
    }

    private void OnObjectSelected(Component component)
    {
        TurretInfoComponent turretBase = (TurretInfoComponent)component;

        SelectedTurret = turretBase;

        TurretDataChanged?.Invoke(SelectedTurret);

        this.gameObject.SetActive(turretBase != null);
    }
}

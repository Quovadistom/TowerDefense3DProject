using System;
using UnityEngine;
using Zenject;

public class SelectedTurretMenu : MonoBehaviour
{
    private SelectionService m_selectionService;

    public event Action<TowerInfoComponent> TurretDataChanged;
    public TowerInfoComponent SelectedTurret { get; private set; }

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

    private void OnGameObjectSelected(GameObject gameObject)
    {
        if  (gameObject == null)
        {
            this.gameObject.SetActive(false);
            return;
        }

        if (gameObject.TryGetComponent(out TowerInfoComponent towerInfoComponent))
        {
            SelectedTurret = towerInfoComponent;
            TurretDataChanged?.Invoke(SelectedTurret);
            this.gameObject.SetActive(true);
        }
    }
}

using UnityEngine;
using Zenject;

public class TowerButtonCollection : MonoBehaviour
{
    private TowerAvailabilityService m_towerAvailabilityService;
    private SpawnTowerButton.Factory m_buttonFactory;

    [Inject]
    public void Construct(TowerAvailabilityService towerAvailabilityService, SpawnTowerButton.Factory factory)
    {
        m_towerAvailabilityService = towerAvailabilityService;
        m_buttonFactory = factory;
    }

    private void Awake()
    {
        foreach (TowerAssets towerAssets in m_towerAvailabilityService.AvailableTowers)
        {
            SpawnTowerButton button = m_buttonFactory.Create();
            button.transform.SetParent(transform, false);
            button.TurretAssets = towerAssets;
        }
    }
}

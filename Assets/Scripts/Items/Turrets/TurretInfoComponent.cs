using UnityEngine;
using Zenject;

public class TurretInfoComponent : MonoBehaviour
{
    [SerializeField] private int m_cost;

    public TurretUpgradeTreeBase UpgradeTreeAsset;
    private LevelService m_levelService;

    public int Cost { get => m_cost; }

    [Inject]
    public void Construct(LevelService levelService)
    {
        m_levelService = levelService;
    }

    public void Awake()
    {
        m_levelService.Money -= m_cost;
    }

    public class Factory : PlaceholderFactory<TurretInfoComponent, TurretInfoComponent>
    {
    }
}

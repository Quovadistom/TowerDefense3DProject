using UnityEngine;
using Zenject;

public class TurretInfoComponent : CostComponent
{
    public TurretUpgradeTreeBase UpgradeTreeAsset;

    public class Factory : PlaceholderFactory<TurretInfoComponent, TurretInfoComponent>
    {
    }
}

public class CostComponent : MonoBehaviour
{
    [SerializeField] private int m_cost;
    private LevelService m_levelService;

    [Inject]
    public void Construct(LevelService levelService)
    {
        m_levelService = levelService;
    }

    public void SubtractCost()
    {
        m_levelService.Money -= m_cost;
    }

    public int Cost { get => m_cost; }
}
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelService : ServiceSerializationHandler<LevelServiceDTO>
{
    private List<Transform> m_waypoints = new List<Transform>();
    public IReadOnlyList<Transform> Waypoints => m_waypoints;

    private int m_health = 10;
    private int m_money = 1000;
    private GameBoostService m_gameBoostService;

    public LevelService(GameBoostService gameBoostService, SerializationService serializationService) : base(serializationService)
    {
        m_gameBoostService = gameBoostService;
        m_gameBoostService.AllGameBoostsApplied += OnGameBoostsApplied;
    }

    public override void Initialize()
    {
        base.Initialize();

        m_gameBoostService.ApplyBoosts();
    }

    private void OnGameBoostsApplied(GameBoostValues gameBoostValues)
    {
        Health += gameBoostValues.Health;
    }

    public event Action<int> HealthChanged;
    public event Action<int> MoneyChanged;

    public int Health
    {
        get { return m_health; }
        set
        {
            m_health = Mathf.Max(0, value);
            HealthChanged?.Invoke(m_health);
        }
    }

    public int Money
    {
        get { return m_money; }
        set
        {
            m_money = value;
            MoneyChanged?.Invoke(m_money);
        }
    }

    public void SetWaypoints(List<Transform> waypoints)
    {
        m_waypoints = waypoints;
    }

    protected override Guid Id => Guid.Parse("7c631af4-9fff-4572-86aa-1f2178772e80");

    protected override void ConvertDtoBack(LevelServiceDTO dto)
    {
        Health = dto.Health;
        Money = dto.Money;
    }

    protected override void ConvertDto()
    {
        Dto.Health = Health;
        Dto.Money = Money;
    }
}

[Serializable]
public class LevelServiceDTO
{
    public int Health;
    public int Money;
}

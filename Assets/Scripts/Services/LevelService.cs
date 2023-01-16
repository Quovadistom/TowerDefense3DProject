using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelService : ServiceSerializationHandler<LevelServiceDTO>
{
    private List<Transform> m_waypoints = new List<Transform>();
    public IReadOnlyList<Transform> Waypoints => m_waypoints;

    private int m_health = 10;
    private int m_money = 1000;

    public LevelService(SerializationService serializationService) : base(serializationService)
    {
    }

    public event Action<int> HealthChanged;
    public event Action<int> MoneyChanged;

    public int Health
    {
        get { return m_health; }
        set
        {
            m_health = value;
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

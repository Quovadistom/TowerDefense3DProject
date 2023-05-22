using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public class GameBoostService : ServiceSerializationHandler<GameBoostServiceDto>
{
    private string[] m_gameBoosts = new string[5];
    private BoostAvailabilityService m_boostAvailabilityService;
    private GameUpgradeValues m_gameBoostValues;

    public event Action<int, string> GameBoostActivated;
    public event Action<GameUpgradeValues> AllGameBoostsApplied;

    public ICollection<string> GameBoosts => m_gameBoosts.AsReadOnlyCollection();

    protected override Guid Id => Guid.Parse("0afb0eb2-09db-4a9c-92f9-9a20adc4a339");

    public GameBoostService(BoostAvailabilityService boostAvailabilityService, SerializationService serializationService, DebugSettings debugSettings) : base(serializationService, debugSettings)
    {
        m_boostAvailabilityService = boostAvailabilityService;
    }

    public void RemoveBoost(int index) => AddBoost(index, null);

    public void AddBoost(int index, GameUpgradeBase boost)
    {
        m_gameBoosts[index] = boost.ID;
        m_boostAvailabilityService.RemoveAvailableBoost(boost.ID);
        GameBoostActivated?.Invoke(index, boost.Name);
    }

    public void ApplyBoosts()
    {
        foreach (string boostID in m_gameBoosts.Where(id => !string.IsNullOrEmpty(id)))
        {
            if (m_boostAvailabilityService.TryGetGameBoost(boostID, out var boostInfo))
            {
                m_gameBoostValues = boostInfo.ApplyUpgrade(m_gameBoostValues);
            }
        }

        AllGameBoostsApplied?.Invoke(m_gameBoostValues);
    }

    protected override void ConvertDto()
    {
        Dto.SelectedGameBoosts = m_gameBoosts;
    }

    protected override void ConvertDtoBack(GameBoostServiceDto dto)
    {
        m_gameBoosts = dto.SelectedGameBoosts;
    }
}

public class GameBoostServiceDto
{
    public string[] SelectedGameBoosts;
}

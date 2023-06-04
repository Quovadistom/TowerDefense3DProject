using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public class GameBoostService : ServiceSerializationHandler<GameBoostServiceDto>
{
    private string[] m_gameBoosts = new string[5];
    private BoostService m_boostService;
    private BoostAvailabilityService m_boostAvailabilityService;

    public event Action<int, string> GameBoostActivated;

    public ICollection<string> GameBoosts => m_gameBoosts.AsReadOnlyCollection();

    protected override Guid Id => Guid.Parse("0afb0eb2-09db-4a9c-92f9-9a20adc4a339");

    public GameBoostService(BoostService boostService, BoostAvailabilityService boostAvailabilityService, SerializationService serializationService, DebugSettings debugSettings) : base(serializationService, debugSettings)
    {
        m_boostService = boostService;
        m_boostAvailabilityService = boostAvailabilityService;
    }

    public void RemoveBoost(int index) => AddBoost(index, null);

    public void AddBoost(int index, BoostContainer boost)
    {
        m_gameBoosts[index] = boost.Name;
        m_boostAvailabilityService.RemoveAvailableBoost(boost.Name);
        m_boostService.AddUpgrade(boost);
        GameBoostActivated?.Invoke(index, boost.Name);
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

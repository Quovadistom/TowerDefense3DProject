using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public class GameBoostService : ServiceSerializationHandler<GameBoostServiceDto>
{
    private Guid[] m_gameBoosts = new Guid[5];
    private BoostService m_boostService;
    private BoostAvailabilityService m_boostAvailabilityService;

    public event Action<int, Guid> GameBoostActivated;

    public ICollection<Guid> GameBoosts => m_gameBoosts.AsReadOnlyCollection();

    protected override Guid Id => Guid.Parse("0afb0eb2-09db-4a9c-92f9-9a20adc4a339");

    public GameBoostService(BoostService boostService, BoostAvailabilityService boostAvailabilityService, SerializationService serializationService, DebugSettings debugSettings) : base(serializationService, debugSettings)
    {
        m_boostService = boostService;
        m_boostAvailabilityService = boostAvailabilityService;
    }

    public void RemoveBoost(int index) => AddBoost(index, null);

    public void AddBoost(int index, BoostContainer boost)
    {
        m_gameBoosts[index] = boost.ID;
        m_boostAvailabilityService.RemoveAvailableBoost(boost.ID);
        m_boostService.AddUpgrade(boost);
        GameBoostActivated?.Invoke(index, boost.ID);
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
    public Guid[] SelectedGameBoosts;
}

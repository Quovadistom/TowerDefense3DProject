using System;
using System.Collections.Generic;
using UnityEngine;

public class InflationService : ServiceSerializationHandler<InflationServiceDTO>
{
    public event Action InflationChanged;

    private List<InflationData> m_inflationDatas = new();

    private float m_gameInflationPercentage = 0;
    public float GameInflationPercentage
    {
        get => m_gameInflationPercentage;
        set
        {
            m_gameInflationPercentage = value;
            InflationChanged?.Invoke();
        }
    }

    public InflationService(SerializationService serializationService, DebugSettings debugSettings) : base(serializationService, debugSettings)
    {

    }

    public int CalculateInflation(TowerModule towerModule)
    {
        float totalInflation = GameInflationPercentage;

        foreach (var data in m_inflationDatas)
        {
            try
            {
                //Type moduleType = Type.GetType(data.ModuleName);
                //if (towerModule.HasComponent(moduleType))
                //{
                //    totalInflation += data.InflationPercentage;
                //}
            }
            catch (Exception ex)
            {
                Debug.LogError($"Type is not found {ex.Message}");
            }
        }

        return towerModule.TowerCost.AddPercentage(totalInflation);
    }

    protected override Guid Id => Guid.Parse("21ab3cb6-6fa3-4ce1-9adc-201fe272ad29");

    protected override void ConvertDto()
    {
        Dto.GameInflationPercentage = GameInflationPercentage;
    }

    protected override void ConvertDtoBack(InflationServiceDTO dto)
    {
        GameInflationPercentage = dto.GameInflationPercentage;
    }
}

[Serializable]
public class InflationServiceDTO
{
    public float GameInflationPercentage;
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

public class InflationService : ServiceSerializationHandler<InflationServiceDTO>
{
    public event Action InflationChanged;

    private InflationCollection m_inflationCollection;

    private List<InflationData> m_inflationDatas = new();

    public InflationService(InflationCollection inflationCollection,
        SerializationService serializationService, DebugSettings debugSettings) : base(serializationService, debugSettings)
    {
        m_inflationCollection = inflationCollection;

        InflationData testData = inflationCollection.GetInflationData<GeneralInflation>();
        testData.InflationPercentage = 50;
        m_inflationDatas.Add(testData);
    }

    public float CalculateInflationPercentage(ModuleParent towerModule)
    {
        float totalInflation = 0;

        foreach (InflationData data in m_inflationDatas)
        {
            if (data.IsModelParentSuitable(towerModule))
            {
                totalInflation += data.InflationPercentage;
            }
        }

        return totalInflation;
    }

    public float CalculateInflationPercentage(ModuleModificationBase moduleModificationBase, ModuleParent towerModule)
    {
        float totalInflation = 0;

        foreach (InflationData data in m_inflationDatas)
        {
            if (data.IsModificationSuitable(moduleModificationBase, towerModule))
            {
                totalInflation += data.InflationPercentage;
            }
        }

        return totalInflation;
    }

    protected override Guid Id => Guid.Parse("21ab3cb6-6fa3-4ce1-9adc-201fe272ad29");

    protected override void ConvertDto()
    {
        Dto.InflationDatas = m_inflationDatas.ToDictionary(x => x.Guid, x => x.InflationPercentage);
    }

    protected override void ConvertDtoBack(InflationServiceDTO dto)
    {
        m_inflationDatas = dto.InflationDatas.Select(x =>
        {
            InflationData data = m_inflationCollection.GetInflationData(x.Key);
            data.InflationPercentage = x.Value;
            return data;
        }).ToList();
    }
}

[Serializable]
public class InflationServiceDTO
{
    public Dictionary<Guid, float> InflationDatas;
}

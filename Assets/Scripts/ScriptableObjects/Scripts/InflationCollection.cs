using System;
using System.Collections.Generic;
using System.Linq;

public enum InflationType
{
    General,
    Fire,
    Corrosion,
    Water,
    Electricity,
    Laser
}

public class InflationCollection
{
    private List<InflationData> m_inflationDataList = new();

    public InflationCollection()
    {
        m_inflationDataList.AddRange(new List<InflationData>()
        {
            new GeneralInflation(Guid.Parse("ba836721-6ee5-4cf6-b386-b6d1e08a2483")),
            new StatusEffectInflation(Guid.Parse("4afd6096-ecc2-4efe-b52f-f6c0189a9780"),
                EffectType.Fire),
            new StatusEffectInflation(Guid.Parse("fb59f069-7c67-47c8-b717-1cc940eb911a"),
                EffectType.Corrosion),
            new StatusEffectInflation(Guid.Parse("ad484b4f-c030-4e76-a554-a1b7eb55b7de"),
                EffectType.Water),
            new StatusEffectInflation(Guid.Parse("9fba3109-df4e-4610-9076-0ebc547e56fd"),
                EffectType.Electricity)
        });
    }

    public InflationData GetInflationData(Guid guid) => m_inflationDataList.First(data => data.Guid == guid);

    public T GetInflationData<T>() where T : InflationData
    {
        return (T)m_inflationDataList.First(data => data.GetType() == typeof(T));
    }
}

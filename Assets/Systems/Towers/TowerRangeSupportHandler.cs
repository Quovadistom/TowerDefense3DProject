using System;

public class TowerRangeSupportHandler : TowerFloatSupportHandler<RangeModule>
{
    protected override Action<RangeModule, float> ComponentFunc => (component, value) =>
    {
        component.RangeValue.BuffPercentage += value;
    };
}

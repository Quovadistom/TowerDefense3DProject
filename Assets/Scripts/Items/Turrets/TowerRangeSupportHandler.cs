using System;

public class TowerRangeSupportHandler : TowerFloatSupportHandler<RangeComponent>
{
    protected override Action<RangeComponent, float> ComponentFunc => (component, value) =>
    {
        component.BuffPercentage += value;
    };
}

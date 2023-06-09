using System;

public class TowerRangeBuff : Upgrade<RangeComponent>
{
    public float Percentage;

    protected override Action<RangeComponent> ComponentAction => (component) =>
    {
        component.BuffPercentage += Percentage;
    };
}
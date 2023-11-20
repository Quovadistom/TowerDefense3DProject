using System;

public class TowerRangeBuff : Modification<RangeModule>
{
    public float Percentage;

    protected override Action<RangeModule> ComponentAction => (component) =>
    {
        component = null;
        //component.RangeValue.BuffPercentage += Percentage;
    };
}
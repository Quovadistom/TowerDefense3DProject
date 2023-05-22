public class TowerRangeSupportHandler : TowerFloatSupportHandler<TurretRangeComponent>
{
    protected override float GetFloat(TurretRangeComponent component) => component.Range;

    protected override void SetFLoat(TurretRangeComponent component, float value) => component.Range = value;
}
